#pragma warning disable CS8603 // Possible null reference return.
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
using apigenerica.model.interpretes;
using apigenerica.model.modelos;
using apigenerica.model.reflectores;
using apigenerica.model.servicios;
using comunes.primitivas;
using comunes.primitivas.configuracion.mongo;
using disenocurricular.model;
using disenocurricular.services.dbcontext;
using extensibilidad.metadatos;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using System.Collections.Specialized;
using System.Text.Json;

namespace disenocurricular.services.temario.tema;

[ServicioEntidadAPI(typeof(Tema))]
public class ServicioTema : ServicioEntidadGenericaBase<Tema, Tema, Tema, Tema, string>,
    IServicioEntidadAPI, IServicioTema
{
    private readonly ILogger<ServicioTema> _logger;
    private readonly IReflectorEntidadesAPI reflector;
    private Temario? temario;
    private DbSet<Temario> _dbSetTemarios;

    public ServicioTema(ILogger<ServicioTema> logger,
        IServicionConfiguracionMongo configuracionMongo,
        IReflectorEntidadesAPI reflector,
        IDistributedCache cache) : base(null, null, logger, reflector, cache)
    {
        this._logger = logger;
        this.reflector = reflector;
        interpreteConsulta = new InterpreteConsultaExpresiones();
        var configuracionEntidad = configuracionMongo.ConexionEntidad(MongoDbContextDisenoCurricular.NOMBRE_COLECCION_TEMARIOS);
        if (configuracionEntidad == null)
        {
            string err = $"No existe configuracion de mongo para '{MongoDbContextDisenoCurricular.NOMBRE_COLECCION_TEMARIOS}'";
            _logger.LogError(err);
            throw new Exception(err);
        }
        try
        {
            _logger.LogDebug($"Mongo DB {configuracionEntidad.Esquema} coleccion {configuracionEntidad.Esquema} utilizando conexion default {string.IsNullOrEmpty(configuracionEntidad.Conexion)}");
            var cadenaConexion = string.IsNullOrEmpty(configuracionEntidad.Conexion) && string.IsNullOrEmpty(configuracionMongo.ConexionDefault())
                ? configuracionMongo.ConexionDefault()
                : string.IsNullOrEmpty(configuracionEntidad.Conexion)
                    ? configuracionMongo.ConexionDefault()
                    : configuracionEntidad.Conexion;
            var client = new MongoClient(cadenaConexion);

            _db = MongoDbContextDisenoCurricular.Create(client.GetDatabase(configuracionEntidad.Esquema));
            _dbSetTemarios = ((MongoDbContextDisenoCurricular)_db).Temarios;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al inicializar mongo para {coleccion}'", MongoDbContextDisenoCurricular.NOMBRE_COLECCION_TEMARIOS);
            throw;
        }
    }

    private MongoDbContextDisenoCurricular DB { get { return (MongoDbContextDisenoCurricular)_db; } }

    public bool RequiereAutenticacion => true;

    public async Task<Respuesta> ActualizarAPI(object id, JsonElement data, StringDictionary? parametros = null)
    {
        _logger.LogDebug("ServicioMiembro-ActualizarAPI-{data}", data);
        var update = data.Deserialize<Tema>(JsonAPIDefaults());
        Respuesta respuesta = await this.Actualizar((string)id, update, parametros);
        _logger.LogDebug("ServicioMiembro-ActualizarAPI resultado {ok} {code} {error}", respuesta!.Ok, respuesta!.HttpCode, respuesta.Error);
        return respuesta;
    }

    public async Task<Respuesta> EliminarAPI(object id, StringDictionary? parametros = null)
    {
        _logger.LogDebug("ServicioMiembro-EliminarAPI");
        Respuesta respuesta = await this.Eliminar((string)id, parametros);
        _logger.LogDebug("ServicioMiembro-EliminarAPI resultado {ok} {code} {error}", respuesta!.Ok, respuesta!.HttpCode, respuesta.Error);
        return respuesta;
    }

    public Entidad EntidadDespliegueAPI()
    {
        _logger.LogDebug("ServicioMiembro-EntidadDespliegueAPI");
        return this.EntidadDespliegue();
    }

    public Entidad EntidadInsertAPI()
    {
        _logger.LogDebug("ServicioMiembro-EntidadInsertAPI");
        return this.EntidadInsert();
    }

    public Entidad EntidadRepoAPI()
    {
        _logger.LogDebug("ServicioMiembro-EntidadRepoAPI");
        return this.EntidadRepo();
    }

    public Entidad EntidadUpdateAPI()
    {
        _logger.LogDebug("ServicioMiembro-EntidadUpdateAPI");
        return this.EntidadUpdate();
    }

    public void EstableceContextoUsuarioAPI(ContextoUsuario contexto)
    {
        _logger.LogDebug("ServicioMiembro-EstableceContextoUsuarioAPI");
        this.EstableceContextoUsuario(contexto);
    }

    public async Task<RespuestaPayload<object>> InsertarAPI(JsonElement data, StringDictionary? parametros = null)
    {
        _logger.LogDebug("ServicioMiembro-InsertarAPI-{data}", data);
        var add = data.Deserialize<Tema>(JsonAPIDefaults());
        var temp = await this.Insertar(add, parametros);
        RespuestaPayload<object> respuesta = JsonSerializer.Deserialize<RespuestaPayload<object>>(JsonSerializer.Serialize(temp));
        _logger.LogDebug("ServicioMiembro-InsertarAPI resultado {ok} {code} {error}", respuesta!.Ok, respuesta!.HttpCode, respuesta.Error);
        return respuesta;
    }

    public ContextoUsuario? ObtieneContextoUsuarioAPI()
    {
        _logger.LogDebug("ServicioMiembro-ObtieneContextoUsuarioAPI");
        return this._contextoUsuario;
    }

    public async Task<RespuestaPayload<PaginaGenerica<object>>> PaginaAPI(Consulta consulta, StringDictionary? parametros = null)
    {
        _logger.LogDebug("ServicioMiembro-PaginaAPI-{consulta}", consulta);
        var temp = await this.Pagina(consulta, parametros);
        RespuestaPayload<PaginaGenerica<object>> respuesta = JsonSerializer.Deserialize<RespuestaPayload<PaginaGenerica<object>>>(JsonSerializer.Serialize(temp));
        _logger.LogDebug("ServicioMiembro-PaginaAPI resultado {ok} {code} {error}", respuesta!.Ok, respuesta!.HttpCode, respuesta.Error);
        return respuesta;
    }

    public async Task<RespuestaPayload<PaginaGenerica<object>>> PaginaDespliegueAPI(Consulta consulta, StringDictionary? parametros = null)
    {
        _logger.LogDebug("ServicioMiembro-PaginaDespliegueAPI-{consulta}", consulta);
        var temp = await this.PaginaDespliegue(consulta, parametros);
        RespuestaPayload<PaginaGenerica<object>> respuesta = JsonSerializer.Deserialize<RespuestaPayload<PaginaGenerica<object>>>(JsonSerializer.Serialize(temp));
        _logger.LogDebug("ServicioMiembro-PaginaDespliegueAPI resultado {ok} {code} {error}", respuesta!.Ok, respuesta!.HttpCode, respuesta.Error);
        return respuesta;
    }

    public async Task<RespuestaPayload<object>> UnicaPorIdAPI(object id, StringDictionary? parametros = null)
    {
        _logger.LogDebug("ServicioMiembro-UnicaPorIdAPI");
        var temp = await this.UnicaPorId((string)id, parametros);
        RespuestaPayload<object> respuesta = JsonSerializer.Deserialize<RespuestaPayload<object>>(JsonSerializer.Serialize(temp));
        _logger.LogDebug("ServicioMiembro-UnicaPorIdAPI resultado {ok} {code} {error}", respuesta!.Ok, respuesta!.HttpCode, respuesta.Error);
        return respuesta;
    }

    public async Task<RespuestaPayload<object>> UnicaPorIdDespliegueAPI(object id, StringDictionary? parametros = null)
    {
        _logger.LogDebug("ServicioMiembro-UnicaPorIdDespliegueAPI");
        var temp = await this.UnicaPorIdDespliegue((string)id, parametros);
        RespuestaPayload<object> respuesta = JsonSerializer.Deserialize<RespuestaPayload<object>>(JsonSerializer.Serialize(temp));
        _logger.LogDebug("ServicioMiembro-UnicaPorIdDespliegueAPI resultado {ok} {code} {error}", respuesta!.Ok, respuesta!.HttpCode, respuesta.Error);
        return respuesta;
    }

    #region Overrides para la personalización de la ENTIDAD => Tema
    public override async Task<ResultadoValidacion> ValidarInsertar(Tema data)
    {
        ResultadoValidacion resultado = new();


        if(!data.TemaId.Equals(Guid.Empty))
        {
            var existeTemaId = temario.Temas.FirstOrDefault(x => x.TemaId.Equals(data.TemaId));

            if(existeTemaId == null)
            {
                resultado.Valido = true;

            }
            else
            {
                resultado.Error = new ErrorProceso()
                {
                    Codigo = CodigosError.DISENOCURRICULAR_TEMA_TEMAID_NO_EXISTE,
                    Mensaje = "No existe el TemaId",
                    HttpCode = HttpCode.BadRequest
                };
                resultado.Valido = false;
            }
        }
        return resultado;
    }
    public override async Task<ResultadoValidacion> ValidarEliminacion(string id, Tema original)
    {
        ResultadoValidacion resultado = new();


        if (original.TemaId.Equals(Guid.Empty))
        {
            var existeTemaId = temario.Temas.FirstOrDefault(x => x.TemaId.Equals(original.TemaId));
            if (existeTemaId != null)
            {
                resultado.Error = new ErrorProceso()
                {
                    Codigo = CodigosError.DISENOCURRICULAR_TEMA_EXISTE_TEMAS_DEPENDIENTES,
                    Mensaje = "Existen temas dependientes",
                    HttpCode = HttpCode.Conflict
                };
                resultado.Valido = false;
            }
            else
            {
                resultado.Valido = true;
            }
        }
        return resultado;
    }
    public override async Task<ResultadoValidacion> ValidarActualizar(string id, Tema actualizacion, Tema original)
    {
        ResultadoValidacion resultado = new();
        if (!actualizacion.TemaId.Equals(Guid.Empty))
        {
            var existeTemaId = temario.Temas.FirstOrDefault(x => x.TemaId.Equals(actualizacion.TemaId));
            if (existeTemaId != null)
            {
                resultado.Valido = true;

            }
            else
            {
                resultado.Error = new ErrorProceso()
                {
                    Codigo = CodigosError.DISENOCURRICULAR_TEMA_TEMAID_NO_EXISTE,
                    Mensaje = "No existe el TemaId",
                    HttpCode = HttpCode.BadRequest
                };
                resultado.Valido = false;
            }
        }
        return resultado;
    }

    public override Tema ADTOFull(Tema actualizacion, Tema actual)
    {
        actual.Indice = actualizacion.Indice;
        actual.Clave = actualizacion.Clave;
        actual.Nombre = actualizacion.Nombre;
        actual.TemaId = actualizacion.TemaId;
        return actual;
    }

    public override Tema ADTOFull(Tema data)
    {
        Tema Tema = new Tema()
        {
            Id = Guid.NewGuid(),
            Indice = data.Indice,
            Clave = data.Clave,
            Nombre = data.Nombre,
            TemaId = data.TemaId
        };
        return Tema;
    }

    public override Tema ADTODespliegue(Tema data)
    {
        Tema Tema = new Tema()
        {
            Id = data.Id,
            Indice = data.Indice,
            Clave = data.Clave,
            Nombre = data.Nombre,
            TemaId = data.TemaId
        };
        return Tema;
    }

    public override async Task<RespuestaPayload<Tema>> Insertar(Tema data, StringDictionary? parametros = null)
    {
        var respuesta = new RespuestaPayload<Tema>();

        try
        {
            var resultadoValidacion = await ValidarInsertar(data);
            if (resultadoValidacion.Valido)
            {
                var entidad = ADTOFull(data);
                temario = _dbSetTemarios.FirstOrDefault(_ => _.Id == new Guid(parametros["n0Id"]));
                temario.Temas.Add(entidad);
                _dbSetTemarios.Update(temario);
                await _db.SaveChangesAsync();
                respuesta.Ok = true;
                respuesta.HttpCode = HttpCode.Ok;
                respuesta.Payload = ADTODespliegue(entidad);
            }
            else
            {
                respuesta.Error = resultadoValidacion.Error;
                respuesta.Error!.Codigo = CodigosError.DISENOCURRICULAR_DATOS_NO_VALIDOS;
                respuesta.HttpCode = resultadoValidacion.Error?.HttpCode ?? HttpCode.None;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "ServicioMiembro-Insertar {msg}", ex.Message);
            respuesta.Error = new ErrorProceso() { Codigo = CodigosError.DISENOCURRICULAR_ERROR_DESCONOCIDO, HttpCode = HttpCode.ServerError, Mensaje = ex.Message };
            respuesta.HttpCode = HttpCode.ServerError;
        }

        return respuesta;
    }

    public override async Task<Respuesta> Actualizar(string id, Tema data, StringDictionary? parametros = null)
    {
        var respuesta = new Respuesta();
        try
        {
            if (string.IsNullOrEmpty(id.ToString()) || data == null)
            {
                respuesta.Error = new ErrorProceso()
                {
                    Codigo = CodigosError.DISENOCURRICULAR_TEMA_ID_PAYLOAD_NO_INGRESADO,
                    Mensaje = "No ha sido proporcionado el Id ó Payload",
                    HttpCode = HttpCode.BadRequest
                };
                respuesta.HttpCode = HttpCode.BadRequest;
                return respuesta;
            }
            temario = _dbSetTemarios.FirstOrDefault(_ => _.Id == new Guid(parametros["n0Id"]));
            Tema actual = temario.Temas.FirstOrDefault(_ => _.Id == Guid.Parse(id));
            if (actual == null)
            {
                respuesta.Error = new ErrorProceso()
                {
                    Codigo = CodigosError.DISENOCURRICULAR_TEMA_NO_ENCONTRADA,
                    Mensaje = "No existe un Tema con el Id proporcionado",
                    HttpCode = HttpCode.NotFound
                };
                respuesta.HttpCode = HttpCode.NotFound;
                return respuesta;
            }


            var resultadoValidacion = await ValidarActualizar(id.ToString(), data, actual);
            if (resultadoValidacion.Valido)
            {
                var entidad = ADTOFull(data, actual);
                var index = temario.Temas.IndexOf(entidad);
                if (index == 0)
                {
                    temario.Temas[0] = entidad;
                    _dbSetTemarios.Update(temario);
                    await _db.SaveChangesAsync();
                    respuesta.Ok = true;
                    respuesta.HttpCode = HttpCode.Ok;
                }
                else
                {
                    respuesta.Error = new ErrorProceso()
                    {
                        Codigo = CodigosError.DISENOCURRICULAR_TEMA_ERROR_ACTUALIZAR,
                        Mensaje = "No ha sido posible actualizar el Tema",
                        HttpCode = HttpCode.BadRequest
                    };
                    respuesta.HttpCode = HttpCode.BadRequest;
                    return respuesta;
                }
            }
            else
            {
                respuesta.Error = resultadoValidacion.Error;
                respuesta.Error!.Codigo = CodigosError.DISENOCURRICULAR_DATOS_NO_VALIDOS;
                respuesta.HttpCode = resultadoValidacion.Error?.HttpCode ?? HttpCode.None;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "ServicioMiembro-Actualizar {msg}", ex.Message);
            respuesta.Error = new ErrorProceso() { Codigo = CodigosError.DISENOCURRICULAR_ERROR_DESCONOCIDO, HttpCode = HttpCode.ServerError, Mensaje = ex.Message };
            respuesta.HttpCode = HttpCode.ServerError;
        }
        return respuesta;
    }

    public override async Task<RespuestaPayload<Tema>> UnicaPorId(string id, StringDictionary? parametros = null)
    {
        var respuesta = new RespuestaPayload<Tema>();
        try
        {
            temario = _dbSetTemarios.FirstOrDefault(_ => _.Id == new Guid(parametros["n0Id"]));
            Tema actual = temario.Temas.FirstOrDefault(_ => _.Id == Guid.Parse(id));
            if (actual == null)
            {
                respuesta.Error = new ErrorProceso()
                {
                    Codigo = CodigosError.DISENOCURRICULAR_TEMA_NO_ENCONTRADA,
                    Mensaje = "No existe un Tema con el Id proporcionado",
                    HttpCode = HttpCode.NotFound
                };
                respuesta.HttpCode = HttpCode.NotFound;
                return respuesta;
            }
            respuesta.Ok = true;
            respuesta.HttpCode = HttpCode.Ok;
            respuesta.Payload = actual;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "ServicioMiembro-UnicaPorId {msg}", ex.Message);
            respuesta.Error = new ErrorProceso() { Codigo = CodigosError.DISENOCURRICULAR_ERROR_DESCONOCIDO, HttpCode = HttpCode.ServerError, Mensaje = ex.Message };
            respuesta.HttpCode = HttpCode.ServerError;
        }
        return respuesta;
    }

    public override async Task<Respuesta> Eliminar(string id, StringDictionary? parametros = null)
    {
        var respuesta = new Respuesta();
        try
        {

            if (string.IsNullOrEmpty(id))
            {
                respuesta.Error = new ErrorProceso()
                {
                    Codigo = CodigosError.DISENOCURRICULAR_TEMA_ID_NO_INGRESADO,
                    Mensaje = "No ha sido proporcionado el Id",
                    HttpCode = HttpCode.BadRequest
                };
                respuesta.HttpCode = HttpCode.BadRequest;
                return respuesta;
            }
            temario = _dbSetTemarios.FirstOrDefault(_ => _.Id == new Guid(parametros["n0Id"]));
            Tema actual = temario.Temas.FirstOrDefault(_ => _.Id == Guid.Parse(id));
            if (actual == null)
            {
                respuesta.Error = new ErrorProceso()
                {
                    Codigo = CodigosError.DISENOCURRICULAR_TEMA_NO_ENCONTRADA,
                    Mensaje = "No existe un Tema con el Id proporcionado",
                    HttpCode = HttpCode.NotFound
                };
                respuesta.HttpCode = HttpCode.NotFound;
                return respuesta;
            }

            var resultadoValidacion = await ValidarEliminacion(id, actual);
            if (resultadoValidacion.Valido)
            {
                temario.Temas.Remove(actual);
                _dbSetTemarios.Update(temario);
                await _db.SaveChangesAsync();
                respuesta.Ok = true;
                respuesta.HttpCode = HttpCode.Ok;
            }
            else
            {
                respuesta.Error = new ErrorProceso()
                {
                    Codigo = CodigosError.DISENOCURRICULAR_TEMA_ERROR_ELIMINAR,
                    Mensaje = "No ha sido posible ELIMINAR el Tema",
                    HttpCode = HttpCode.BadRequest
                };
                respuesta.HttpCode = HttpCode.BadRequest;
                return respuesta;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "ServicioMiembro-Eliminar {msg}", ex.Message);
            respuesta.Error = new ErrorProceso() { Codigo = CodigosError.DISENOCURRICULAR_ERROR_DESCONOCIDO, HttpCode = HttpCode.ServerError, Mensaje = ex.Message };
            respuesta.HttpCode = HttpCode.ServerError;
        }
        return respuesta;
    }

    public override async Task<PaginaGenerica<Tema>> ObtienePaginaElementos(Consulta consulta, StringDictionary? parametros = null)
    {
        _logger.LogDebug("ServicioMiembro - ObtienePaginaElementos - {consulta}", consulta);
        Entidad entidad = reflectorEntidades.ObtieneEntidad(typeof(Tema));
        var Elementos = Enumerable.Empty<Tema>().AsQueryable();
        temario = _dbSetTemarios.FirstOrDefault(_ => _.Id == new Guid(parametros["n0Id"]));
        if (temario != null)
        {
            if (consulta.Filtros.Count > 0)
            {
                var predicateBody = interpreteConsulta.CrearConsultaExpresion<Tema>(consulta, entidad);

                if (predicateBody != null)
                {
                    var RConsulta = temario.Temas.AsQueryable().Provider.CreateQuery<Tema>(predicateBody.getWhereExpression(temario.Temas.AsQueryable().Expression));

                    Elementos = RConsulta.OrdenarPor(consulta.Paginado.ColumnaOrdenamiento ?? "Id", consulta.Paginado.Ordenamiento ?? Ordenamiento.asc);
                }
            }
            else
            {
                var RConsulta = temario.Temas.AsQueryable();
                Elementos = RConsulta.OrdenarPor(consulta.Paginado.ColumnaOrdenamiento ?? "Id", consulta.Paginado.Ordenamiento ?? Ordenamiento.asc);

            }
        }
        return Elementos.Paginado(consulta);
    }
    #endregion
}
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
#pragma warning restore CS8603 // Possible null reference return