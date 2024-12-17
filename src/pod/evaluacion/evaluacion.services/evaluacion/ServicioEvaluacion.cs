#pragma warning disable CS8603 // Possible null reference return.
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
using Amazon.Runtime.Internal;
using apigenerica.model.interpretes;
using apigenerica.model.modelos;
using apigenerica.model.reflectores;
using apigenerica.model.servicios;
using comunes.primitivas;
using comunes.primitivas.configuracion.mongo;
using evaluacion.model;
using evaluacion.model.evaluacion;
using evaluacion.model.evaluacion.temas;
using evaluacion.model.reactivos;
using evaluacion.services.dbcontext;
using extensibilidad.metadatos;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using System.Collections.Specialized;
using System.Text.Json;

namespace evaluacion.services.evaluacion;
[ServicioEntidadAPI(typeof(Evaluacion))]
public class ServicioEvaluacion : ServicioEntidadGenericaBase<Evaluacion, EvaluacionInsertar, EvaluacionActualizar, EvaluacionDespliegue, Guid>,
    IServicioEntidadAPI, IServicioEvaluacion
{
    private readonly ILogger<ServicioEvaluacion> _logger;
    private readonly IReflectorEntidadesAPI _reflector;
    private readonly IDistributedCache _cache;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private const string _DOMINIOHEADER = "x-d-id";
    private const string _UORGHEADER = "x-uo-id";
    public ServicioEvaluacion(ILogger<ServicioEvaluacion> logger, IServicionConfiguracionMongo configuracionMongo, IReflectorEntidadesAPI reflector, IDistributedCache cache, IHttpContextAccessor httpContextAccessor) : base(null, null, logger, reflector, cache)
    {
        _logger = logger;
        _reflector = reflector;
        _cache = cache;
        _httpContextAccessor = httpContextAccessor;
        interpreteConsulta = new InterpreteConsultaExpresiones();

        var configuracionEntidad = configuracionMongo.ConexionEntidad(MongoDbContextEvaluacion.NOMBRE_COLECCION_EVALUACION);

        if(configuracionEntidad == null)
        {
            string err = $"No existe configuración de mondo para '{MongoDbContextEvaluacion.NOMBRE_COLECCION_EVALUACION}'";
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
            _dbSetFull = ((MongoDbContextEvaluacion)_db).Evaluaciones;

        }
        catch(Exception ex)
        {
            _logger.LogError(ex, $"Error al inicializar mongo para '{MongoDbContextEvaluacion.NOMBRE_COLECCION_EVALUACION}'");
            throw;
        }
    }

    public bool RequiereAutenticacion => true;
    private MongoDbContextEvaluacion DB { get { return (MongoDbContextEvaluacion)_db; } }

    public async Task<Respuesta> ActualizarAPI(object id, JsonElement data, StringDictionary? parametros = null)
    {
        _logger.LogDebug("ServicioEvaluación - ActualizarAPI - {data}", data);
        var update = data.Deserialize<EvaluacionActualizar>(JsonAPIDefaults());
        Respuesta respuesta = await this.Actualizar(Guid.Parse((string)id), update, parametros);
        _logger.LogDebug("ServicioEvaluacion-ActualizarAPI resultado {ok} {code} {error}", respuesta!.Ok, respuesta!.HttpCode, respuesta.Error);
        return respuesta;
    }

    public async Task<Respuesta> EliminarAPI(object id, StringDictionary? parametros = null)
    {
        _logger.LogDebug("ServicioEvaluación-EliminarAPI");
        Respuesta respuesta = await this.Eliminar(Guid.Parse((string)id), parametros);
        _logger.LogDebug("ServicioEvaluación-EliminarAPI resultado {ok} {code} {error}", respuesta!.Ok, respuesta!.HttpCode, respuesta.Error);
        return respuesta;
    }


    public async Task<RespuestaPayload<object>> InsertarAPI(JsonElement data, StringDictionary? parametros = null)
        {
        _logger.LogDebug("ServicioEvaluación - InsertarAPI - {data}", data);
        var add = data.Deserialize<EvaluacionInsertar>(JsonAPIDefaults());
        var temp = await this.Insertar(add, parametros);
        RespuestaPayload<object> respuesta = JsonSerializer.Deserialize<RespuestaPayload<object>>(JsonSerializer.Serialize(temp));
        _logger.LogDebug("ServicioEvaluación - InsertarAPI resultado {ok} {code} {error}", respuesta!.Ok, respuesta!.HttpCode,
            respuesta.Error);
        return respuesta;
    }

    public async Task<RespuestaPayload<PaginaGenerica<object>>> PaginaAPI(Consulta consulta, StringDictionary? parametros = null)
    {
        _logger.LogDebug("ServicioEvaluación - PaginaAPI - {consulta}", consulta);
        var temp = await this.Pagina(consulta, parametros);
        RespuestaPayload<PaginaGenerica<object>> respuesta = JsonSerializer.Deserialize<RespuestaPayload<PaginaGenerica<object>>>(JsonSerializer.Serialize(temp));
        _logger.LogDebug("ServicioEvaluación - PaginaAPI resultado {ok} {code} {error}", respuesta!.Ok, respuesta!.HttpCode, respuesta.Error);
        return respuesta;
    }
    public async Task<RespuestaPayload<PaginaGenerica<object>>> PaginaDespliegueAPI(Consulta consulta, StringDictionary? parametros = null)
    {
        _logger.LogDebug("ServicioEvaluación - PaginaDespliegueAPI - {consulta}", consulta);
        var temp = await this.PaginaDespliegue(consulta, parametros);
        RespuestaPayload<PaginaGenerica<object>> respuesta = JsonSerializer.Deserialize<RespuestaPayload<PaginaGenerica<object>>>(JsonSerializer.Serialize(temp));
        _logger.LogDebug("ServicioEvaluación - PaginaDespliegueAPI resutlado {ok} {code} {error}", respuesta!.Ok, respuesta!.HttpCode, respuesta.Error);
        return respuesta;
    }

    public async Task<RespuestaPayload<object>> UnicaPorIdAPI(object id, StringDictionary? parametros = null)
    {
        _logger.LogDebug("ServicioEvaluación - UnicoPorIdAPI");
        var temp = await this.UnicaPorId(Guid.Parse((string)id),parametros);
        RespuestaPayload<object> respuesta = JsonSerializer.Deserialize<RespuestaPayload<object>>(JsonSerializer.Serialize(temp));
        _logger.LogDebug("ServicioEvaluación - UnicoPorIdAPI - resultado {ok} {code} {error}", respuesta!.Ok, respuesta!.HttpCode, respuesta.Error);
        return respuesta;
    }

    public async Task<RespuestaPayload<object>> UnicaPorIdDespliegueAPI(object id, StringDictionary? parametros = null)
    {
        _logger.LogDebug("ServicioEvaluación - UnicoPorIdDespliegueAPI");
        var temp = await this.UnicaPorIdDespliegue(Guid.Parse((string)id), parametros);
        RespuestaPayload<object> respuesta = JsonSerializer.Deserialize<RespuestaPayload<object>>(JsonSerializer.Serialize(temp));
        _logger.LogDebug("ServicioEvaluación - UnicaPorIdDespliegueAPI - resultado {ok} {code} {error}", respuesta!.Ok, respuesta!.HttpCode, respuesta.Error);
        return respuesta;
    }

    public Entidad EntidadRepoAPI()
    {
        _logger.LogDebug("ServicioEvaluación - ServicioEntidadRepoAPI");
        return EntidadRepo();
    }

    public Entidad EntidadInsertAPI()
    {
        _logger.LogDebug("ServicioEvaluación - ServicioEntidadInsertAPI");
        return EntidadInsert();
    }

    public Entidad EntidadUpdateAPI()
    {
        _logger.LogDebug("ServicioEvaluación - ServicioEntidadUpdateAPI");
        return EntidadUpdate();
    }

    public Entidad EntidadDespliegueAPI()
    {
        _logger.LogDebug("ServicioEvaluación - EntidadDespliegueAPI");
        return EntidadDespliegue();
    }

    public void EstableceContextoUsuarioAPI(ContextoUsuario contexto)
    {
        _logger.LogDebug("ServicioEvaluación - EstableceContextoUsuarioAPI");
        this.EstableceContextoUsuario(contexto);
    }

    public ContextoUsuario? ObtieneContextoUsuarioAPI()
    {
        _logger.LogDebug("ServicioEvaluacion - ObtieneContextoUsuarioAPI");
        return this._contextoUsuario;
    }

    #region Overrides para la entidad Evaluación

    public async Task<ResultadoValidacion> ValidarActualizar(Guid id, EvaluacionActualizar actualizacion, Evaluacion original)
    {
        return new ResultadoValidacion() { Valido = true};
    }

    public async Task<ResultadoValidacion> ValidarEliminacion(Guid id, Evaluacion original)
    {
        return new ResultadoValidacion() { Valido = true };
    }

    public async Task<ResultadoValidacion> ValidarInsertar(EvaluacionInsertar data)
    {
        return new ResultadoValidacion() { Valido = true };
    }

    public override Evaluacion ADTOFull(EvaluacionInsertar data)
    {
        return new Evaluacion()
        {
            Id = Guid.NewGuid(),
            Nombre = data.Nombre,
            ParticipantesFijos = data.ParticipantesFijos,
            DominioId = Guid.Parse(this._contextoUsuario.DominioId),
            OUId = Guid.Parse(this._contextoUsuario.UOrgId),
            CreadorId = Guid.Parse(this._contextoUsuario.UsuarioId),
        };
    }

    public override Evaluacion ADTOFull(EvaluacionActualizar actualizacion, Evaluacion actual)
    {
        actual.Nombre = actualizacion.Nombre;
        actual.ParticipantesFijos = actualizacion.ParticipantesFijos;
        return actual;
    }

    public override EvaluacionDespliegue ADTODespliegue(Evaluacion data)
    {
        return new EvaluacionDespliegue()
        {
            Id = data.Id,
            Nombre = data.Nombre,
            CreadorId = data.CreadorId,
            FechaCreacion = data.FechaCreacion,
            ParticipantesFijos = data.ParticipantesFijos,
            Temas = data.Temas,
            TotalReactivos = data.TotalReactivos, 
            Estado = data.Estado
        };
    }

    public async Task<Respuesta> Actualizar(Guid id, EvaluacionActualizar data, StringDictionary? parametros = null)
    {
        var respuesta = new Respuesta();
        try
        {

            if(id == Guid.Empty || data == null)
            {
                respuesta.Error = new ErrorProceso()
                {
                    Codigo = CodigosError.EVALUACION_ID_PAYLOAD_NO_INGRESADO,
                    Mensaje = "No existe una entidad Evaluación con el Id proporcinado",
                    HttpCode = HttpCode.BadRequest
                };

                respuesta.HttpCode = HttpCode.BadRequest;
                return respuesta;
            }

            Evaluacion  actual = _dbSetFull.Find(id);

            if(actual == null)
            {
                respuesta.Error = new ErrorProceso()
                {
                    Codigo = CodigosError.EVALUACION_NO_ENCONTRADA,
                    Mensaje = "No existe una Entidad Evaluación con el Id proporcionado",
                    HttpCode = HttpCode.NotFound
                };
                respuesta.HttpCode = HttpCode.NotFound;
                return respuesta;
            }


            var resultadoValidacion = await ValidarActualizar(id, data, actual);
            if (resultadoValidacion.Valido)
            {
                var entidad = ADTOFull(data,actual);
                _dbSetFull.Update(entidad);
                await _db.SaveChangesAsync();

                respuesta.Ok = true;
                respuesta.HttpCode = HttpCode.Ok;
            }
            else
            {
                respuesta.Error = resultadoValidacion.Error;
                respuesta.Error!.Codigo = CodigosError.EVALUACION_DATOS_NO_VALIDOS;
                respuesta.HttpCode = resultadoValidacion.Error?.HttpCode ?? HttpCode.None;
            }

        }
        catch(Exception ex)
        {
            _logger.LogError(ex, "ServicioEvaluación - Actualizar {msg}", ex.Message);
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
                    Mensaje = "No ha sido proporcionado el Id",
                    HttpCode = HttpCode.BadRequest
                };
                respuesta.HttpCode = HttpCode.BadRequest;
                return respuesta;
            }

            Evaluacion actual = _dbSetFull.Find(id);

            if(actual == null)
            {
                respuesta.Error = new()
                {
                    Codigo = CodigosError.EVALUACION_NO_ENCONTRADA,
                    Mensaje = "No existe una enitdad Evaluación con el Id proporcionado",
                    HttpCode = HttpCode.NotFound
                };
                respuesta.HttpCode = HttpCode.NotFound;
                return respuesta;
            }

            var resultadoValidacion = await ValidarEliminacion(id, actual);
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
                respuesta.Error!.Codigo = CodigosError.EVALUACION_DATOS_NO_VALIDOS;
                respuesta.HttpCode = resultadoValidacion.Error?.HttpCode ?? HttpCode.None;
            }

        }
        catch(Exception ex)
        {
            _logger.LogError(ex, "ServicioEvaluación-Eliminar {msg}", ex.Message);
            respuesta.Error = new ErrorProceso() { Codigo = CodigosError.EVALUACION_ERROR_DESCONOCIDO, HttpCode = HttpCode.ServerError, Mensaje = ex.Message };
            respuesta.HttpCode = HttpCode.ServerError;
        }
        return respuesta;

    }

    public async Task<RespuestaPayload<Evaluacion>> UnicaPorId(Guid id, StringDictionary? parametros = null)
    {
        var respuesta = new RespuestaPayload<Evaluacion>();

        try
        {
            Evaluacion actual = await _dbSetFull.FindAsync(id);
            if (actual == null)
            {
                respuesta.Error = new ErrorProceso()
                {
                    Codigo = CodigosError.EVALUACION_NO_ENCONTRADA,
                    Mensaje = "No existe una Entidad Evaluación con el Id proporcionado",
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
            _logger.LogError(ex, "ServicioEvaluación-UnicaPorId {msg}", ex.Message);
            respuesta.Error = new ErrorProceso() { Codigo = CodigosError.EVALUACION_ERROR_DESCONOCIDO, HttpCode = HttpCode.ServerError, Mensaje = ex.Message };
            respuesta.HttpCode = HttpCode.ServerError;

        }
        return respuesta;
    }


    public async Task<RespuestaPayload<EvaluacionDespliegue>> UnicaPorIdDespliegue(Guid id, StringDictionary? parametros  = null)
    {
        RespuestaPayload<EvaluacionDespliegue> respuesta = new RespuestaPayload<EvaluacionDespliegue>();

        try
        {
            var resultado = await UnicaPorId(id, parametros);

            respuesta.Ok = resultado.Ok;
            if(resultado.Ok) 
            {
                respuesta.Payload = ADTODespliegue((Evaluacion)resultado.Payload);
            }
            else
            {
                respuesta.Error = resultado.Error;
                respuesta.Error!.Codigo = CodigosError.EVALUACION_DATOS_NO_VALIDOS;
                respuesta.HttpCode = resultado.Error?.HttpCode ?? HttpCode.None;
            }
        }
        catch(Exception ex)
        {
            _logger.LogError(ex, "ServicioEvaluacion - UnicaPorid {msg}", ex.Message);
            respuesta.Error = new ErrorProceso() { Codigo = CodigosError.EVALUACION_ERROR_DESCONOCIDO, HttpCode = HttpCode.ServerError, Mensaje = ex.Message};
            respuesta.HttpCode = HttpCode.ServerError;
        }
        return respuesta;
    }

    public async Task<Respuesta> CambiarEstado(Guid evaluacionId, EstadoEvaluacion estadoEvaluacion)
    {
        var respuesta = new Respuesta();
        
        var evaluacion =  _dbSetFull.FirstOrDefault(e => e.Id == evaluacionId && e.DominioId == Guid.Parse(_httpContextAccessor.HttpContext.Request.Headers[_DOMINIOHEADER]) && e.OUId == Guid.Parse(_httpContextAccessor.HttpContext.Request.Headers[_UORGHEADER]));

        if(evaluacion == null)
        {
            respuesta.Error = new ErrorProceso()
            {
                Codigo = CodigosError.EVALUACION_ESTADONUEVO_ERROR,
                Mensaje = "Verifique que exista la evaluación, dominio y UnidadOrganizacional para realizar la acción",
                HttpCode = HttpCode.NotFound
            };
            respuesta.HttpCode = HttpCode.NotFound;
            return respuesta;
        }

        evaluacion.Estado = estadoEvaluacion;
        _dbSetFull.Update(evaluacion);
        await _db.SaveChangesAsync();

        respuesta.Ok = true;
        respuesta.HttpCode = HttpCode.Ok;
        return respuesta;
    }

    public async Task<Respuesta> ReactivoMultipleCrear(Guid evaluacionId, ReactivoMultipleCrear multipleCrear)
    {
        var respuesta = new Respuesta();

        try
        {
            var evaluacion = _dbSetFull.FirstOrDefault(e => e.Id == evaluacionId && e.DominioId == Guid.Parse(_httpContextAccessor.HttpContext.Request.Headers[_DOMINIOHEADER]) && e.OUId == Guid.Parse(_httpContextAccessor.HttpContext.Request.Headers[_UORGHEADER]));

            if (evaluacion != null)
            {
                foreach (var data in multipleCrear.Reactivos)
                {
                    var temaEvaluacion = evaluacion.Temas.FirstOrDefault(_ => _.TemaId == data.TemaId && _.TemarioId == data.TemarioId);

                    var reactivoTema = new ReactivoTema() { Id = data.ReactivoId, Dificultad = data.Dificultad, Puntaje = data.Puntaje, Obligatorio = data.Obligatorio };

                    if (temaEvaluacion == null)
                    {
                        var temaEvaluacionCrea = new TemaEvaluacion()
                        {
                            TemarioId = data.TemarioId,
                            TemaId = data.TemaId,
                        };

                        temaEvaluacionCrea.Reactivos.Add(reactivoTema);
                        evaluacion.Temas.Add(temaEvaluacionCrea);
                        evaluacion.TotalReactivos++;


                        _dbSetFull.Update(evaluacion);
                        await _db.SaveChangesAsync();

                        respuesta.Ok = true;
                        respuesta.HttpCode = HttpCode.Ok;

                    }
                    else
                    {

                        var reactivo = temaEvaluacion.Reactivos.FirstOrDefault(_ => _.Id == reactivoTema.Id);

                        if (reactivo == null)
                        {
                            temaEvaluacion.Reactivos.Add(reactivoTema);
                            evaluacion.TotalReactivos++;
                            _dbSetFull.Update(evaluacion);
                            await _db.SaveChangesAsync();
                            respuesta.Ok = true;
                            respuesta.HttpCode = HttpCode.Ok;
                        }
                    }
                }
            }
            else
            {
                respuesta.Error = new()
                {
                    Codigo = CodigosError.EVALUACION_NO_ENCONTRADA,
                    Mensaje = "No existe una enitdad Evaluación",
                    HttpCode = HttpCode.NotFound
                };
                respuesta.HttpCode = HttpCode.NotFound;
                return respuesta;
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

    public async Task<Respuesta> ReactivoMultipleEliminar(Guid evaluacionId, ReactivoMultipleEliminar reactivos)
    {
        var respuesta = new Respuesta();
        try
        {
            if (evaluacionId == Guid.Empty)
            {
                respuesta.Error = new()
                {
                    Codigo = CodigosError.EVALUACION_ID_NO_INGRESADO,
                    Mensaje = "Id no proporcionado de la Evaluacion",
                    HttpCode = HttpCode.BadRequest
                };
                respuesta.HttpCode = HttpCode.BadRequest;
                return respuesta;
            }

            var evaluacion = _dbSetFull.FirstOrDefault(e => e.Id == evaluacionId && e.DominioId == Guid.Parse(_httpContextAccessor.HttpContext.Request.Headers[_DOMINIOHEADER]) && e.OUId == Guid.Parse(_httpContextAccessor.HttpContext.Request.Headers[_UORGHEADER]));
            if (evaluacion == null)
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

            foreach (var reactivoId in reactivos.Ids)
            {

                var reactivo = evaluacion.Temas.SelectMany(_ => _.Reactivos).FirstOrDefault(_ => _.Id == reactivoId.ToString());

                if (reactivo == null)
                {
                    respuesta.Error = new ErrorProceso()
                    {
                        Codigo = CodigosError.EVALUACION_REACTIVO_NO_ENCONTRADO,
                        Mensaje = "No se ha encontrado el Reactivo para eliminar",
                        HttpCode = HttpCode.NotFound,
                    };
                    respuesta.HttpCode = HttpCode.NotFound;
                    return respuesta;
                }

                var temaEvaluacion = evaluacion.Temas.FirstOrDefault(reactivo => reactivo.Reactivos.Any(_ => _.Id == reactivoId.ToString()));
                evaluacion.TotalReactivos--;
                temaEvaluacion.Reactivos.Remove(reactivo);

                _dbSetFull.Update(evaluacion);
                await _db.SaveChangesAsync();
                respuesta.Ok = true;
                respuesta.HttpCode = HttpCode.Ok;
            }

        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "ServicioEvaluacion-Eliminar {msg}", ex.Message);
            respuesta.Error = new ErrorProceso() { Codigo = CodigosError.EVALUACION_ERROR_DESCONOCIDO, HttpCode = HttpCode.ServerError, Mensaje = ex.Message };
            respuesta.HttpCode = HttpCode.ServerError;
        }
        return respuesta;
    }

    #endregion


}
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
#pragma warning restore CS8603 // Possible null reference return.