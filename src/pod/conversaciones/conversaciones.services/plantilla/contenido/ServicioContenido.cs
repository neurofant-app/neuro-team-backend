﻿#pragma warning disable CS8603 // Possible null reference return.
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
using apigenerica.model.interpretes;
using apigenerica.model.modelos;
using apigenerica.model.reflectores;
using apigenerica.model.servicios;
using comunes.primitivas;
using comunes.primitivas.configuracion.mongo;
using conversaciones.model;
using conversaciones.services.dbcontext;
using extensibilidad.metadatos;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using System.Collections.Specialized;
using System.Text.Json;
using Plantilla = conversaciones.model.Plantilla;
namespace conversaciones.services.plantilla.contenido;
[ServicioEntidadAPI(entidad:typeof(Contenido))]
public class ServicioContenido : ServicioEntidadGenericaBase<Contenido,Contenido,Contenido,Contenido,string>,
    IServicioEntidadAPI,IServicioContenido
{
    private readonly ILogger<ServicioContenido> _logger;
    private readonly IReflectorEntidadesAPI _reflector;
    private Plantilla? plantilla;
    private DbSet<Plantilla> _dbSetPlantilla;
    public ServicioContenido(ILogger<ServicioContenido> logger,
        IServicionConfiguracionMongo configuracionMongo,
        IReflectorEntidadesAPI reflector, IDistributedCache cache) : base(null, null, logger, reflector, cache)
    {
        _logger = logger;
        _reflector = reflector;
        interpreteConsulta = new InterpreteConsultaExpresiones();

        var configuracionEntidad = configuracionMongo.ConexionEntidad(MongoDbContextConversaciones.NOMBRE_COLECCION_PLANTILLA);
        if (configuracionEntidad == null)
        {
            string err = $"No existe configuracion de mongo para '{MongoDbContextConversaciones.NOMBRE_COLECCION_PLANTILLA}'";
            _logger.LogError(err);
            throw new Exception(err);
        }

        try
        {
            _logger.LogDebug($"Mongo DB {configuracionEntidad.Esquema} coleccioón {configuracionEntidad.Esquema} utilizando conexión default {string.IsNullOrEmpty(configuracionEntidad.Conexion)}");
            var cadenaConexion = string.IsNullOrEmpty(configuracionEntidad.Conexion) && string.IsNullOrEmpty(configuracionMongo.ConexionDefault())
                ? configuracionMongo.ConexionDefault()
                : string.IsNullOrEmpty(configuracionEntidad.Conexion)
                    ? configuracionMongo.ConexionDefault()
                    : configuracionEntidad.Conexion;
            var client = new MongoClient(cadenaConexion);

            _db = MongoDbContextConversaciones.Create(client.GetDatabase(configuracionEntidad.Esquema));
            _dbSetPlantilla = ((MongoDbContextConversaciones)_db).Plantilla;

        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error al inicializar mongo para '{MongoDbContextConversaciones.NOMBRE_COLECCION_PLANTILLA}'");
            throw;
        }
    }

    private MongoDbContextConversaciones DB { get { return (MongoDbContextConversaciones)_db; } }

    public bool RequiereAutenticacion => true;

    public async Task<Respuesta> ActualizarAPI(object id, JsonElement data, StringDictionary? parametros = null)
    {
        _logger.LogDebug("ServicioContenido-ActualizarAPI-{data}", data);
        var update = data.Deserialize<Contenido>(JsonAPIDefaults());
        Respuesta respuesta = await this.Actualizar((string)id, update, parametros);
        _logger.LogDebug("ServicioContenido-ActualizarAPI resultado {ok} {code} {error}", respuesta!.Ok, respuesta!.HttpCode, respuesta.Error);
        return respuesta;
    }

    public async Task<Respuesta> EliminarAPI(object id, StringDictionary? parametros = null, bool forzarEliminacion = false)
    {
        _logger.LogDebug("ServicioContenido-EliminarAPI");
        Respuesta respuesta = await this.Eliminar((string)id, parametros, forzarEliminacion);
        _logger.LogDebug("ServicioContenido-EliminarAPI resultado {ok} {code} {error}", respuesta!.Ok, respuesta!.HttpCode, respuesta.Error);
        return respuesta;
    }

    public Entidad EntidadDespliegueAPI()
    {
        _logger.LogDebug("ServicioContenido-EntidadDespliegueAPI");
        return this.EntidadDespliegue();
    }

    public Entidad EntidadInsertAPI()
    {
        _logger.LogDebug("ServicioContenido-EntidadInsertAPI");
        return this.EntidadInsert();
    }

    public Entidad EntidadRepoAPI()
    {
        _logger.LogDebug("ServicioContenido-EntidadRepoAPI");
        return this.EntidadRepo();
    }

    public Entidad EntidadUpdateAPI()
    {
        _logger.LogDebug("ServicioContenido-EntidadUpdateAPI");
        return this.EntidadUpdate();
    }

    public void EstableceContextoUsuarioAPI(ContextoUsuario contexto)
    {
        _logger.LogDebug("ServicioContenido-EstableceContextoUsuarioAPI");
        this.EstableceContextoUsuario(contexto);
    }

    public async Task<RespuestaPayload<object>> InsertarAPI(JsonElement data, StringDictionary? parametros = null)
    {
        _logger.LogDebug("ServicioContenido-InsertarAPI-{data}", data);
        var add = data.Deserialize<Contenido>(JsonAPIDefaults());
        var temp = await this.Insertar(add, parametros);
        RespuestaPayload<object> respuesta = JsonSerializer.Deserialize<RespuestaPayload<object>>(JsonSerializer.Serialize(temp));
        _logger.LogDebug("ServicioContenido-InsertarAPI resultado {ok} {code} {error}", respuesta!.Ok, respuesta!.HttpCode, respuesta.Error);
        return respuesta;
    }
    public ContextoUsuario? ObtieneContextoUsuarioAPI()
    {
        _logger.LogDebug("ServicioContenido-ObtieneContextoUsuarioAPI");
        return this._contextoUsuario;
    }
    public async Task<RespuestaPayload<PaginaGenerica<object>>> PaginaAPI(Consulta consulta, StringDictionary? parametros = null)
    {
        _logger.LogDebug("ServicioContenido-PaginaAPI-{consulta}", consulta);
        var temp = await this.Pagina(consulta, parametros);
        RespuestaPayload<PaginaGenerica<object>> respuesta = JsonSerializer.Deserialize<RespuestaPayload<PaginaGenerica<object>>>(JsonSerializer.Serialize(temp));
        _logger.LogDebug("ServicioContenido-PaginaAPI resultado {ok} {code} {error}", respuesta!.Ok, respuesta!.HttpCode, respuesta.Error);
        return respuesta;
    }
    public async Task<RespuestaPayload<PaginaGenerica<object>>> PaginaDespliegueAPI(Consulta consulta, StringDictionary? parametros = null)
    {
        _logger.LogDebug("ServicioContenido-PaginaDespliegueAPI-{consulta}", consulta);
        var temp = await this.PaginaDespliegue(consulta, parametros);
        RespuestaPayload<PaginaGenerica<object>> respuesta = JsonSerializer.Deserialize<RespuestaPayload<PaginaGenerica<object>>>(JsonSerializer.Serialize(temp));
        _logger.LogDebug("ServicioContenido-PaginaDespliegueAPI resultado {ok} {code} {error}", respuesta!.Ok, respuesta!.HttpCode, respuesta.Error);
        return respuesta;
    }
    public async Task<RespuestaPayload<object>> UnicaPorIdAPI(object id, StringDictionary? parametros = null)
    {
        _logger.LogDebug("ServicioContenido-UnicaPorIdAPI");
        var temp = await this.UnicaPorId((string)id, parametros);
        RespuestaPayload<object> respuesta = JsonSerializer.Deserialize<RespuestaPayload<object>>(JsonSerializer.Serialize(temp));
        _logger.LogDebug("ServicioContenido-UnicaPorIdAPI resultado {ok} {code} {error}", respuesta!.Ok, respuesta!.HttpCode, respuesta.Error);
        return respuesta;
    }
    public async Task<RespuestaPayload<object>> UnicaPorIdDespliegueAPI(object id, StringDictionary? parametros = null)
    {
        _logger.LogDebug("ServicioContenido-UnicaPorIdDespliegueAPI");
        var temp = await this.UnicaPorIdDespliegue((string)id, parametros);
        RespuestaPayload<object> respuesta = JsonSerializer.Deserialize<RespuestaPayload<object>>(JsonSerializer.Serialize(temp));
        _logger.LogDebug("ServicioContenido-UnicaPorIdDespliegueAPI resultado {ok} {code} {error}", respuesta!.Ok, respuesta!.HttpCode, respuesta.Error);
        return respuesta;
    }

    #region Overrides para la personalizacion de le ENTIDAD => Contenido
    public override async Task<ResultadoValidacion> ValidarInsertar(Contenido data)
    {
        ResultadoValidacion resultado = new();
        resultado.Valido = true;
        return resultado;
    }
    public override async Task<ResultadoValidacion> ValidarEliminacion(string id, Contenido original, bool forzarEliminacion = false)
    {
        ResultadoValidacion resultado = new();
        resultado.Valido = true;
        return resultado;
    }
    public override async Task<ResultadoValidacion> ValidarActualizar(string id, Contenido actualizacion, Contenido original)
    {
        ResultadoValidacion resultado = new();
        resultado.Valido = true;
        return resultado;
    }

    public override Contenido ADTOFull(Contenido actualizacion, Contenido actual)
    {
        actual.Id = actualizacion.Id;
        actual.Canal = actualizacion.Canal;
        actual.Cuerpo = actualizacion.Cuerpo;
        actual.Encabezado = actualizacion.Encabezado;
        actual.Idioma = actualizacion.Idioma;
        return actual;
    }

    public override Contenido ADTOFull(Contenido data)
    {
        Contenido contenido = new Contenido()
        {
            Id = data.Id,
            Canal = data.Canal,
            Cuerpo = data.Cuerpo,
            Encabezado = data.Encabezado,
            Idioma = data.Idioma,
        };
        return contenido;
    }

    public override Contenido ADTODespliegue(Contenido data)
    {
        Contenido contenido = new Contenido()
        {
            Id = data.Id,
            Canal = data.Canal,
            Cuerpo = data.Cuerpo,
            Encabezado = data.Encabezado,
            Idioma = data.Idioma,
        };
        return contenido;
    }

    public virtual async Task<RespuestaPayload<Contenido>> Insertar(Contenido data, StringDictionary? parametros = null)
    {
        var respuesta = new RespuestaPayload<Contenido>();

        try
        {
            var resultadoValidacion = await ValidarInsertar(data);
            if (resultadoValidacion.Valido)
            {
                var entidad = ADTOFull(data);
                plantilla = _dbSetPlantilla.FirstOrDefault(_ => _.Id == parametros["n0Id"]);
                plantilla.Contenidos.Add(entidad);
                _dbSetPlantilla.Update(plantilla);
                await _db.SaveChangesAsync();
                respuesta.Ok = true;
                respuesta.HttpCode = HttpCode.Ok;
                respuesta.Payload = ADTODespliegue(entidad);
            }
            else
            {
                respuesta.Error = resultadoValidacion.Error;
                respuesta.Error!.Codigo = CodigosError.CONVERSACIONES_DATOS_NO_VALIDOS;
                respuesta.HttpCode = resultadoValidacion.Error?.HttpCode ?? HttpCode.None;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "ServicioContenido-Insertar {msg}", ex.Message);
            respuesta.Error = new ErrorProceso() { Codigo = CodigosError.CONVERSACIONES_ERROR_DESCONOCIDO, HttpCode = HttpCode.ServerError, Mensaje = ex.Message };
            respuesta.HttpCode = HttpCode.ServerError;
        }

        return respuesta;
    }

    public override async Task<Respuesta> Actualizar(string id, Contenido data, StringDictionary? parametros = null)
    {
        var respuesta = new Respuesta();
        try
        {
            if (string.IsNullOrEmpty(id.ToString()) || data == null)
            {
                respuesta.Error = new ErrorProceso()
                {
                    Codigo = CodigosError.CONVERSACIONES_CONTENIDO_ID_PAYLOAD_NO_INGRESADO,
                    Mensaje = "No ha sido proporcionado el Id ó Payload",
                    HttpCode = HttpCode.BadRequest
                };
                respuesta.HttpCode = HttpCode.BadRequest;
                return respuesta;
            }
            plantilla = _dbSetPlantilla.FirstOrDefault(_ => _.Id == parametros["n0Id"]);
            Contenido actual = plantilla.Contenidos.FirstOrDefault(_ => _.Id == data.Id);
            if (actual == null)
            {
                respuesta.Error = new ErrorProceso()
                {
                    Codigo = CodigosError.CONVERSACIONES_CONTENIDO_NO_ENCONTRADA,
                    Mensaje = "No existe un Contenido con el Id proporcionado",
                    HttpCode = HttpCode.NotFound
                };
                respuesta.HttpCode = HttpCode.NotFound;
                return respuesta;
            }


            var resultadoValidacion = await ValidarActualizar(id, data, actual);
            if (resultadoValidacion.Valido)
            {
                var entidad = ADTOFull(data, actual);
                var index = plantilla.Contenidos.IndexOf(entidad);
                if (index == 0)
                {
                    plantilla.Contenidos[0] = entidad;
                    _dbSetPlantilla.Update(plantilla);
                    await _db.SaveChangesAsync();
                    respuesta.Ok = true;
                    respuesta.HttpCode = HttpCode.Ok;
                }
                else
                {
                    respuesta.Error = new ErrorProceso()
                    {
                        Codigo = CodigosError.CONVERSACIONES_CONTENIDO_ERROR_ACTUALIZAR,
                        Mensaje = "No ha sido posible actualizar el Contenido",
                        HttpCode = HttpCode.BadRequest
                    };
                    respuesta.HttpCode = HttpCode.BadRequest;
                    return respuesta;
                }

            }
            else
            {
                respuesta.Error = resultadoValidacion.Error;
                respuesta.Error!.Codigo = CodigosError.CONVERSACIONES_DATOS_NO_VALIDOS;
                respuesta.HttpCode = resultadoValidacion.Error?.HttpCode ?? HttpCode.None;
            }

        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "ServicioContenido-UnicoPorId {msg}", ex.Message);
            respuesta.Error = new ErrorProceso() { Codigo = CodigosError.CONVERSACIONES_ERROR_DESCONOCIDO, HttpCode = HttpCode.ServerError, Mensaje = ex.Message };
            respuesta.HttpCode = HttpCode.ServerError;
        }
        return respuesta;
    }

    public override async Task<RespuestaPayload<Contenido>> UnicaPorId(string id, StringDictionary? parametros = null)
    {
        var respuesta = new RespuestaPayload<Contenido>();
        try
        {
            plantilla = _dbSetPlantilla.FirstOrDefault(_ => _.Id == parametros["n0Id"]);
            Contenido actual = plantilla.Contenidos.FirstOrDefault(_ => _.Id == id);
            if (actual == null)
            {
                respuesta.Error = new ErrorProceso()
                {
                    Codigo = CodigosError.CONVERSACIONES_CONTENIDO_NO_ENCONTRADA,
                    Mensaje = "No existe un CONVERSIONES con el Id proporcionado",
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
            _logger.LogError(ex, "ServicioContenido-UnicaPorId {msg}", ex.Message);
            respuesta.Error = new ErrorProceso() { Codigo = CodigosError.CONVERSACIONES_ERROR_DESCONOCIDO, HttpCode = HttpCode.ServerError, Mensaje = ex.Message };
            respuesta.HttpCode = HttpCode.ServerError;
        }
        return respuesta;
    }

    public override async Task<Respuesta> Eliminar(string id, StringDictionary? parametros = null, bool forzarEliminacion = false)
    {
        var respuesta = new Respuesta();
        try
        {
            if (string.IsNullOrEmpty(id))
            {
                respuesta.Error = new ErrorProceso()
                {
                    Codigo = CodigosError.CONVERSACIONES_CONTENIDO_ID_NO_INGRESADO,
                    Mensaje = "No ha sido proporcionado el Id",
                    HttpCode = HttpCode.BadRequest
                };
                respuesta.HttpCode = HttpCode.BadRequest;
                return respuesta;
            }
            plantilla = _dbSetPlantilla.FirstOrDefault(_ => _.Id == parametros["n0Id"]);
            Contenido actual = plantilla.Contenidos.FirstOrDefault(_ => _.Id == id);
            if (actual == null)
            {
                respuesta.Error = new ErrorProceso()
                {
                    Codigo = CodigosError.CONVERSACIONES_CONTENIDO_NO_ENCONTRADA,
                    Mensaje = "No existe un Contenido con el Id proporcionado",
                    HttpCode = HttpCode.NotFound
                };
                respuesta.HttpCode = HttpCode.NotFound;
                return respuesta;
            }

            var resultadoValidacion = await ValidarEliminacion(id, actual);
            if (resultadoValidacion.Valido)
            {
                plantilla.Contenidos.Remove(actual);
                _dbSetPlantilla.Update(plantilla);
                await _db.SaveChangesAsync();
                respuesta.Ok = true;
                respuesta.HttpCode = HttpCode.Ok;
            }
            else
            {
                respuesta.Error = new ErrorProceso()
                {
                    Codigo = CodigosError.CONVERSACIONES_CONTENIDO_ERROR_ELIMINAR,
                    Mensaje = "No ha sido posible ELIMINAR el Contenido",
                    HttpCode = HttpCode.BadRequest
                };
                respuesta.HttpCode = HttpCode.BadRequest;
                return respuesta;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "ServicioContenido-Eliminar {msg}", ex.Message);
            respuesta.Error = new ErrorProceso() { Codigo = CodigosError.CONVERSACIONES_ERROR_DESCONOCIDO, HttpCode = HttpCode.ServerError, Mensaje = ex.Message };
            respuesta.HttpCode = HttpCode.ServerError;
        }
        return respuesta;
    }

    public override async Task<PaginaGenerica<Contenido>> ObtienePaginaElementos(Consulta consulta, StringDictionary? parametros = null)
    {
        _logger.LogDebug("ServicioContenido - ObtienePaginaElementos - {consulta}", consulta);

        Entidad entidad = reflectorEntidades.ObtieneEntidad(typeof(Contenido));
        var Elementos = Enumerable.Empty<Contenido>().AsQueryable();
        plantilla = _dbSetPlantilla.FirstOrDefault(_ => _.Id == parametros["n0Id"]);
        if (plantilla != null)
        {
            if (consulta.Filtros.Count > 0)
            {
                var predicateBody = interpreteConsulta.CrearConsultaExpresion<Contenido>(consulta, entidad);

                if (predicateBody != null)
                {
                    var RConsulta = plantilla.Contenidos.AsQueryable().Provider.CreateQuery<Contenido>(predicateBody.getWhereExpression(plantilla.Contenidos.AsQueryable().Expression));

                    Elementos = RConsulta.OrdenarPor(consulta.Paginado.ColumnaOrdenamiento ?? "Id", consulta.Paginado.Ordenamiento ?? Ordenamiento.asc);
                }
            }
            else
            {
                var RConsulta = plantilla.Contenidos.AsQueryable();
                Elementos = RConsulta.OrdenarPor(consulta.Paginado.ColumnaOrdenamiento ?? "Id", consulta.Paginado.Ordenamiento ?? Ordenamiento.asc);

            }
        }
        return Elementos.Paginado(consulta);
    }

    #endregion

}
#pragma warning disable CS8603 // Possible null reference return.
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.