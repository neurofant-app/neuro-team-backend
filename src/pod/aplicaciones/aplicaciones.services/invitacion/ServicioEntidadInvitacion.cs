#pragma warning disable CS8603 // Possible null reference return.
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
using api.comunicaciones;
using extensibilidad.metadatos;
using apigenerica.model.modelos;
using apigenerica.model.reflectores;
using apigenerica.model.servicios;
using aplicaciones.model;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using comunes.primitivas;
using aplicaciones.services.dbcontext;
using comunes.primitivas.configuracion.mongo;
using MongoDB.Driver;
using Microsoft.EntityFrameworkCore;
using System.Text;
using comunicaciones.modelo;
using Microsoft.Extensions.Configuration;
using aplicaciones.services.extensiones;
using aplicaciones.services.proxy.abstractions;
using System.Data.Common;
using comunes.primitivas.atributos;
using System.Runtime.CompilerServices;
using System.Collections.Specialized;


namespace aplicaciones.services.invitacion;
[ServicioEntidadAPI(entidad: typeof(EntidadInvitacion))]
public class ServicioEntidadInvitacion : ServicioEntidadGenericaBase<EntidadInvitacion, CreaInvitacion, ActualizaInvitacion, ConsultaInvitacion, string>,
    IServicioEntidadAPI, IServicioEntidadInvitacion
{
    private readonly ILogger _logger;

    private readonly IReflectorEntidadesAPI reflector;
    private readonly IConfiguration configuration;
    private readonly IProxyComunicacionesServices _proxyComunicacionesServices;

    public ServicioEntidadInvitacion(ILogger<ServicioEntidadInvitacion> logger,
        IServicionConfiguracionMongo configuracionMongo,
        IReflectorEntidadesAPI Reflector, IDistributedCache cache, IConfiguration configuration, IProxyComunicacionesServices proxyComunicacionesServices) : base(null, null, logger, Reflector, cache)
    {
        _logger = logger;
        reflector = Reflector;
        this.configuration = configuration;
        _proxyComunicacionesServices = proxyComunicacionesServices;
        var configuracionEntidad = configuracionMongo.ConexionEntidad(MongoDbContextAplicaciones.NOMBRE_COLECCION_INVITACION);
        if (configuracionEntidad == null)
        {
            string err = $"No existe configuracion de mongo para '{MongoDbContextAplicaciones.NOMBRE_COLECCION_INVITACION}'";
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
            _dbSetFull = ((MongoDbContextAplicaciones)_db).Invitaciones;

        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error al inicializar mongo para '{MongoDbContextAplicaciones.NOMBRE_COLECCION_INVITACION}'");
            throw;
        }
    }
    private MongoDbContextAplicaciones DB { get { return (MongoDbContextAplicaciones)_db; } }
    public bool RequiereAutenticacion => true;
    public Entidad EntidadRepoAPI()
    {
        _logger.LogDebug("ServicioEntidadInvitacion-EntidadRepoAPI");
        return this.EntidadRepo();
    }
    public Entidad EntidadInsertAPI()
    {
        _logger.LogDebug("ServicioEntidadInvitacion-EntidadInsertAPI");
        return this.EntidadInsert();
    }
    public Entidad EntidadUpdateAPI()
    {
        _logger.LogDebug("ServicioEntidadInvitacion-EntidadUpdateAPI");
        return this.EntidadUpdate();
    }
    public Entidad EntidadDespliegueAPI()
    {
        _logger.LogDebug("ServicioEntidadInvitacion-EntidadDespliegueAPI");
        return this.EntidadDespliegue();
    }

    public void EstableceContextoUsuarioAPI(ContextoUsuario contexto)
    {
        _logger.LogDebug("ServicioEntidadInvitacion-EstableceContextoUsuarioAPI");
        this.EstableceContextoUsuario(contexto);
    }

    public ContextoUsuario? ObtieneContextoUsuarioAPI()
    {
        _logger.LogDebug("ServicioEntidadInvitacion-ObtieneContextoUsuarioAPI");
        return this._contextoUsuario;
    }

    private bool permisosValidos(string appId, [CallerMemberName] string metodoId = null)
    {
        var metodoActual = _contextoUsuario.AtributosMetodos.FirstOrDefault(_ => _.MetodoId == metodoId);

        if (metodoActual == null) { return false; }
        foreach (var id in metodoActual.atributosId)
        {
            if (!_contextoUsuario.RolesAplicacion.Contains(id) && !_contextoUsuario.PermisosAplicacion.Contains(id)) return false;
        }
        return true;
    }

    [Rol("00000000-0000-0000-0000-000000000001", "app-manager-rol-admin")]
    [Permiso("00000000-0000-0000-0000-000000000001", "app-manager-perm-admin")]
    public async Task<RespuestaPayload<object>> InsertarAPI(JsonElement data, StringDictionary? parametros = null)
    {
        _logger.LogDebug("ServicioEntidadInvitacion-InsertarAPI-{data}",data);
        if (!permisosValidos("00000000-0000-0000-0000-000000000001"))
        {
            RespuestaPayload<object> respuestaPayload = new RespuestaPayload<object>()
            { HttpCode = HttpCode.FORBIDDEN, Error = new() { Codigo = CodigosError.ENTIDADINVITACION_ACCION_PROHIBIDA_NO_PERMISOS, Mensaje = "No se tiene el permiso acccion prohibida", HttpCode = HttpCode.FORBIDDEN } };
            _logger.LogDebug("ServicioEntidadInvitacion-InsertarAPI resultado {ok} {code} {error}", respuestaPayload!.Ok, respuestaPayload!.HttpCode, respuestaPayload.Error);
            return respuestaPayload;
        }
        var add = data.Deserialize<CreaInvitacion>(JsonAPIDefaults());
        var temp = await this.Insertar(add);
        RespuestaPayload<object> respuesta = JsonSerializer.Deserialize<RespuestaPayload<object>>(JsonSerializer.Serialize(temp));
        _logger.LogDebug("ServicioEntidadInvitacion-InsertarAPI resultado {ok} {code} {error}", respuesta.Ok, respuesta!.HttpCode, respuesta.Error);
        return respuesta;
    }

    public async Task<Respuesta> ActualizarAPI(object id, JsonElement data, StringDictionary? parametros = null)
    {
        _logger.LogDebug("ServicioEntidadInvitacion-ActualizarAPI-{data}", data);
        var update = data.Deserialize<ActualizaInvitacion>(JsonAPIDefaults());
        Respuesta respuesta = await this.Actualizar((string)id, update);
        _logger.LogDebug("ServicioEntidadInvitacion-ActualizarAPI resultado {ok} {code} {error}", respuesta!.Ok, respuesta!.HttpCode, respuesta.Error);
        return respuesta;
    }

    public async Task<Respuesta> EliminarAPI(object id, StringDictionary? parametros = null)
    {
        _logger.LogDebug("ServicioEntidadInvitacion-EliminarAPI");
        Respuesta respuesta = await this.Eliminar((string)id);
        _logger.LogDebug("ServicioEntidadInvitacion-EliminarAPI resultado {ok} {code} {error}", respuesta!.Ok, respuesta!.HttpCode, respuesta.Error);
        return respuesta;
    }

    public async Task<RespuestaPayload<object>> UnicaPorIdAPI(object id, StringDictionary? parametros = null)
    {
        _logger.LogDebug("ServicioEntidadInvitacion-UnicaPorIdAPI");
        var temp = await this.UnicaPorId((string)id);
        RespuestaPayload<object> respuesta = JsonSerializer.Deserialize<RespuestaPayload<object>>(JsonSerializer.Serialize(temp));
        _logger.LogDebug("ServicioEntidadInvitacion-UnicaPorIdAPI resultado {ok} {code} {error}", respuesta!.Ok, respuesta!.HttpCode, respuesta.Error);
        return respuesta;
    }

    public async Task<RespuestaPayload<object>> UnicaPorIdDespliegueAPI(object id, StringDictionary? parametros = null)
    {
        _logger.LogDebug("ServicioEntidadInvitacion-UnicaPorIdDespliegueAPI");
        var temp = await this.UnicaPorIdDespliegue((string)id);
        RespuestaPayload<object> respuesta = JsonSerializer.Deserialize<RespuestaPayload<object>>(JsonSerializer.Serialize(temp));
        _logger.LogDebug("ServicioEntidadInvitacion-UnicaPorIdDespliegueAPI resultado {ok} {code} {error}", respuesta!.Ok, respuesta!.HttpCode, respuesta.Error);
        return respuesta;
    }

    public async Task<RespuestaPayload<PaginaGenerica<object>>> PaginaAPI(Consulta consulta, StringDictionary? parametros = null)
    {
        _logger.LogDebug("ServicioEntidadInvitacion-PaginaAPI-{consulta}", consulta);
        var temp = await this.Pagina(consulta);
        RespuestaPayload<PaginaGenerica<object>> respuesta = JsonSerializer.Deserialize<RespuestaPayload<PaginaGenerica<object>>>(JsonSerializer.Serialize(temp));
        _logger.LogDebug("ServicioEntidadInvitacion-PaginaAPI resultado {ok} {code} {error}", respuesta!.Ok, respuesta!.HttpCode, respuesta.Error);
        return respuesta;
    }

    public async Task<RespuestaPayload<PaginaGenerica<object>>> PaginaDespliegueAPI(Consulta consulta, StringDictionary? parametros = null)
    {
        _logger.LogDebug("ServicioEntidadInvitacion-PaginaDespliegueAPI-{consulta}", consulta);
        var temp = await this.PaginaDespliegue(consulta);
        RespuestaPayload<PaginaGenerica<object>> respuesta = JsonSerializer.Deserialize<RespuestaPayload<PaginaGenerica<object>>>(JsonSerializer.Serialize(temp));
        _logger.LogDebug("ServicioEntidadInvitacion-PaginaDespliegueAPI resultado {ok} {code} {error}", respuesta!.Ok, respuesta!.HttpCode, respuesta.Error);
        return respuesta;
    }

    #region Overrides para la personalización de la entidad Invitacion
    public override async Task<ResultadoValidacion> ValidarInsertar(CreaInvitacion data)
    {
        ResultadoValidacion resultado = new();
        resultado.Valido = true;

        return resultado;
    }
    public override async Task<ResultadoValidacion> ValidarEliminacion(string id, EntidadInvitacion original)
    {
        ResultadoValidacion resultado = new();
        resultado.Valido = true;
        return resultado;
    }

    public override async Task<ResultadoValidacion> ValidarActualizar(string id, ActualizaInvitacion actualizacion, EntidadInvitacion original)
    {
        ResultadoValidacion resultado = new();

        resultado.Valido = true;

        return resultado;
    }

    public override EntidadInvitacion ADTOFull(ActualizaInvitacion actualizacion, EntidadInvitacion actual)
    {
        actual.Id = actualizacion.Id;
        actual.AplicacionId = actualizacion.AplicacionId;
        return actual;
    }

    public override EntidadInvitacion ADTOFull(CreaInvitacion data)
    {
        EntidadInvitacion inv = new EntidadInvitacion()
        {
            Id = Guid.NewGuid(),
            AplicacionId = data.AplicacionId,
            Email = data.Email,
            RolId = data.RolId,
            Nombre = data.Nombre,
            Tipo = data.Tipo,
            Token = data.Token
        };
        return inv;
    }

    public override async Task<Respuesta> Actualizar(string id, ActualizaInvitacion data, StringDictionary? parametros = null)
    {
        var respuesta = new Respuesta();
        try
        {
            if (string.IsNullOrEmpty(id.ToString()) || data == null)
            {
                respuesta.Error = new ErrorProceso()
                {
                    Codigo = CodigosError.ENTIDADINVITACION_ID_PAYLOAD_NO_INGRESADO,
                    Mensaje = "No ha sido proporcionado el Id ó Payload",
                    HttpCode = HttpCode.BadRequest
                };
                respuesta.HttpCode = HttpCode.BadRequest;
                return respuesta;
            }


            EntidadInvitacion actual = _dbSetFull.Find(Guid.Parse(id));

            if (actual == null)
            {
                respuesta.Error = new ErrorProceso()
                {
                    Codigo = CodigosError.ENTIDADINVITACION_NO_ENCONTRADO,
                    Mensaje = "No se encontro una EntidadInvitacion con el ID proporcionado",
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
            _logger.LogError(ex, "ServicioEntidadInvitacion-Actualizar {msg}", ex.Message);
            respuesta.Error = new ErrorProceso() { Codigo = CodigosError.APPLICACION_ERROR_DESCONOCIDO, HttpCode = HttpCode.ServerError, Mensaje = ex.Message };
            respuesta.HttpCode = HttpCode.ServerError;
        }

        return respuesta;
    }


    public override async Task<RespuestaPayload<EntidadInvitacion>> UnicaPorId(string id, StringDictionary? parametros = null)
    {
        var respuesta = new RespuestaPayload<EntidadInvitacion>();
        try
        {
            EntidadInvitacion actual = await _dbSetFull.FindAsync(Guid.Parse(id));
            if (actual == null)
            {
                respuesta.Error = new ErrorProceso()
                {
                    Codigo = CodigosError.ENTIDADINVITACION_NO_ENCONTRADO,
                    Mensaje = "No existe una ENTIDADINVITACION con el Id proporcionado",
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
            _logger.LogError(ex, "ServicioEntidadInvitacion-UnicaPorId {msg}", ex.Message);
            respuesta.Error = new ErrorProceso() { Codigo = CodigosError.APPLICACION_ERROR_DESCONOCIDO, HttpCode = HttpCode.ServerError, Mensaje = ex.Message };
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
                    Codigo = CodigosError.ENTIDADINVITACION_ID_NO_INGRESADO,
                    Mensaje = "No ha sido proporcionado el Id",
                    HttpCode = HttpCode.BadRequest
                };
                respuesta.HttpCode = HttpCode.BadRequest;
                return respuesta;
            }

            EntidadInvitacion actual = _dbSetFull.Find(Guid.Parse(id));
            if (actual == null)
            {
                respuesta.Error = new ErrorProceso()
                {
                    Codigo = CodigosError.ENTIDADINVITACION_NO_ENCONTRADO,
                    Mensaje = "No existe una ENTIDADINVITACION con el Id proporcionado",
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
            _logger.LogError(ex, "ServicioEntidadInvitacion-Eliminar {msg}", ex.Message);
            respuesta.Error = new ErrorProceso() { Codigo = CodigosError.APPLICACION_ERROR_DESCONOCIDO, HttpCode = HttpCode.ServerError, Mensaje = ex.Message };
            respuesta.HttpCode = HttpCode.ServerError;
        }
        return respuesta;
    }
    public override ConsultaInvitacion ADTODespliegue(EntidadInvitacion data)
    {
        ConsultaInvitacion invitacionDesplegar = new ConsultaInvitacion()
        {
            Id = data.Id,
            AplicacionId = data.AplicacionId,
            Fecha = data.Fecha,
            Estado = data.Estado,
            Email = data.Email,
            RolId = data.RolId,
            Tipo = data.Tipo,
            Token = data.Token,

        };
        return invitacionDesplegar;
    }

    public override async Task<RespuestaPayload<ConsultaInvitacion>> Insertar(CreaInvitacion data, StringDictionary? parametros = null)
    {
        var respuesta = new RespuestaPayload<ConsultaInvitacion>();

        try
        {
            var resultadoValidacion = await ValidarInsertar(data);
            if (resultadoValidacion.Valido)
            {
                var entidad = ADTOFull(data);
                //para determinar el tipo de plantilla que se enviará, invitación, recuperación contraseña, etc.
                var tipoPlantillaContenido = TipoCOntenidoPlantilla(data.Tipo);
                var logoTipos = await DB.LogoAplicaciones.ToListAsync();
                var logoAp = logoTipos.FirstOrDefault(x => x.AplicacionId == data.AplicacionId);
                EntidadPlantillaInvitacion plantillaInvitacion = await DB.PlantillaInvitaciones.Where(x => x.AplicacionId == data.AplicacionId && x.TipoContenido == tipoPlantillaContenido).FirstOrDefaultAsync();
                
                byte[] bytes = Convert.FromBase64String(plantillaInvitacion.Plantilla);
                string html = Encoding.UTF8.GetString(bytes);
                DatosPlantillaRegistro datosPlantilla = new DatosPlantillaRegistro()
                {
                    Activacion = entidad.Id.ToString(),
                    FechaLimite = entidad.Fecha.ToString(),
                    Nombre = entidad.Nombre,
                    Email = entidad.Email,
                    UrlBase = configuration.LeeUrlBase(),
                    Logo64 = logoAp.LogoURLBase64,
                    Remitente = entidad.Nombre,
                };


                api.comunicaciones.MensajeEmail m = new()
                {
                    NombreDe = null,
                    DireccionDe = null,
                    DireccionPara = entidad.Email,
                    NombrePara = entidad.Nombre,
                    PlantillaTema = configuration.LeeTemaRegistro(),
                    PlantillaCuerpo = html,
                    JsonData = JsonSerializer.Serialize(datosPlantilla),
                };

                var respuestaCorreo = await _proxyComunicacionesServices.EnviarCorreo(m);

                if (respuestaCorreo.Ok)
                {
                    _dbSetFull.Add(entidad);
                    await _db.SaveChangesAsync();

                    respuesta.Ok = true;
                    respuesta.HttpCode = HttpCode.Ok;
                    respuesta.Payload = ADTODespliegue(entidad);
                }
                else
                {
                    respuesta.Error = resultadoValidacion.Error;
                    respuesta.Error!.Codigo = CodigosError.ENTIDADINVITACION_ENVIO_EMAIL_FALLIDO;
                    respuesta.HttpCode = resultadoValidacion.Error?.HttpCode ?? HttpCode.None;
                }

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
            _logger.LogError(ex, "ServicioEntidadInvitacion-Insertar {msg}", ex.Message);
            respuesta.Error = new ErrorProceso() { Codigo = CodigosError.APPLICACION_ERROR_DESCONOCIDO, HttpCode = HttpCode.ServerError, Mensaje = ex.Message };
            respuesta.HttpCode = HttpCode.ServerError;
        }

        return respuesta;
    }

    private TipoContenido TipoCOntenidoPlantilla(TipoComunicacion tipo)
    {
        switch (tipo)
        {
            case TipoComunicacion.Registro:
                return TipoContenido.Invitacion;
            case TipoComunicacion.RecuperacionContrasena:
                return TipoContenido.RecuperacionPassword;
            default:
                return TipoContenido.Invitacion;
        }
    }

    #endregion


}
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
#pragma warning restore CS8603 // Possible null reference return.