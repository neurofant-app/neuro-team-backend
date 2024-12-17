#pragma warning disable CS8603 // Possible null reference return.
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
using apigenerica.model.modelos;
using apigenerica.model.reflectores;
using apigenerica.model.servicios;
using comunes.primitivas;
using comunes.primitivas.configuracion.mongo;
using evaluacion.model.evaluacion;
using evaluacion.model.evaluacion.temas;
using evaluacion.model.reactivos;
using evaluacion.services.dbcontext;
using extensibilidad.metadatos;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using System.Collections.Specialized;
using System.Text.Json;

namespace evaluacion.services.evaluacion;

[ServicioEntidadAPI(typeof(ReactivoTema))]
public class ServicioReactivo : ServicioEntidadGenericaBase<ReactivoTema, ReactivoCrear, ReactivoActualizar, ReactivoTema, Guid>,
    IServicioEntidadAPI, IServicioReactivo
{
    private readonly ILogger<ServicioReactivo> _logger;
    private readonly IReflectorEntidadesAPI _reflector;
    private readonly IDistributedCache _cache;
    private DbSet<Evaluacion> _dbSetEvaluacion;
    private Evaluacion _evaluacion;

    public ServicioReactivo(ILogger<ServicioReactivo> logger, IServicionConfiguracionMongo configuracionMongo,IReflectorEntidadesAPI reflector, IDistributedCache cache) 
        : base(null,null,logger,reflector,cache)
    {
        _logger = logger;
        _reflector = reflector;
        _cache = cache;

        var configuracionEntidad = configuracionMongo.ConexionEntidad(MongoDbContextEvaluacion.NOMBRE_COLECCION_EVALUACION);

        if (configuracionEntidad == null)
        {
            string err = $"No existe configuracion de mongo para '{MongoDbContextEvaluacion.NOMBRE_COLECCION_EVALUACION}'";
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


            _db = MongoDbContextEvaluacion.Create(client.GetDatabase(configuracionEntidad.Esquema));
            _dbSetEvaluacion = ((MongoDbContextEvaluacion)_db).Evaluaciones;

        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error al inicialzar mongo para '{MongoDbContextEvaluacion.NOMBRE_COLECCION_EVALUACION}'");
            throw;
        }
    }

    public bool RequiereAutenticacion => true;

    public async Task<Respuesta> ActualizarAPI(object id, JsonElement data, StringDictionary? parametros = null)
    {
        _logger.LogDebug("ServicioReactivo-ActualizarAPI-{data}", data);
        var update = data.Deserialize<ReactivoActualizar>(JsonAPIDefaults());
        Respuesta respuesta = await this.Actualizar(Guid.Parse((string)id), update, parametros);
        _logger.LogDebug("ServicioReactivo-ActualizarAPI resultado {ok} {code} {error}", respuesta!.Ok, respuesta!.HttpCode, respuesta.Error);
        return respuesta;
    }

    public async Task<Respuesta> EliminarAPI(object id, StringDictionary? parametros = null)
    {
        _logger.LogDebug("ServicioReactivo-EliminarAPI");
        Respuesta respuesta = await this.Eliminar(Guid.Parse((string)id), parametros);
        _logger.LogDebug("ServicioReactivo-EliminarAPI resultado {ok} {code} {error}", respuesta!.Ok, respuesta!.HttpCode, respuesta.Error);
        return respuesta;
    }
    public async Task<RespuestaPayload<object>> InsertarAPI(JsonElement data, StringDictionary? parametros = null)
    {
        _logger.LogDebug("ServicioReactivo-InsertarAPI-{data}", data);
        var add = data.Deserialize<ReactivoCrear>(JsonAPIDefaults());
        var temp = await this.Insertar(add, parametros);
        RespuestaPayload<object> respuesta = JsonSerializer.Deserialize<RespuestaPayload<object>>(JsonSerializer.Serialize(temp));
        _logger.LogDebug("ServicioReactivo-InsertarAPI resultado {ok} {code} {error}", respuesta!.Ok, respuesta!.HttpCode, respuesta.Error);
        return respuesta;
    }


    public async Task<RespuestaPayload<PaginaGenerica<object>>> PaginaAPI(Consulta consulta, StringDictionary? parametros = null)
    {
        _logger.LogDebug("ServicioReactivo-PaginaAPI");
        var respuesta = new RespuestaPayload<PaginaGenerica<object>>();
        {
            respuesta.Error = new ErrorProceso()
            {
                Codigo = CodigosError.EVALUACION_REACTIVO_ACCION_NO_IMPLEMENTADA,
                Mensaje = "Acción no implementada",
                HttpCode = HttpCode.NotImplemented
            };
        };
        _logger.LogDebug("ServicioReactivo-PaginaAPI resultado {ok} {code} {error}", respuesta!.Ok, respuesta!.HttpCode, respuesta.Error);
        return respuesta;
    }

    public async Task<RespuestaPayload<PaginaGenerica<object>>> PaginaDespliegueAPI(Consulta consulta, StringDictionary? parametros = null)
    {
        _logger.LogDebug("ServicioReactivo-PaginaDespliegueAPI");
        var respuesta = new RespuestaPayload<PaginaGenerica<object>>();
        {
            respuesta.Error = new ErrorProceso()
            {
                Codigo = CodigosError.EVALUACION_REACTIVO_ACCION_NO_IMPLEMENTADA,
                Mensaje = "Acción no implementada",
                HttpCode = HttpCode.NotImplemented
            };
        };
        _logger.LogDebug("ServicioReactivo-PaginaDespliegueAPI resultado {ok} {code} {error}", respuesta!.Ok, respuesta!.HttpCode, respuesta.Error);
        return respuesta;
    }

    public async Task<RespuestaPayload<object>> UnicaPorIdAPI(object id, StringDictionary? parametros = null)
    {
        _logger.LogDebug("ServicioReactivo-UnicaPorIdAPI");
        var respuesta = new RespuestaPayload<object>()
        {
            Error = new ErrorProceso()
            {
                Codigo = CodigosError.EVALUACION_REACTIVO_ACCION_NO_IMPLEMENTADA,
                Mensaje = "Acción no implementada",
                HttpCode = HttpCode.NotImplemented
            },
        };
        _logger.LogDebug("ServicioReactivo-UnicaPorIdAPI resultado {ok} {code} {error}", respuesta!.Ok, respuesta!.HttpCode, respuesta.Error);
        return respuesta;
    }

    public async Task<RespuestaPayload<object>> UnicaPorIdDespliegueAPI(object id, StringDictionary? parametros = null)
    {
        _logger.LogDebug("ServicioReactivo-UnicaPorIdDespliegueAPI");
        var respuesta = new RespuestaPayload<object>()
        {
            Error = new ErrorProceso()
            {
                Codigo = CodigosError.EVALUACION_REACTIVO_ACCION_NO_IMPLEMENTADA,
                Mensaje = "Acción no implementada",
                HttpCode = HttpCode.NotImplemented
            },
        };
        _logger.LogDebug("ServicioReactivo-UnicaPorIdDespliegueAPI resultado {ok} {code} {error}", respuesta!.Ok, respuesta!.HttpCode, respuesta.Error);
        return respuesta;
    }

    public Entidad EntidadRepoAPI()
    {
        _logger.LogDebug("ServicioReactivo-EntidadRepoAPI");
        return this.EntidadRepo();
    }

    public Entidad EntidadInsertAPI()
    {
        _logger.LogDebug("ServicioReactivo-EntidadInsertAPI");
        return this.EntidadInsert();
    }

    public Entidad EntidadUpdateAPI()
    {
        _logger.LogDebug("ServicioReactivo-EntidadUpdateAPI");
        return this.EntidadUpdate();
    }

    public Entidad EntidadDespliegueAPI()
    {
        _logger.LogDebug("ServicioReactivo-DespliegueAPI");
        return this.EntidadDespliegue();
    }

    public void EstableceContextoUsuarioAPI(ContextoUsuario contexto)
    {
        _logger.LogDebug("ServicioReactivo-ContextoUsuarioAPI");
        this.EstableceContextoUsuario(contexto);
    }

    public ContextoUsuario? ObtieneContextoUsuarioAPI()
    {
        _logger.LogDebug("ServicioReactivo-ObtieneContextoUsuarioAPI");
        return this._contextoUsuario;
    }

    #region Overrides para la entidad ReactivoTema
    public async Task<ResultadoValidacion> ValidarActualizar(Guid id, ReactivoActualizar actualizacion, ReactivoTema original)
    {
        return new ResultadoValidacion() { Valido = true };
    }

    public async Task<ResultadoValidacion> ValidarEliminacion(Guid id, ReactivoTema original)
    {
        return new ResultadoValidacion() { Valido = true };
    }


    public override ReactivoTema ADTOFull(ReactivoCrear data)
    {
        return new ReactivoTema() { Id = data.ReactivoId, Dificultad = data.Dificultad, Puntaje = data.Puntaje, Obligatorio = data.Obligatorio };
    }

    public async Task<ResultadoValidacion> ValidarInsertar(ReactivoCrear data, StringDictionary? parametros = null)
    {
        var resultado = new ResultadoValidacion();

        _evaluacion = _dbSetEvaluacion.FirstOrDefault(_ => _.Id == Guid.Parse(parametros["n0Id"]) 
                                                         && _.DominioId == Guid.Parse(this._contextoUsuario.DominioId) 
                                                         && _.OUId == Guid.Parse(this._contextoUsuario.UOrgId));

        if(_evaluacion == null && _evaluacion.Estado != model.EstadoEvaluacion.Diseno)
        {
            resultado.Valido = false;
            resultado.Error = new ErrorProceso()
            {
                Codigo = CodigosError.EVALUACION_REACTIVO_ERROR_INSERTAR,
                Mensaje = "Verifique que exista la evaluación, dominio y UnidadOrganizacional para realizar la acción",
                HttpCode = HttpCode.NotFound
            };
            return resultado;
        }
        resultado.Valido = true;
        return resultado;
    }


    public override async Task<RespuestaPayload<ReactivoTema>> Insertar(ReactivoCrear data, StringDictionary? parametros = null)
    {
        var respuesta = new RespuestaPayload<ReactivoTema>();

        try
        {
            var resultadoValidacion = await ValidarInsertar(data, parametros);
            if (resultadoValidacion.Valido)
            {
                var temaEvaluacion = _evaluacion.Temas.FirstOrDefault(_ => _.TemaId == data.TemaId && _.TemarioId == data.TemarioId);
                var entidad = ADTOFull(data);

                if (temaEvaluacion == null)
                {
                    var temaEvaluacionCrea = new TemaEvaluacion()
                    {
                        TemarioId = data.TemarioId,
                        TemaId = data.TemaId,
                    };

                    temaEvaluacionCrea.Reactivos.Add(entidad);

                    _evaluacion.Temas.Add(temaEvaluacionCrea);
                    _evaluacion.TotalReactivos++;
                    

                    _dbSetEvaluacion.Update(_evaluacion);
                    await _db.SaveChangesAsync();

                    respuesta.Ok = true;
                    respuesta.HttpCode = HttpCode.Ok;
                    respuesta.Payload = ADTODespliegue(entidad);

                }

                var reactivo = temaEvaluacion.Reactivos.FirstOrDefault(_ => _.Id == entidad.Id);

                if(reactivo != null)
                {
                    respuesta.Error = new()
                    {
                        Codigo = CodigosError.EVALUACION_REACTIVO_EXISTENTE_ERROR,
                        Mensaje = "Reactivo existente, inserte un reactivo distinto",
                        HttpCode = HttpCode.Conflict
                    };
                    return respuesta;
                }
                if (entidad.Puntaje < 0) entidad.Puntaje = 0;
                temaEvaluacion.Reactivos.Add(entidad);
                _evaluacion.TotalReactivos++;
                _dbSetEvaluacion.Update(_evaluacion);
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
            _logger.LogError(ex, "ServicioReactivo-Insertar {msg}", ex.Message);
            respuesta.Error = new ErrorProceso() { Codigo = CodigosError.EVALUACION_ERROR_DESCONOCIDO, HttpCode = HttpCode.ServerError, Mensaje = ex.Message };
            respuesta.HttpCode = HttpCode.ServerError;
        }

        return respuesta;
    }


    public async Task<Respuesta> Eliminar(Guid id, StringDictionary? parametros = null)
    {
        var respuesta = new Respuesta();
        try
        {
            if(id == Guid.Empty) 
            {
                respuesta.Error = new()
                {
                    Codigo = CodigosError.EVALUACION_ID_NO_INGRESADO,
                    Mensaje = "Id no proporcionado",
                    HttpCode = HttpCode.BadRequest
                };
                respuesta.HttpCode= HttpCode.BadRequest;
                return respuesta;
            }

            _evaluacion = _dbSetEvaluacion.FirstOrDefault(_ => _.Id == Guid.Parse(parametros["n0Id"])
                                                 && _.DominioId == Guid.Parse(this._contextoUsuario.DominioId)
                                                 && _.OUId == Guid.Parse(this._contextoUsuario.UOrgId));
            if (_evaluacion == null)
            {
                respuesta.Error = new()
                {
                    Codigo = CodigosError.EVALUACION_NO_ENCONTRADA,
                    Mensaje = "No se ha encontrado la Evaluacion",
                    HttpCode = HttpCode.NotFound,
                };
                respuesta.HttpCode = HttpCode.NotFound;
                return respuesta;
            }

            var reactivo = _evaluacion.Temas.SelectMany(_ => _.Reactivos).FirstOrDefault(_ => _.Id == parametros["n1Id"]);

            if(reactivo == null)
            {
                respuesta.Error = new ErrorProceso()
                {
                    Codigo = CodigosError.EVALUACION_REACTIVO_NO_ENCONTRADO,
                    Mensaje = "No se ha encontrado el Reactivo para actualizar",
                    HttpCode = HttpCode.NotFound,
                };
                respuesta.HttpCode = HttpCode.NotFound;
                return respuesta;
            }

            var temaEvaluacion = _evaluacion.Temas.FirstOrDefault(reactivo => reactivo.Reactivos.Any(_ => _.Id == parametros["n1Id"]));
            _evaluacion.TotalReactivos--;
            temaEvaluacion.Reactivos.Remove(reactivo);

            _dbSetEvaluacion.Update(_evaluacion);
            await _db.SaveChangesAsync();
            respuesta.Ok = true;
            respuesta.HttpCode = HttpCode.Ok;
        }
        catch(Exception ex)
        {
            _logger.LogError(ex, "ServicioEvaluacion-Eliminar {msg}", ex.Message);
            respuesta.Error = new ErrorProceso() { Codigo = CodigosError.EVALUACION_ERROR_DESCONOCIDO, HttpCode = HttpCode.ServerError, Mensaje = ex.Message };
            respuesta.HttpCode = HttpCode.ServerError;
        }
        return respuesta;
    }

    public async Task<Respuesta> Actualizar(Guid id, ReactivoActualizar data, StringDictionary? parametros = null)
    {
        var respuesta = new Respuesta();

        try
        {
            if(id == Guid.Empty || data == null)
            {
                respuesta.Error = new()
                {
                    Codigo = CodigosError.EVALUACION_ID_PAYLOAD_NO_INGRESADO,
                    Mensaje = "No se ha proporcionado Id o Payload",
                    HttpCode = HttpCode.BadRequest
                };
                respuesta.HttpCode = HttpCode.BadRequest;
                return respuesta;
            }

            _evaluacion = _dbSetEvaluacion.FirstOrDefault(_ => _.Id == Guid.Parse(parametros["n0Id"])
                                                             && _.DominioId == Guid.Parse(this._contextoUsuario.DominioId)
                                                             && _.OUId == Guid.Parse(this._contextoUsuario.UOrgId));
            if (_evaluacion == null)
            {
                respuesta.Error = new()
                {
                    Codigo = CodigosError.EVALUACION_NO_ENCONTRADA,
                    Mensaje = "No se ha encontrado la Evaluacion",
                    HttpCode = HttpCode.NotFound,
                };
                respuesta.HttpCode = HttpCode.NotFound;
                return respuesta;
            }

            var reactivo = _evaluacion.Temas.SelectMany(_ => _.Reactivos).FirstOrDefault(_ => _.Id == id.ToString());

            if(reactivo == null)
            {
                respuesta.Error = new ErrorProceso()
                {
                    Codigo = CodigosError.EVALUACION_REACTIVO_NO_ENCONTRADO,
                    Mensaje = "No se ha encontrado el Reactivo para actualizar",
                    HttpCode = HttpCode.NotFound,
                };
                respuesta.HttpCode = HttpCode.NotFound;
                return respuesta;
            }

            reactivo.Obligatorio = data.Obligatorio;
            var diferencia = data.Puntaje - reactivo.Puntaje;
            reactivo.Puntaje = data.Puntaje;
            if (reactivo.Puntaje < 0) reactivo.Puntaje = 0;

            _dbSetEvaluacion.Update(_evaluacion);
            await _db.SaveChangesAsync();
            respuesta.Ok = true;
            respuesta.HttpCode = HttpCode.Ok;

        }
        catch(Exception ex)
        {
            _logger.LogError(ex, "ServicioEvaluacion-Actualizar {msg}", ex.Message);
            respuesta.Error = new ErrorProceso() { Codigo = CodigosError.EVALUACION_ERROR_DESCONOCIDO, HttpCode = HttpCode.ServerError, Mensaje = ex.Message };
            respuesta.HttpCode = HttpCode.ServerError;
        }
        return respuesta;
    }

    public Task<RespuestaPayload<ReactivoTema>> UnicaPorId(Guid id, StringDictionary? parametros = null)
    {
        throw new NotImplementedException();
    }

    public Task<RespuestaPayload<ReactivoTema>> UnicaPorIdDespliegue(Guid id, StringDictionary? parametros = null)
    {
        throw new NotImplementedException();
    }

    #endregion
}
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
#pragma warning restore CS8603 // Possible null reference return