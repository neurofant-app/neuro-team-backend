#pragma warning disable CS8603 // Possible null reference return.
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.

using apigenerica.model.interpretes;
using apigenerica.model.modelos;
using apigenerica.model.reflectores;
using apigenerica.model.servicios;
using comunes.primitivas;
using comunes.primitivas.configuracion.mongo;
using extensibilidad.metadatos;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using organizacion.model;
using organizacion.model.dominio;
using organizacion.services.dbcontext;
using System.Collections.Specialized;
using System.Text.Json;

namespace organizacion.services.dominio;
[ServicioEntidadAPI(typeof(Dominio))]
public class ServicioDominio : ServicioEntidadGenericaBase<Dominio, DominioInsertar, DominioActualizar, DominioDespliegue, Guid>,
    IServicioEntidadAPI, IServicioDominio
{
    private readonly ILogger<ServicioDominio> _logger;
    private readonly IReflectorEntidadesAPI _reflector;
    private readonly IDistributedCache _cache;
    public ServicioDominio(ILogger<ServicioDominio> logger, IServicionConfiguracionMongo configuracionMongo, IReflectorEntidadesAPI reflector,
        IDistributedCache cache) : base(null, null, logger, reflector, cache) {

        _logger = logger;
        _reflector = reflector;
        _cache = cache;
        interpreteConsulta = new InterpreteConsultaExpresiones();


        var configuracionEntidad = configuracionMongo.ConexionEntidad(MongoDbContextOrganizacion.NOMBRE_COLECCION_DOMINIOS);

        if (configuracionEntidad == null)
        {
            string err = $"No existe configuracion de mongo para '{MongoDbContextOrganizacion.NOMBRE_COLECCION_DOMINIOS}'";
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

            _db = MongoDbContextOrganizacion.Create(client.GetDatabase(configuracionEntidad.Esquema));
            _dbSetFull = ((MongoDbContextOrganizacion)_db).Dominios;

        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error al inicializar mongo para '{MongoDbContextOrganizacion.NOMBRE_COLECCION_DOMINIOS}'");
            throw;
        }
    }

    public bool RequiereAutenticacion => true;

    private MongoDbContextOrganizacion DB { get { return (MongoDbContextOrganizacion)_db; } }

    public async Task<Respuesta> ActualizarAPI(object id, JsonElement data, StringDictionary? parametros = null)
    {
        _logger.LogDebug("ServicioDominio-ActualizarAPI-{data}", data);
        var update = data.Deserialize<DominioActualizar>(JsonAPIDefaults());
        Respuesta respuesta = await this.Actualizar(Guid.Parse((string)id), update, parametros);
        _logger.LogDebug("ServicioDominio-ActualizarAPI resultado {ok} {code} {error}", respuesta!.Ok, respuesta!.HttpCode, respuesta.Error);
        return respuesta;
    }

    public async Task<Respuesta> EliminarAPI(object id, StringDictionary? parametros = null, bool forzarEliminacion = false)
    {
        _logger.LogDebug("ServicioDominio-EliminarAPI");
        Respuesta respuesta = await this.Eliminar(Guid.Parse((string)id), parametros, forzarEliminacion);
        _logger.LogDebug("ServicioDominio-EliminarAPI resultado {ok} {code} {error}", respuesta!.Ok, respuesta!.HttpCode, respuesta.Error);
        return respuesta;
    }

    public async Task<RespuestaPayload<object>> InsertarAPI(JsonElement data, StringDictionary? parametros = null)
    {
        _logger.LogDebug("ServicioDominio-InsertarAPI-{data}", data);
        var add = data.Deserialize<DominioInsertar>(JsonAPIDefaults());
        var temp = await this.Insertar(add, parametros);
        RespuestaPayload<object> respuesta = JsonSerializer.Deserialize<RespuestaPayload<object>>(JsonSerializer.Serialize(temp));
        _logger.LogDebug("ServicioDominio-InsertarAPI resultado {ok} {code} {error}", respuesta!.Ok, respuesta!.HttpCode, respuesta.Error);
        return respuesta;
    }

    public async Task<RespuestaPayload<PaginaGenerica<object>>> PaginaAPI(Consulta consulta, StringDictionary? parametros = null)
    {
        _logger.LogDebug("ServicioDominio-PaginaAPI-{consulta}", consulta);
        var temp = await this.Pagina(consulta, parametros);
        RespuestaPayload<PaginaGenerica<object>> respuesta = JsonSerializer.Deserialize<RespuestaPayload<PaginaGenerica<object>>>(JsonSerializer.Serialize(temp));
        _logger.LogDebug("ServicioDominio-PaginaAPI resultado {ok} {code} {error}", respuesta!.Ok, respuesta!.HttpCode, respuesta.Error);
        return respuesta;
    }

    public async Task<RespuestaPayload<PaginaGenerica<object>>> PaginaDespliegueAPI(Consulta consulta, StringDictionary? parametros = null)
    {
        _logger.LogDebug("ServicioDominio-PaginaDespliegueAPI-{consulta}", consulta);
        var temp = await this.PaginaDespliegue(consulta, parametros);
        RespuestaPayload<PaginaGenerica<object>> respuesta = JsonSerializer.Deserialize<RespuestaPayload<PaginaGenerica<object>>>(JsonSerializer.Serialize(temp));
        _logger.LogDebug("ServicioDominio-PaginaDespliegueAPI resultado {ok} {code} {error}", respuesta!.Ok, respuesta!.HttpCode, respuesta.Error);
        return respuesta;
    }

    public async Task<RespuestaPayload<object>> UnicaPorIdAPI(object id, StringDictionary? parametros = null)
    {
        _logger.LogDebug("ServicioDominio-UnicaPorIdAPI");
        var temp = await this.UnicaPorId(Guid.Parse((string)id), parametros);
        RespuestaPayload<object> respuesta = JsonSerializer.Deserialize<RespuestaPayload<object>>(JsonSerializer.Serialize(temp));
        _logger.LogDebug("ServicioDominio-UnicaPorIdAPI resultado {ok} {code} {error}", respuesta!.Ok, respuesta!.HttpCode, respuesta.Error);
        return respuesta;
    }

    public async Task<RespuestaPayload<object>> UnicaPorIdDespliegueAPI(object id, StringDictionary? parametros = null)
    {
        _logger.LogDebug("ServicioDominio-UnicaPorIdDespliegueAPI");
        var temp = await this.UnicaPorIdDespliegue(Guid.Parse((string)id), parametros);
        RespuestaPayload<object> respuesta = JsonSerializer.Deserialize<RespuestaPayload<object>>(JsonSerializer.Serialize(temp));
        _logger.LogDebug("ServicioDominio-UnicaPorIdDespliegueAPI resultado {ok} {code} {error}", respuesta!.Ok, respuesta!.HttpCode, respuesta.Error);
        return respuesta;
    }

    public Entidad EntidadRepoAPI()
    {
        _logger.LogDebug("ServicioDominio-EntidadRepoAPI");
        return this.EntidadRepo();
    }

    public Entidad EntidadInsertAPI()
    {
        _logger.LogDebug("ServicioDominio-EntidadInsertAPI");
        return this.EntidadInsert();
    }

    public Entidad EntidadUpdateAPI()
    {
        _logger.LogDebug("ServicioDominio-EntidadUpdateAPI");
        return this.EntidadUpdate();
    }

    public Entidad EntidadDespliegueAPI()
    {
        _logger.LogDebug("ServicioDominio-DespliegueAPI");
        return this.EntidadDespliegue();
    }

    public void EstableceContextoUsuarioAPI(ContextoUsuario contexto)
    {
        _logger.LogDebug("ServicioDominio-ContextoUsuarioAPI");
        this.EstableceContextoUsuario(contexto);
    }

    public ContextoUsuario? ObtieneContextoUsuarioAPI()
    {
        _logger.LogDebug("ServicioDominio-ObtieneContextoUsuarioAPI");
        return this._contextoUsuario;
    }

    #region Overrides para la entidad Dominio
    public async Task<ResultadoValidacion> ValidarActualizar(Guid id, DominioActualizar actualizacion, Dominio original)
    {
        return new ResultadoValidacion() { Valido = true };
    }

    public async Task<ResultadoValidacion> ValidarEliminacion(Guid id, Dominio original, bool forzarEliminacion = false)
    {
        return new ResultadoValidacion() { Valido = true };
    }

    public async Task<ResultadoValidacion> ValidarInsertar(DominioInsertar data)
    {
        return new ResultadoValidacion() { Valido = true };
    }


    public override Dominio ADTOFull(DominioInsertar data)
    {
        return new Dominio()
        {
            Id = Guid.NewGuid(),
            Nombre = data.Nombre,
            OrigenId = Guid.Parse(_contextoUsuario.UsuarioId),
            TipoOrigen = TipoOrigenDominio.Usuario
        };
    }

    public override Dominio ADTOFull(DominioActualizar actualizacion, Dominio actual)
    {
        actual.Id = actualizacion.Id;
        actual.Nombre = actualizacion.Nombre;
        actual.Activo = actualizacion.Activo;
        return actual;
    }


    public override DominioDespliegue ADTODespliegue(Dominio data)
    {
        return new DominioDespliegue()
        {
            Id = data.Id,
            Nombre =  data.Nombre
        };
    }

    public async Task<Respuesta> Actualizar(Guid id, DominioActualizar data, StringDictionary? parametros = null)
    {
        var respuesta = new Respuesta();
        try
        {
            if(id == Guid.Empty || data == null)
            {
                respuesta.Error = new ErrorProceso()
                {
                    Codigo = CodigosError.ORGANIZACION_DOMINIO_ID_PAYLOAD_NO_INGRESADO,
                    Mensaje = "No ha sido proporcionado el Id ó Payload",
                    HttpCode = HttpCode.BadRequest,
                };
                respuesta.HttpCode = HttpCode.BadRequest;
                return respuesta;
            }

            Dominio actual = _dbSetFull.Find(id);

            if(actual == null)
            {
                respuesta.Error = new ErrorProceso()
                {
                    Codigo = CodigosError.ORGANIZACION_DOMINIO_NO_ENCONTRADA,
                    Mensaje = "No existe un Dominio con el Id proporcionado",
                    HttpCode = HttpCode.NotFound
                };
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
                respuesta.Error!.Codigo = CodigosError.ORGANIZACION_DATOS_NO_VALIDOS;
                respuesta.HttpCode = resultadoValidacion.Error?.HttpCode ?? HttpCode.None;
            }

        }
        catch(Exception ex)
        {
            _logger.LogError(ex, "ServicioDominio-Actualizar {msg}", ex.Message);
            respuesta.Error = new ErrorProceso() { Codigo = CodigosError.ORGANIZACION_ERROR_DESCONOCIDO, HttpCode = HttpCode.ServerError, Mensaje = ex.Message };
            respuesta.HttpCode = HttpCode.ServerError;
        }
        return respuesta;
    }

    public async Task<Respuesta> Eliminar(Guid id, StringDictionary? parametros = null, bool forzarEliminacion = false)
    {
        var respuesta = new Respuesta();
        try
        {
            if(id == Guid.Empty)
            {
                respuesta.Error = new()
                {
                    Codigo = CodigosError.ORGANIZACION_DOMINIO_ID_NO_INGRESADO,
                    Mensaje = "No ha sido proporciando el Id",
                    HttpCode = HttpCode.BadRequest,
                };
                return respuesta;
            }

            Dominio actual = _dbSetFull.Find(id);

            if(actual == null) 
            {
                respuesta.Error = new()
                {
                    Codigo = CodigosError.ORGANIZACION_DOMINIO_NO_ENCONTRADA,
                    Mensaje = "No existe un Dominio con el Id proporcionado",
                    HttpCode = HttpCode.NotFound,
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
                respuesta.Error!.Codigo = CodigosError.ORGANIZACION_DATOS_NO_VALIDOS;
                respuesta.HttpCode = resultadoValidacion.Error?.HttpCode ?? HttpCode.None;
            }

        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "ServicioDominio-Eliminar {msg}", ex.Message);
            respuesta.Error = new ErrorProceso() { Codigo = CodigosError.ORGANIZACION_ERROR_DESCONOCIDO, HttpCode = HttpCode.ServerError, Mensaje = ex.Message };
            respuesta.HttpCode = HttpCode.ServerError;
        }
        return respuesta;
    }

    public async Task<RespuestaPayload<Dominio>> UnicaPorId(Guid id, StringDictionary? parametros = null)
    {
        var respuesta = new RespuestaPayload<Dominio>();
        try
        {
            Dominio actual = await _dbSetFull.FindAsync(id);

            if(actual == null)
            {
                respuesta.Error = new ErrorProceso()
                {
                    Codigo = CodigosError.ORGANIZACION_DOMINIO_NO_ENCONTRADA,
                    Mensaje = "No existe una Dominio con el Id proporcionado",
                    HttpCode = HttpCode.NotFound
                };
                respuesta.HttpCode = HttpCode.NotFound;
                return respuesta;
            }

            respuesta.Ok = true;
            respuesta.HttpCode = HttpCode.Ok;
            respuesta.Payload = actual;
        }
        catch(Exception ex)
        {
            _logger.LogError(ex, "ServicioDominio-UnicaPorId {msg}", ex.Message);
            respuesta.Error = new ErrorProceso() { Codigo = CodigosError.ORGANIZACION_ERROR_DESCONOCIDO, HttpCode = HttpCode.ServerError, Mensaje = ex.Message };
            respuesta.HttpCode = HttpCode.ServerError;
        }
        return respuesta;
    }

    public async Task<RespuestaPayload<DominioDespliegue>> UnicaPorIdDespliegue(Guid id, StringDictionary? parametros = null)
    {
        RespuestaPayload<DominioDespliegue> respuesta = new RespuestaPayload<DominioDespliegue>();

        try
        {
            var resultado = await UnicaPorId(id, parametros);

            respuesta.Ok = resultado.Ok;

            if (resultado.Ok)
            {
                respuesta.Payload = ADTODespliegue((Dominio)resultado.Payload);
            }
            else
            {
                respuesta.Error = resultado.Error;
                respuesta.Error!.Codigo = CodigosError.ORGANIZACION_DATOS_NO_VALIDOS;
                respuesta.HttpCode = resultado.Error?.HttpCode ?? HttpCode.None;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "ServicioDominio-UnicaPorId {msg}", ex.Message);
            respuesta.Error = new ErrorProceso() { Codigo = CodigosError.ORGANIZACION_ERROR_DESCONOCIDO, HttpCode = HttpCode.ServerError, Mensaje = ex.Message };
            respuesta.HttpCode = HttpCode.ServerError;
        }
        return respuesta;
    }


    #endregion
}
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
#pragma warning restore CS8603 // Possible null reference return.
