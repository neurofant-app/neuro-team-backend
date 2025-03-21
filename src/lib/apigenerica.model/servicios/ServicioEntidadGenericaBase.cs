﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using apigenerica.model.modelos;
using extensibilidad.metadatos;
using apigenerica.model.reflectores;
using Microsoft.Extensions.Caching.Distributed;
using apigenerica.model.abstracciones;
using comunes.primitivas;
using apigenerica.model.interpretes;
using System.Collections.Specialized;

namespace apigenerica.model.servicios;

#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously

/// <summary>
/// Definición para la API genérica de entidades
/// </summary>
/// <typeparam name="DTOFull">Objeto tal como se almacena en el repositorio</typeparam>
/// <typeparam name="DTOInsert">DTO utilizado para insertar</typeparam>
/// <typeparam name="DTOUpdate">DTO utilizado para actualizar</typeparam>
/// <typeparam name="DTODespliegue">DTO utilizado para el despligue neutral en UI</typeparam>
/// <typeparam name="TipoId">Tipo de dato utilizado para el identificador de la entidad</typeparam>
public abstract class ServicioEntidadGenericaBase<DTOFull, DTOInsert, DTOUpdate, DTODespliegue, TipoId>
    where DTOFull : class
    where DTODespliegue : class
    where DTOUpdate : class
    where DTOInsert : class
{
    protected IInterpreteConsulta? interpreteConsulta;
    protected DbSet<DTOFull> _dbSetFull;
    protected DbContext _db;
    protected ContextoUsuario? _contextoUsuario;
    protected ILogger _logger;
    protected IReflectorEntidadesAPI reflectorEntidades;
    protected IDistributedCache _cache;
    /// <summary>
    /// Constructor de la clase base
    /// </summary>
    /// <param name="db"></param>
    /// <param name="dbSetFull"></param>
    /// <param name="logger"></param>
    /// <param name="reflectorEntidades"></param>
    public ServicioEntidadGenericaBase(DbContext db, DbSet<DTOFull> dbSetFull, ILogger logger, IReflectorEntidadesAPI reflectorEntidades, IDistributedCache cache)
    {
        _cache = cache;
        _dbSetFull = dbSetFull;
        _db = db;
        _logger = logger;
        this.reflectorEntidades = reflectorEntidades;
    }

    public JsonSerializerOptions JsonAPIDefaults()
    {
        return new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
    }

    public virtual async Task<RespuestaPayload<DTODespliegue>> Insertar(DTOInsert data, StringDictionary? parametros = null)
    {
        var respuesta = new RespuestaPayload<DTODespliegue>();

        try
        {
            var resultadoValidacion = await ValidarInsertar(data);
            if (resultadoValidacion.Valido)
            {
                var entidad = ADTOFull(data);
                _dbSetFull.Add(entidad);
                await _db.SaveChangesAsync();

                respuesta.Ok = true;
                respuesta.HttpCode = HttpCode.Ok;
                respuesta.Payload = ADTODespliegue(entidad);
            }
            else
            {
                respuesta.Error = resultadoValidacion.Error;
                respuesta.HttpCode = resultadoValidacion.Error?.HttpCode ?? HttpCode.None;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError($"Insertar {ex.Message}");
            _logger.LogError($"{ex}");

            respuesta.Error = new ErrorProceso() { Codigo = "", HttpCode = HttpCode.ServerError, Mensaje = ex.Message };
            respuesta.HttpCode = HttpCode.ServerError;
        }

        return respuesta;
    }

    public virtual async Task<Entidad>? Metadatos(string Tipo, StringDictionary? parametros = null)
    {
        await Task.Delay(0);
        Entidad entidad = new();
        string entidadCache = await _cache.GetStringAsync($"Entidad-{Tipo}");
        if (!string.IsNullOrEmpty(entidadCache))
        {
            entidad = JsonSerializer.Deserialize<Entidad>(entidadCache);
        }
        else
        {
            entidad = reflectorEntidades.ObtieneEntidadUI(typeof(DTOFull), typeof(DTOInsert), typeof(DTOUpdate), typeof(DTODespliegue));
            foreach (var propiedad in entidad.Propiedades.Where(p => p.Tipo == TipoDatos.ListaSeleccionMultiple || p.Tipo == TipoDatos.ListaSeleccionSimple))
            {
                if (propiedad.Lista!.DatosRemotos = false && !string.IsNullOrEmpty(propiedad.Lista.Id))
                {
                    propiedad.Lista.Elementos = await ObtieneListaLocal(propiedad.Lista.Id, propiedad.Lista.Ordenamiento);
                }
            }

            await _cache.SetStringAsync($"Entidad-{Tipo}", JsonSerializer.Serialize(entidad));
        }
        return entidad!;
    }

    public virtual async Task<Respuesta> Actualizar(string id, DTOUpdate data, StringDictionary? parametros = null)
    {
        var respuesta = new Respuesta();
        try
        {
            if (string.IsNullOrEmpty(id) || data == null)
            {
                respuesta.HttpCode = HttpCode.BadRequest;
                return respuesta;
            }


            DTOFull actual = _dbSetFull.Find(id);

            if (actual == null)
            {
                respuesta.HttpCode = HttpCode.NotFound;
                return respuesta;
            }

            var resultadoValidacion = await ValidarActualizar(id, data, actual);
            if (resultadoValidacion.Valido)
            {
                var entidad = ADTOFull(data, actual);
                _dbSetFull.Update(entidad);
                await _db.SaveChangesAsync();

                respuesta.Ok = true;
                respuesta.HttpCode = HttpCode.Ok;
            }
            else
            {
                respuesta.Error = resultadoValidacion.Error;
                respuesta.HttpCode = resultadoValidacion.Error?.HttpCode ?? HttpCode.None;
            }

        }
        catch (Exception ex)
        {
            _logger.LogError($"Insertar {ex.Message}");
            _logger.LogError($"{ex}");

            respuesta.Error = new ErrorProceso() { Codigo = "", HttpCode = HttpCode.ServerError, Mensaje = ex.Message };
            respuesta.HttpCode = HttpCode.ServerError;
        }

        return respuesta;
    }

    public virtual async Task<Respuesta> Eliminar(string id, StringDictionary? parametros = null, bool forzarEliminacion = false)
    {
        var respuesta = new Respuesta();
        try
        {

            if (string.IsNullOrEmpty(id))
            {
                respuesta.HttpCode = HttpCode.BadRequest;
                return respuesta;
            }

            DTOFull actual = _dbSetFull.Find(id);
            if (actual == null)
            {
                respuesta.HttpCode = HttpCode.NotFound;
                return respuesta;
            }

            var resultadoValidacion = await ValidarEliminacion(id, actual, forzarEliminacion);
            if (resultadoValidacion.Valido)
            {

                _dbSetFull.Remove(actual);
                await _db.SaveChangesAsync();

                respuesta.Ok = true;
                respuesta.HttpCode = HttpCode.Ok;
            }
            else
            {
                respuesta.Error = resultadoValidacion.Error;
                respuesta.HttpCode = resultadoValidacion.Error?.HttpCode ?? HttpCode.None;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError($"Insertar {ex.Message}");
            _logger.LogError($"{ex}");

            respuesta.Error = new ErrorProceso() { Codigo = "", HttpCode = HttpCode.ServerError, Mensaje = ex.Message };
            respuesta.HttpCode = HttpCode.ServerError;
        }
        return respuesta;
    }

    public virtual async Task<RespuestaPayload<DTOFull>> UnicaPorId(string id, StringDictionary? parametros = null)
    {
        var respuesta = new RespuestaPayload<DTOFull>();
        try
        {
            DTOFull actual = await _dbSetFull.FindAsync(id);
            if (actual == null)
            {
                respuesta.HttpCode = HttpCode.NotFound;
                return respuesta;
            }

            respuesta.Ok = true;
            respuesta.HttpCode = HttpCode.Ok;
            respuesta.Payload = actual;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Insertar {ex.Message}");
            _logger.LogError($"{ex}");

            respuesta.Error = new ErrorProceso() { Codigo = "", HttpCode = HttpCode.ServerError, Mensaje = ex.Message };
            respuesta.HttpCode = HttpCode.ServerError;
        }
        return respuesta;
    }

    public virtual async Task<RespuestaPayload<PaginaGenerica<DTOFull>>> Pagina(Consulta consulta, StringDictionary? parametros = null)
    {
        RespuestaPayload<PaginaGenerica<DTOFull>> respuesta = new();
        try
        {
            if (interpreteConsulta == null)
            {
                respuesta.HttpCode = HttpCode.UnprocessableEntity;
                respuesta.Error = new ErrorProceso() { HttpCode = HttpCode.UnprocessableEntity, Codigo = "SIN-INTERPRETE-CONSULTA", Mensaje = "No hay un interprete de consulta definido" };
                return respuesta;
            }

            var pagina = await ObtienePaginaElementos(consulta, parametros);

            respuesta.Payload = pagina;
            respuesta.Ok = true;
            respuesta.HttpCode = HttpCode.Ok;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Insertar {ex.Message}");
            _logger.LogError($"{ex}");

            respuesta.Error = new ErrorProceso() { Codigo = "", HttpCode = HttpCode.ServerError, Mensaje = ex.Message };
            respuesta.HttpCode = HttpCode.ServerError;
        }
        return respuesta;
    }


    /// <summary>
    /// Este método siempre debe implementarse en la clase derivada
    /// </summary>
    /// <param name="consulta"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>

    public virtual async Task<PaginaGenerica<DTOFull>> ObtienePaginaElementos(Consulta consulta, StringDictionary? parametros = null)
    {
        Entidad entidad = reflectorEntidades.ObtieneEntidad(typeof(DTOFull));
        var Elementos = Enumerable.Empty<DTOFull>().AsQueryable();

        if (consulta.Filtros.Count > 0)
        {
            var predicateBody = interpreteConsulta.CrearConsultaExpresion<DTOFull>(consulta, entidad);

            if (predicateBody != null)
            {
                var RConsulta = _dbSetFull.AsQueryable().Provider.CreateQuery<DTOFull>(predicateBody.getWhereExpression(_dbSetFull.AsQueryable().Expression));

                Elementos = RConsulta.OrdenarPor(consulta.Paginado.ColumnaOrdenamiento ?? "Id", consulta.Paginado.Ordenamiento ?? modelos.Ordenamiento.asc);
            }
        }
        else
        {
            var RConsulta = _dbSetFull.AsQueryable();
            Elementos = RConsulta.OrdenarPor(consulta.Paginado.ColumnaOrdenamiento ?? "Id", consulta.Paginado.Ordenamiento ?? modelos.Ordenamiento.asc);

        }
        return await Elementos.PaginadoAsync(consulta);
    }

    public virtual ContextoUsuario? ObtieneContextoUsuario()
    {
        return _contextoUsuario;
    }

    public virtual void EstableceContextoUsuario(ContextoUsuario? contexto)
    {
        _contextoUsuario = contexto;
    }

    public virtual ContextoUsuario? ObtieneContextoUsuario(ContextoUsuario? contexto)
    {
        return _contextoUsuario;
    }

    public virtual async Task<ResultadoValidacion> ValidarActualizar(string id, DTOUpdate actualizacion, DTOFull original)
    {
        return new ResultadoValidacion() { Valido = true };
    }

    public virtual async Task<ResultadoValidacion> ValidarEliminacion(string id, DTOFull original, bool forzarEliminacion = false)
    {
        return new ResultadoValidacion() { Valido = true };
    }

    public virtual async Task<ResultadoValidacion> ValidarInsertar(DTOInsert data)
    {
        return new ResultadoValidacion() { Valido = true };
    }


    public virtual async Task<RespuestaPayload<DTODespliegue>> UnicaPorIdDespliegue(string id, StringDictionary? parametros = null)
    {
        RespuestaPayload<DTODespliegue> respuesta = new RespuestaPayload<DTODespliegue>();

        try
        {
            var resultado = await UnicaPorId(id, parametros);

            respuesta.Ok = resultado.Ok;

            if (resultado.Ok)
            {
                respuesta.Payload = ADTODespliegue((DTOFull)resultado.Payload);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError($"UnicaPorIdDespliegue {ex.Message}");
            _logger.LogError($"{ex}");

            respuesta.Error = new ErrorProceso() { Codigo = "", HttpCode = HttpCode.ServerError, Mensaje = ex.Message };
            respuesta.HttpCode = HttpCode.ServerError;
        }
        return respuesta;
    }

    public virtual async Task<RespuestaPayload<PaginaGenerica<DTODespliegue>>> PaginaDespliegue(Consulta consulta, StringDictionary? parametros = null)
    {
        RespuestaPayload<PaginaGenerica<DTODespliegue>> respuesta = new RespuestaPayload<PaginaGenerica<DTODespliegue>>();

        try
        {
            var resultado = await Pagina(consulta, parametros);

            respuesta.Ok = resultado.Ok;

            if (resultado.Ok)
            {
                PaginaGenerica<DTODespliegue> pagina = new()
                {
                    ConsultaId = Guid.NewGuid().ToString(),
                    Elementos = new List<DTODespliegue>(),
                    Milisegundos = 0,
                    Paginado = new Paginado() { Indice = 0, Tamano = ((PaginaGenerica<DTOFull>)resultado.Payload).Paginado.Tamano, Ordenamiento = consulta.Paginado.Ordenamiento, ColumnaOrdenamiento = consulta.Paginado.ColumnaOrdenamiento },
                    Total = ((PaginaGenerica<DTOFull>)resultado.Payload).Total
                };

                foreach (DTOFull item in ((PaginaGenerica<DTOFull>)resultado.Payload).Elementos)
                {
                    pagina.Elementos.Add(ADTODespliegue(item));
                }
                respuesta.Payload = pagina;
            }

        }
        catch (Exception ex)
        {
            _logger.LogError($"Insertar {ex.Message}");
            _logger.LogError($"{ex}");

            respuesta.Error = new ErrorProceso() { Codigo = "", HttpCode = HttpCode.ServerError, Mensaje = ex.Message };
            respuesta.HttpCode = HttpCode.ServerError;
        }

        return respuesta;
    }

    public virtual Task<RespuestaPayload<List<NodoArbol<object>>>> Arbol(string? raizId = null, bool parcial = false, bool incluirPayload = false, StringDictionary? parametros = null)
    {
        throw new NotImplementedException();
    }

    public virtual Task<RespuestaPayload<List<ParClaveTexto>>> TextoIds(List<string> Ids, StringDictionary? parametros = null)
    {
        throw new NotImplementedException();
    }

    public virtual Task<RespuestaPayload<List<object>>> InsertarMultipleAPI(JsonElement data, StringDictionary? parametros = null)
    {
        throw new NotImplementedException();
    }

    public virtual Task<Respuesta> EliminarAPI(List<string> ids, StringDictionary? parametros = null, bool forzarEliminacion = false)
    {
        throw new NotImplementedException();
    }

    public virtual Filtro? FiltroPorEntidadPadre(string tipoPadre, string padreId)
    {
        return null;
    }

    public virtual DTOFull ADTOFull(DTOInsert data)
    {
        throw new NotImplementedException();
    }

    public virtual DTOFull ADTOFull(DTOUpdate actualizacion, DTOFull actual)
    {
        throw new NotImplementedException();
    }


    public virtual DTODespliegue ADTODespliegue(DTOFull data)
    {
        throw new NotImplementedException();
    }


    public virtual Entidad EntidadInsert()
    {
        throw new NotImplementedException();
    }

    public virtual Entidad EntidadRepo()
    {
        throw new NotImplementedException();
    }

    public virtual Entidad EntidadUpdate()
    {
        throw new NotImplementedException();
    }

    public virtual Entidad EntidadDespliegue()
    {
        throw new NotImplementedException();
    }

    public virtual Task<List<ElementoListaI18N>> ObtieneListaLocal(string clave, OrdenamientoLista ordenamiento)
    {

#if DEBUG
        return Task.FromResult(new List<ElementoListaI18N>()
        {
            new ElementoListaI18N() { Id ="1", Nombre = new List<DatoI18N<string>>{new DatoI18N<string>() { Idioma = "es-Mx", Valor = "Uno"} }, Posicion = 1, Valor = "1"},
            new ElementoListaI18N() { Id ="2", Nombre = new List<DatoI18N<string>>{new DatoI18N<string>() { Idioma = "es-Mx", Valor = "Dos"} }, Posicion = 2, Valor = "2"},
            new ElementoListaI18N() { Id ="3", Nombre = new List<DatoI18N<string>>{new DatoI18N<string>() { Idioma = "es-Mx", Valor = "Tres"} }, Posicion = 3, Valor = "3"},
            new ElementoListaI18N() { Id ="4", Nombre = new List<DatoI18N<string>>{new DatoI18N<string>() { Idioma = "es-Mx", Valor = "Cuatro"} }, Posicion = 4, Valor = "4"},
            new ElementoListaI18N() { Id ="5", Nombre = new List<DatoI18N<string>>{new DatoI18N<string>() { Idioma = "es-Mx", Valor = "Cinco"} }, Posicion = 5, Valor = "5"}
        });
#else
        throw new NotImplementedException();
#endif

    }


    public virtual async Task<string> PorContar(string query)
    {
        if (query.Contains("ORDER"))
        {
            query = query.Split("ORDER")[0];
            query = $"{query.Replace("*", "count(*)")}";
        }

        return query;
    }
}

#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously

#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.