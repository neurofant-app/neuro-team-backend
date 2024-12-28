using apigenerica.model.interpretes;
using apigenerica.model.modelos;
using apigenerica.model.reflectores;
using apigenerica.model.servicios;
using comunes.primitivas;
using comunes.primitivas.configuracion.mongo;
using controlescolar.modelo.escuela;
using controlescolar.modelo.plantel;
using controlescolar.servicios.dbcontext;
using extensibilidad.metadatos;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using System.Collections.Specialized;
using System.Text.Json;
#pragma warning disable CS8603 // Possible null reference return.
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
namespace controlescolar.servicios.entidadplantel;
[ServicioEntidadAPI(entidad: typeof(EntidadPlantel))]
public class ServicioEntidadPlantel : ServicioEntidadGenericaBase<EntidadPlantel, CreaPlantel, ActualizaPlantel, ConsultaPlantel, string>,
    IServicioEntidadAPI, IServicioEntidadPlantel
{
    private readonly ILogger<ServicioEntidadPlantel> _logger;
    private readonly IReflectorEntidadesAPI _reflector;
    private EntidadEscuela? _entidadEscuela;
    private DbSet<EntidadEscuela> _dbSetEntidadEscuela;

    public ServicioEntidadPlantel(ILogger<ServicioEntidadPlantel> logger, IServicionConfiguracionMongo configuracionMongo, IReflectorEntidadesAPI reflector,
        IDistributedCache cache) : base(null, null, logger, reflector, cache)
    {
        _logger = logger;
        _reflector = reflector;
        interpreteConsulta = new InterpreteConsultaExpresiones();

        var configuracionEntidad = configuracionMongo.ConexionEntidad(MongoDbContext.NOMBRE_COLECCION_ESCUELAS);

        if (configuracionEntidad == null)
        {
            string err = $"No existe configuracion de mongo para '{MongoDbContext.NOMBRE_COLECCION_ESCUELAS}'";
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

            _db = MongoDbContext.Create(client.GetDatabase(configuracionEntidad.Esquema));
            _dbSetEntidadEscuela = ((MongoDbContext)_db).Escuelas;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error al inicializar mongo para '{MongoDbContext.NOMBRE_COLECCION_ESCUELAS}'");
        }

    }

    private MongoDbContext DB { get { return (MongoDbContext)_db; } }

    public bool RequiereAutenticacion => true;

    public async Task<Respuesta> ActualizarAPI(object id, JsonElement data, StringDictionary? parametros = null)
    {
        _logger.LogDebug("ServicioEntidadPlantel-ActualizarAPI-{data}", data);
        var update = data.Deserialize<ActualizaPlantel>(JsonAPIDefaults());
        Respuesta respuesta = await this.Actualizar((string)id, update, parametros);
        _logger.LogDebug("ServicioEntidadPlantel-ActualizarAPI resultado {ok} {code} {error}", respuesta!.Ok, respuesta!.HttpCode, respuesta.Error);
        return respuesta;
    }

    public async Task<Respuesta> EliminarAPI(object id, StringDictionary? parametros = null, bool forzarEliminacion = false)
    {
        _logger.LogDebug("ServicioEntidadPlantel-EliminarAPI");
        Respuesta respuesta = await this.Eliminar((string)id, parametros, forzarEliminacion);
        _logger.LogDebug("ServicioEntidadPlantel-EliminarAPI resultado {ok} {code} {error}", respuesta!.Ok, respuesta!.HttpCode, respuesta.Error);
        return respuesta;
    }

    public Entidad EntidadDespliegueAPI()
    {
        _logger.LogDebug("ServicioEntidadPlantel-EntidadDespliegueAPI");
        return this.EntidadDespliegue();
    }

    public Entidad EntidadInsertAPI()
    {
        _logger.LogDebug("ServicioEntidadPlantel-EntidadInsertAPI");
        return this.EntidadInsert();
    }

    public Entidad EntidadRepoAPI()
    {
        _logger.LogDebug("ServicioEntidadPlantel-EntidadRepoAPI");
        return this.EntidadRepo();
    }

    public Entidad EntidadUpdateAPI()
    {
        _logger.LogDebug("ServicioEntidadPlantel-EntidadUpdateAPI");
        return this.EntidadUpdate();
    }

    public void EstableceContextoUsuarioAPI(ContextoUsuario contexto)
    {
        _logger.LogDebug("ServicioEntidadPlantel-EstableceContextoUsuarioAPI");
        this.EstableceContextoUsuario(contexto);
    }

    public async Task<RespuestaPayload<object>> InsertarAPI(JsonElement data, StringDictionary? parametros = null)
    {
        _logger.LogDebug("ServicioEntidadPlantel-InsertarAPI-{data}", data);
        var add = data.Deserialize<CreaPlantel>(JsonAPIDefaults());
        var temp = await this.Insertar(add, parametros);
        RespuestaPayload<object> respuesta = JsonSerializer.Deserialize<RespuestaPayload<object>>(JsonSerializer.Serialize(temp));
        _logger.LogDebug("ServicioEntidadPlantel-InsertarAPI resultado {ok} {code} {error}", respuesta!.Ok, respuesta!.HttpCode, respuesta.Error);
        return respuesta;
    }

    public ContextoUsuario? ObtieneContextoUsuarioAPI()
    {
        _logger.LogDebug("ServicioEntidadPlantel-ObtieneContextoUsuarioAPI");
        return this._contextoUsuario;
    }
    public async Task<RespuestaPayload<PaginaGenerica<object>>> PaginaAPI(Consulta consulta, StringDictionary? parametros = null)
    {
        _logger.LogDebug("ServicioEntidadPlantel-PaginaAPI-{consulta}", consulta);
        var temp = await this.Pagina(consulta, parametros);
        RespuestaPayload<PaginaGenerica<object>> respuesta = JsonSerializer.Deserialize<RespuestaPayload<PaginaGenerica<object>>>(JsonSerializer.Serialize(temp));
        _logger.LogDebug("ServicioEntidadPlantel-PaginaAPI resultado {ok} {code} {error}", respuesta!.Ok, respuesta!.HttpCode, respuesta.Error);
        return respuesta;
    }
    public async Task<RespuestaPayload<PaginaGenerica<object>>> PaginaDespliegueAPI(Consulta consulta, StringDictionary? parametros = null)
    {
        _logger.LogDebug("ServicioEntidadPlantel-PaginaDespliegueAPI-{consulta}", consulta);
        var temp = await this.PaginaDespliegue(consulta, parametros);
        RespuestaPayload<PaginaGenerica<object>> respuesta = JsonSerializer.Deserialize<RespuestaPayload<PaginaGenerica<object>>>(JsonSerializer.Serialize(temp));
        _logger.LogDebug("ServicioEntidadPlantel-PaginaDespliegueAPI resultado {ok} {code} {error}", respuesta!.Ok, respuesta!.HttpCode, respuesta.Error);
        return respuesta;
    }
    public async Task<RespuestaPayload<object>> UnicaPorIdAPI(object id, StringDictionary? parametros = null)
    {
        _logger.LogDebug("ServicioEntidadPlantel-UnicaPorIdAPI");
        var temp = await this.UnicaPorId((string)id, parametros);
        RespuestaPayload<object> respuesta = JsonSerializer.Deserialize<RespuestaPayload<object>>(JsonSerializer.Serialize(temp));
        _logger.LogDebug("ServicioEntidadPlantel-UnicaPorIdAPI resultado {ok} {code} {error}", respuesta!.Ok, respuesta!.HttpCode, respuesta.Error);
        return respuesta;
    }
    public async Task<RespuestaPayload<object>> UnicaPorIdDespliegueAPI(object id, StringDictionary? parametros = null)
    {
        _logger.LogDebug("ServicioEntidadPlantel-UnicaPorIdDespliegueAPI");
        var temp = await this.UnicaPorIdDespliegue((string)id, parametros);
        RespuestaPayload<object> respuesta = JsonSerializer.Deserialize<RespuestaPayload<object>>(JsonSerializer.Serialize(temp));
        _logger.LogDebug("ServicioEntidadPlantel-UnicaPorIdDespliegueAPI resultado {ok} {code} {error}", respuesta!.Ok, respuesta!.HttpCode, respuesta.Error);
        return respuesta;
    }

    #region Overrides para la personalización de la ENTIDAD => EntidadPlantel
    public override async Task<ResultadoValidacion> ValidarInsertar(CreaPlantel data)
    {
        return new ResultadoValidacion()
        {
            Valido = true
        };
    }

    public override async Task<ResultadoValidacion> ValidarEliminacion(string id, EntidadPlantel data, bool forzarEliminacion = false)
    {
        return new ResultadoValidacion() { Valido = true };
    }

    public override async Task<ResultadoValidacion> ValidarActualizar(string id, ActualizaPlantel actualizacion, EntidadPlantel original)
    {
        return new ResultadoValidacion() { Valido = true };
    }

    public override EntidadPlantel ADTOFull(ActualizaPlantel actualizacion, EntidadPlantel actual)
    {
        actual.Id = actualizacion.Id;
        actual.EscuelaId = actualizacion.EscuelaId;
        actual.Nombre = actualizacion.Nombre;
        actual.Clave = actualizacion.Nombre;
        return actual;
    }

    public override EntidadPlantel ADTOFull(CreaPlantel data)
    {
        return new EntidadPlantel()
        {
            Id = Guid.NewGuid(),
            EscuelaId = data.EscuelaId,
            Nombre = data.Nombre,
            Clave = data.Clave,
            Activo = true,
            Creacion = DateTime.UtcNow,
            EsVirtual = true
        };
    }

    public override ConsultaPlantel ADTODespliegue(EntidadPlantel data)
    {
        return new ConsultaPlantel()
        {
            Id = data.Id,
            Nombre = data.Nombre,
            Activo = data.Activo,
            Creacion = data.Creacion,
            EscuelaId = data.EscuelaId,
            Clave = data.Clave
        };
    }

    public override async Task<RespuestaPayload<ConsultaPlantel>> Insertar(CreaPlantel data, StringDictionary? parametros = null)
    {
        var respuesta = new RespuestaPayload<ConsultaPlantel>();

        try
        {
            var resultadoValidacion = await ValidarInsertar(data);
            if (resultadoValidacion.Valido)
            {
                var entidad = ADTOFull(data);
                _entidadEscuela = _dbSetEntidadEscuela.FirstOrDefault(_ => _.Id == Guid.Parse(parametros["n0Id"]));
                _entidadEscuela.Planteles.Add(entidad);
                _dbSetEntidadEscuela.Update(_entidadEscuela);
                await _db.SaveChangesAsync();
                respuesta.Ok = true;
                respuesta.HttpCode = HttpCode.Ok;
                respuesta.Payload = ADTODespliegue(entidad);
            }
            else
            {
                respuesta.Error = resultadoValidacion.Error;
                respuesta.Error!.Codigo = CodigosError.CONTROLESCOLAR_DATOS_NO_VALIDOS;
                respuesta.HttpCode = resultadoValidacion.Error?.HttpCode ?? HttpCode.None;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "ServicioEntidadPlantel-Insertar {msg}", ex.Message);
            respuesta.Error = new ErrorProceso() { Codigo = CodigosError.CONTROLESCOLAR_ERROR_DESCONOCIDO, HttpCode = HttpCode.ServerError, Mensaje = ex.Message };
            respuesta.HttpCode = HttpCode.ServerError;
        }

        return respuesta;
    }

    public override async Task<Respuesta> Actualizar(string id, ActualizaPlantel data, StringDictionary? parametros = null)
    {
        var respuesta = new Respuesta();
        try
        {
            if (string.IsNullOrEmpty(id.ToString()) || data == null)
            {
                respuesta.Error = new ErrorProceso()
                {
                    Codigo = CodigosError.CONTROLESCOLAR_ENTIDADPLANTEL_ID_PAYLOAD_NO_INGRESADO,
                    Mensaje = "No ha sido proporcionado el Id ó Payload",
                    HttpCode = HttpCode.BadRequest
                };
                respuesta.HttpCode = HttpCode.BadRequest;
                return respuesta;
            }
            _entidadEscuela = _dbSetEntidadEscuela.FirstOrDefault(_ => _.Id == new Guid(parametros["n0Id"]));
            EntidadPlantel actual = _entidadEscuela.Planteles.FirstOrDefault(_ => _.Id.Equals(Guid.Parse(id)));
            if (actual == null)
            {
                respuesta.Error = new ErrorProceso()
                {
                    Codigo = CodigosError.CONTROLESCOLAR_ENTIDADPLANTEL_NO_ENCONTRADA,
                    Mensaje = "No existe un EntidadPlantel con el Id proporcionado",
                    HttpCode = HttpCode.NotFound
                };
                respuesta.HttpCode = HttpCode.NotFound;
                return respuesta;
            }


            var resultadoValidacion = await ValidarActualizar(id.ToString(), data, actual);
            if (resultadoValidacion.Valido)
            {
                var entidad = ADTOFull(data, actual);
                var index = _entidadEscuela.Planteles.IndexOf(entidad);
                if (index == 0)
                {
                    _entidadEscuela.Planteles[0] = entidad;
                    _dbSetEntidadEscuela.Update(_entidadEscuela);
                    await _db.SaveChangesAsync();
                    respuesta.Ok = true;
                    respuesta.HttpCode = HttpCode.Ok;
                }
                else
                {
                    respuesta.Error = new ErrorProceso()
                    {
                        Codigo = CodigosError.CONTROLESCOLAR_ENTIDADPLANTEL_ERROR_ACTUALIZAR,
                        Mensaje = "No ha sido posible actualizar la EntidadPlantel",
                        HttpCode = HttpCode.BadRequest
                    };
                    respuesta.HttpCode = HttpCode.BadRequest;
                    return respuesta;
                }
            }
            else
            {
                respuesta.Error = resultadoValidacion.Error;
                respuesta.Error!.Codigo = CodigosError.CONTROLESCOLAR_DATOS_NO_VALIDOS;
                respuesta.HttpCode = resultadoValidacion.Error?.HttpCode ?? HttpCode.None;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "ServicioEntidadPlantel-Actualizar {msg}", ex.Message);
            respuesta.Error = new ErrorProceso() { Codigo = CodigosError.CONTROLESCOLAR_ERROR_DESCONOCIDO, HttpCode = HttpCode.ServerError, Mensaje = ex.Message };
            respuesta.HttpCode = HttpCode.ServerError;
        }
        return respuesta;
    }

    public override async Task<RespuestaPayload<EntidadPlantel>> UnicaPorId(string id, StringDictionary? parametros = null)
    {
        var respuesta = new RespuestaPayload<EntidadPlantel>();
        try
        {
            _entidadEscuela = _dbSetEntidadEscuela.FirstOrDefault(_ => _.Id == Guid.Parse(parametros["n0Id"]));
            EntidadPlantel actual = _entidadEscuela.Planteles.FirstOrDefault(_ => _.Id.Equals(Guid.Parse(id)));
            if (actual == null)
            {
                respuesta.Error = new ErrorProceso()
                {
                    Codigo = CodigosError.CONTROLESCOLAR_ENTIDADPLANTEL_NO_ENCONTRADA,
                    Mensaje = "No existe un EntidadPlantel con el Id proporcionado",
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
            _logger.LogError(ex, "ServicioEntidadPlantel-UnicaPorId {msg}", ex.Message);
            respuesta.Error = new ErrorProceso() { Codigo = CodigosError.CONTROLESCOLAR_ERROR_DESCONOCIDO, HttpCode = HttpCode.ServerError, Mensaje = ex.Message };
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
                    Codigo = CodigosError.CONTROLESCOLAR_ENTIDADPLANTEL_ID_NO_INGRESADO,
                    Mensaje = "No ha sido proporcionado el Id",
                    HttpCode = HttpCode.BadRequest
                };
                respuesta.HttpCode = HttpCode.BadRequest;
                return respuesta;
            }
            _entidadEscuela = _dbSetEntidadEscuela.FirstOrDefault(_ => _.Id == Guid.Parse(parametros["n0Id"]));
            EntidadPlantel actual = _entidadEscuela.Planteles.FirstOrDefault(_ => _.Id.Equals(Guid.Parse(id)));
            if (actual == null)
            {
                respuesta.Error = new ErrorProceso()
                {
                    Codigo = CodigosError.CONTROLESCOLAR_ENTIDADPLANTEL_NO_ENCONTRADA,
                    Mensaje = "No existe un ENTIDADPLANTEL con el Id proporcionado",
                    HttpCode = HttpCode.NotFound
                };
                respuesta.HttpCode = HttpCode.NotFound;
                return respuesta;
            }

            var resultadoValidacion = await ValidarEliminacion(id, actual);
            if (resultadoValidacion.Valido)
            {
                _entidadEscuela.Planteles.Remove(actual);
                _dbSetEntidadEscuela.Update(_entidadEscuela);
                await _db.SaveChangesAsync();
                respuesta.Ok = true;
                respuesta.HttpCode = HttpCode.Ok;
            }
            else
            {
                respuesta.Error = new ErrorProceso()
                {
                    Codigo = CodigosError.CONTROLESCOLAR_ENTIDADPLANTEL_ERROR_ELIMINAR,
                    Mensaje = "No ha sido posible ELIMINAR la EntidadPlantel",
                    HttpCode = HttpCode.BadRequest
                };
                respuesta.HttpCode = HttpCode.BadRequest;
                return respuesta;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "ServicioEntidadPlantel-Eliminar {msg}", ex.Message);
            respuesta.Error = new ErrorProceso() { Codigo = CodigosError.CONTROLESCOLAR_ERROR_DESCONOCIDO, HttpCode = HttpCode.ServerError, Mensaje = ex.Message };
            respuesta.HttpCode = HttpCode.ServerError;
        }
        return respuesta;
    }

    public override async Task<PaginaGenerica<EntidadPlantel>> ObtienePaginaElementos(Consulta consulta, StringDictionary? parametros = null)
    {
        _logger.LogDebug("ServicioEntidadPlantel - ObtienePaginaElementos - {consulta}", consulta);
        Entidad entidad = reflectorEntidades.ObtieneEntidad(typeof(EntidadPlantel));
        var Elementos = Enumerable.Empty<EntidadPlantel>().AsQueryable();
        _entidadEscuela = _dbSetEntidadEscuela.FirstOrDefault(_ => _.Id == Guid.Parse(parametros["n0Id"]));
        if (_entidadEscuela != null)
        {
            if (consulta.Filtros.Count > 0)
            {
                var predicateBody = interpreteConsulta.CrearConsultaExpresion<EntidadPlantel>(consulta, entidad);
                if (predicateBody != null)
                {
                    var RConsulta = _entidadEscuela.Planteles.AsQueryable().Provider.CreateQuery<EntidadPlantel>(predicateBody.getWhereExpression(_entidadEscuela.Planteles.AsQueryable().Expression));
                    Elementos = RConsulta.OrdenarPor(consulta.Paginado.ColumnaOrdenamiento ?? "Id", consulta.Paginado.Ordenamiento ?? Ordenamiento.asc);
                }
            }
            else
            {
                var RConsulta = _entidadEscuela.Planteles.AsQueryable();
                Elementos = RConsulta.OrdenarPor(consulta.Paginado.ColumnaOrdenamiento ?? "Id", consulta.Paginado.Ordenamiento ?? Ordenamiento.asc);
            }
        }
        return Elementos.Paginado(consulta);
    }

    #endregion
}
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
#pragma warning restore CS8603 // Possible null reference return