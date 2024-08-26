#pragma warning disable CS8603 // Possible null reference return.
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
using extensibilidad.metadatos;
using apigenerica.model.interpretes;
using apigenerica.model.modelos;
using apigenerica.model.reflectores;
using comunes.primitivas;
using apigenerica.model.servicios;
using aplicaciones.model;
using aplicaciones.services.consentimiento;
using aplicaciones.services.plantilla;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using comunes.primitivas.configuracion.mongo;
using aplicaciones.services.dbcontext;
using MongoDB.Driver;



namespace aplicaciones.services.logo;
[ServicioEntidadAPI(entidad:typeof(EntidadLogoAplicacion))]
public class ServicioLogoAplicacion : ServicioEntidadGenericaBase<EntidadLogoAplicacion,EntidadLogoAplicacion,EntidadLogoAplicacion,EntidadLogoAplicacion,string>,
    IServicioEntidadAPI, IServicioLogoAplicacion
{
    private readonly ILogger _logger;
    private readonly IReflectorEntidadesAPI reflector;

    public ServicioLogoAplicacion(ILogger<ServicioLogoAplicacion> logger,
        IServicionConfiguracionMongo configuracionMongo,
        IReflectorEntidadesAPI Reflector, IDistributedCache cache) : base(null, null, logger, Reflector, cache)
    {
        _logger = logger;
        reflector = Reflector;

        var configuracionEntidad = configuracionMongo.ConexionEntidad(MongoDbContextAplicaciones.NOMBRE_COLECCION_LOGOAPLICACION);
        if (configuracionEntidad == null)
        {
            string err = $"No existe configuración de mongo para '{MongoDbContextAplicaciones.NOMBRE_COLECCION_LOGOAPLICACION}'";
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

            _db = MongoDbContextAplicaciones.Create(client.GetDatabase(configuracionEntidad.Esquema));
            _dbSetFull = ((MongoDbContextAplicaciones)_db).LogoAplicaciones;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error al inicializar mongo para '{MongoDbContextAplicaciones.NOMBRE_COLECCION_LOGOAPLICACION}'");
            throw;
        }
    }
    private MongoDbContextAplicaciones DB { get { return (MongoDbContextAplicaciones)_db; } }
    public bool RequiereAutenticacion => true;
    public Entidad EntidadRepoAPI()
    {
        _logger.LogDebug("ServicioLogoAplicacion-EntidadRepoAPI");
        return this.EntidadRepo();
    }
    public Entidad EntidadInsertAPI()
    {
        _logger.LogDebug("ServicioLogoAplicacion-EntidadInsertAPI");
        return this.EntidadInsert();
    }
    public Entidad EntidadUpdateAPI()
    {
        _logger.LogDebug("ServicioLogoAplicacion-EntidadUpdateAPI");
        return this.EntidadUpdate();
    }
    public Entidad EntidadDespliegueAPI()
    {
        _logger.LogDebug("ServicioLogoAplicacion-EntidadDespliegueAPI");
        return this.EntidadDespliegue();
    }

    public void EstableceContextoUsuarioAPI(ContextoUsuario contexto)
    {
        _logger.LogDebug("ServicioLogoAplicacion-EstableceContextoUsuarioAPI");
        this.EstableceContextoUsuario(contexto);
    }

    public ContextoUsuario? ObtieneContextoUsuarioAPI()
    {
        _logger.LogDebug("ServicioLogoAplicacion-ObtieneContextoUsuarioAPI");
        return this._contextoUsuario;
    }

    public async Task<RespuestaPayload<object>> InsertarAPI(JsonElement data)
    {
        _logger.LogDebug("ServicioLogoAplicacion-InsertarAPI-{data}", data);
        var add = data.Deserialize<EntidadLogoAplicacion>(JsonAPIDefaults());
        var temp = await this.Insertar(add);
        RespuestaPayload<object> respuesta = JsonSerializer.Deserialize<RespuestaPayload<object>>(JsonSerializer.Serialize(temp));
        _logger.LogDebug("ServicioLogoAplicacion-InsertarAPI resultado {ok} {code} {error}", respuesta!.Ok, respuesta!.HttpCode, respuesta.Error);
        return respuesta;
    }

    public async Task<Respuesta> ActualizarAPI(object id, JsonElement data)
    {
        _logger.LogDebug("ServicioLogoAplicacion-ActualizarAPI-{data}", data);
        var update = data.Deserialize<EntidadLogoAplicacion>(JsonAPIDefaults());
        Respuesta respuesta = await this.Actualizar((string)id, update);
        _logger.LogDebug("ServicioLogoAplicacion-ActualizarAPI resultado {ok} {code} {error}", respuesta!.Ok, respuesta!.HttpCode, respuesta.Error);
        return respuesta;
    }

    public async Task<Respuesta> EliminarAPI(object id)
    {
        _logger.LogDebug("ServicioLogoAplicacion-EliminarAPI");
        Respuesta respuesta = await this.Eliminar((string)id);
        _logger.LogDebug("ServicioLogoAplicacion-EliminarAPI resultado {ok} {code} {error}", respuesta!.Ok, respuesta!.HttpCode, respuesta.Error);
        return respuesta;
    }

    public async Task<RespuestaPayload<object>> UnicaPorIdAPI(object id)
    {
        _logger.LogDebug("ServicioLogoAplicacion-UnicaPorIdAPI");
        var temp = await this.UnicaPorId((string)id);
        RespuestaPayload<object> respuesta = JsonSerializer.Deserialize<RespuestaPayload<object>>(JsonSerializer.Serialize(temp));
        _logger.LogDebug("ServicioLogoAplicacion-UnicaPorIdAPI resultado {ok} {code} {error}", respuesta!.Ok, respuesta!.HttpCode, respuesta.Error);
        return respuesta;
    }

    public async Task<RespuestaPayload<object>> UnicaPorIdDespliegueAPI(object id)
    {
        _logger.LogDebug("ServicioLogoAplicacion-UnicaPorIdDespliegueAPI");
        var temp = await this.UnicaPorIdDespliegue((string)id);
        RespuestaPayload<object> respuesta = JsonSerializer.Deserialize<RespuestaPayload<object>>(JsonSerializer.Serialize(temp));
        _logger.LogDebug("ServicioLogoAplicacion-UnicaPorIdDespliegueAPI resultado {ok} {code} {error}", respuesta!.Ok, respuesta!.HttpCode, respuesta.Error);
        return respuesta;
    }

    public async Task<RespuestaPayload<PaginaGenerica<object>>> PaginaAPI(Consulta consulta)
    {
        _logger.LogDebug("ServicioLogoAplicacion-PaginaAPI-{consulta}", consulta);
        var temp = await this.Pagina(consulta);
        RespuestaPayload<PaginaGenerica<object>> respuesta = JsonSerializer.Deserialize<RespuestaPayload<PaginaGenerica<object>>>(JsonSerializer.Serialize(temp));
        _logger.LogDebug("ServicioLogoAplicacion-PaginaAPI resultado {ok} {code} {error}", respuesta!.Ok, respuesta!.HttpCode, respuesta.Error);
        return respuesta;
    }

    public async Task<RespuestaPayload<PaginaGenerica<object>>> PaginaDespliegueAPI(Consulta consulta)
    {
        _logger.LogDebug("ServicioLogoAplicacion-PaginaDespliegueAPI-{consulta}", consulta);
        var temp = await this.PaginaDespliegue(consulta);
        RespuestaPayload<PaginaGenerica<object>> respuesta = JsonSerializer.Deserialize<RespuestaPayload<PaginaGenerica<object>>>(JsonSerializer.Serialize(temp));
        _logger.LogDebug("ServicioLogoAplicacion-PaginaDespliegueAPI resultado {ok} {code} {error}", respuesta!.Ok, respuesta!.HttpCode, respuesta.Error);
        return respuesta;
    }

    #region Overrides para la personalización de la entidad LogoAplicacion
    public override async Task<ResultadoValidacion> ValidarInsertar(EntidadLogoAplicacion data)
    {
        ResultadoValidacion resultado = new();
        resultado.Valido = true;

        return resultado;
    }
    public override async Task<ResultadoValidacion> ValidarEliminacion(string id, EntidadLogoAplicacion original)
    {
        ResultadoValidacion resultado = new();
        resultado.Valido = true;
        return resultado;
    }

    public override async Task<ResultadoValidacion> ValidarActualizar(string id, EntidadLogoAplicacion actualizacion, EntidadLogoAplicacion original)
    {
        ResultadoValidacion resultado = new();

        resultado.Valido = true;

        return resultado;
    }

    public override EntidadLogoAplicacion ADTOFull(EntidadLogoAplicacion actualizacion, EntidadLogoAplicacion actual)
    {
        actual.Tipo = actualizacion.Tipo;
        actual.Idioma = actualizacion.Idioma;
        actual.IdiomaDefault = actualizacion.IdiomaDefault;
        actual.LogoURLBase64 = "data:image/jpeg;base64,"+actualizacion.LogoURLBase64;
        actual.EsSVG = actualizacion.EsSVG;
        actual.EsUrl = actualizacion.EsUrl;
        return actual;
    }

    public override EntidadLogoAplicacion ADTOFull(EntidadLogoAplicacion data)
    {
        EntidadLogoAplicacion logoAplicacion = new EntidadLogoAplicacion()
        {
            Id = Guid.NewGuid(),
            AplicacionId = data.AplicacionId,
            Tipo = data.Tipo,
            Idioma = data.Idioma,
            IdiomaDefault = data.IdiomaDefault,
            LogoURLBase64 = "data:image/jpeg;base64,"+data.LogoURLBase64,
            EsSVG = data.EsSVG,
            EsUrl = data.EsUrl
        };
        return logoAplicacion;
    }

    public override EntidadLogoAplicacion ADTODespliegue(EntidadLogoAplicacion data)
    {
        EntidadLogoAplicacion logoAplicacion = new EntidadLogoAplicacion()
        {
            Id = data.Id,
            AplicacionId = data.AplicacionId,
            Tipo = data.Tipo,
            Idioma = data.Idioma,
            IdiomaDefault = data.IdiomaDefault,
            LogoURLBase64 = data.LogoURLBase64,
            EsSVG = data.EsSVG,
            EsUrl = data.EsUrl

        };
        return logoAplicacion;
    }


    public override async Task<Respuesta> Actualizar(string id, EntidadLogoAplicacion data)
    {
        var respuesta = new Respuesta();
        try
        {
            if (string.IsNullOrEmpty(id.ToString()) || data == null)
            {
                respuesta.Error = new ErrorProceso()
                {
                    Codigo = CodigosError.LOGOAPLICACION_ID_PAYLOAD_NO_INGRESADO,
                    Mensaje = "No ha sido proporcionado el Id ó Payload",
                    HttpCode = HttpCode.BadRequest
                };
                respuesta.HttpCode = HttpCode.BadRequest;
                return respuesta;
            }


            EntidadLogoAplicacion actual = _dbSetFull.Find(Guid.Parse(id));

            if (actual == null)
            {
                respuesta.Error = new ErrorProceso()
                {
                    Codigo = CodigosError.LOGOAPLICACION_NO_ENCONTRADO,
                    Mensaje = "No existe una LOGOAPLICACION con el Id proporcionado",
                    HttpCode = HttpCode.NotFound
                };
                respuesta.HttpCode = HttpCode.NotFound;
                return respuesta;
            }

            var resultadoValidacion = await ValidarActualizar(id.ToString(), data, actual);
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
                respuesta.Error!.Codigo = CodigosError.APPLICACION_DATOS_NO_VALIDOS;
                respuesta.HttpCode = resultadoValidacion.Error?.HttpCode ?? HttpCode.None;
            }

        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "ServicioLogoAplicacion-Actualizar {msg}", ex.Message);
            respuesta.Error = new ErrorProceso() { Codigo = CodigosError.APPLICACION_ERROR_DESCONOCIDO, HttpCode = HttpCode.ServerError, Mensaje = ex.Message };
            respuesta.HttpCode = HttpCode.ServerError;
        }

        return respuesta;
    }


    public override async Task<RespuestaPayload<EntidadLogoAplicacion>> UnicaPorId(string id)
    {
        var respuesta = new RespuestaPayload<EntidadLogoAplicacion>();
        try
        {
            EntidadLogoAplicacion actual = await _dbSetFull.FindAsync(Guid.Parse(id));
            if (actual == null)
            {
                respuesta.Error = new ErrorProceso()
                {
                    Codigo = CodigosError.APPLICACION_NO_ENCONTRADA,
                    Mensaje = "No existe una LogoAplicacion con el Id proporcionado",
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
            _logger.LogError(ex, "ServicioLogoAplicacion-UnicaPorId {msg}", ex.Message);
            respuesta.Error = new ErrorProceso() { Codigo = CodigosError.APPLICACION_ERROR_DESCONOCIDO, HttpCode = HttpCode.ServerError, Mensaje = ex.Message };
            respuesta.HttpCode = HttpCode.ServerError;
        }
        return respuesta;
    }

    public override async Task<Respuesta> Eliminar(string id)
    {
        var respuesta = new Respuesta();
        try
        {
            if (string.IsNullOrEmpty(id))
            {
                respuesta.Error = new ErrorProceso()
                {
                    Codigo = CodigosError.LOGOAPLICACION_ID_NO_INGRESADO,
                    Mensaje = "No ha sido proporcionado el Id",
                    HttpCode = HttpCode.BadRequest
                };
                respuesta.HttpCode = HttpCode.BadRequest;
                return respuesta;
            }

            EntidadLogoAplicacion actual = _dbSetFull.Find(Guid.Parse(id));
            if (actual == null)
            {
                respuesta.Error = new ErrorProceso()
                {
                    Codigo = CodigosError.LOGOAPLICACION_NO_ENCONTRADO,
                    Mensaje = "No existe una LOGOAPLICACION con el Id proporcionado",
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
                respuesta.Error!.Codigo = CodigosError.APPLICACION_DATOS_NO_VALIDOS;
                respuesta.HttpCode = resultadoValidacion.Error?.HttpCode ?? HttpCode.None;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "ServicioLogoAplicacion-Eliminar {msg}", ex.Message);
            respuesta.Error = new ErrorProceso() { Codigo = CodigosError.APPLICACION_ERROR_DESCONOCIDO, HttpCode = HttpCode.ServerError, Mensaje = ex.Message };
            respuesta.HttpCode = HttpCode.ServerError;
        }
        return respuesta;
    }

    #endregion
}
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
#pragma warning restore CS8603 // Possible null reference return.
