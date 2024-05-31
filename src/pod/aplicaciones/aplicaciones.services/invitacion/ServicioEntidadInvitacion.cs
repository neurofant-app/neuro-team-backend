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


namespace aplicaciones.services.invitacion;
[ServicioEntidadAPI(entidad: typeof(EntidadInvitacion))]
public class ServicioEntidadInvitacion : ServicioEntidadGenericaBase<EntidadInvitacion, CreaInvitacion, ActualizaInvitacion, ConsultaInvitacion, string>,
    IServicioEntidadAPI, IServicioInvitacion
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
        return this.EntidadRepo();
    }
    public Entidad EntidadInsertAPI()
    {
        return this.EntidadInsert();
    }
    public Entidad EntidadUpdateAPI()
    {
        return this.EntidadUpdate();
    }
    public Entidad EntidadDespliegueAPI()
    {
        return this.EntidadDespliegue();
    }

    public void EstableceContextoUsuarioAPI(ContextoUsuario contexto)
    {
        this.EstableceContextoUsuario(contexto);
    }

    public ContextoUsuario? ObtieneContextoUsuarioAPI()
    {
        return this._contextoUsuario;
    }

    public async Task<RespuestaPayload<object>> InsertarAPI(JsonElement data)
    {
        var add = data.Deserialize<CreaInvitacion>(JsonAPIDefaults());
        var temp = await this.Insertar(add);
        RespuestaPayload<object> respuesta = JsonSerializer.Deserialize<RespuestaPayload<object>>(JsonSerializer.Serialize(temp));
        return respuesta;
    }

    public async Task<Respuesta> ActualizarAPI(object id, JsonElement data)
    {
        var update = data.Deserialize<ActualizaInvitacion>(JsonAPIDefaults());
        return await this.Actualizar((string)id, update);
    }

    public async Task<Respuesta> EliminarAPI(object id)
    {
        return await this.Eliminar((string)id);
    }

    public async Task<RespuestaPayload<object>> UnicaPorIdAPI(object id)
    {
        var temp = await this.UnicaPorId((string)id);
        RespuestaPayload<object> respuesta = JsonSerializer.Deserialize<RespuestaPayload<object>>(JsonSerializer.Serialize(temp));
        return respuesta;
    }

    public async Task<RespuestaPayload<object>> UnicaPorIdDespliegueAPI(object id)
    {
        var temp = await this.UnicaPorIdDespliegue((string)id);

        RespuestaPayload<object> respuesta = JsonSerializer.Deserialize<RespuestaPayload<object>>(JsonSerializer.Serialize(temp));
        return respuesta;
    }

    public async Task<RespuestaPayload<PaginaGenerica<object>>> PaginaAPI(Consulta consulta)
    {
        var temp = await this.Pagina(consulta);
        RespuestaPayload<PaginaGenerica<object>> respuesta = JsonSerializer.Deserialize<RespuestaPayload<PaginaGenerica<object>>>(JsonSerializer.Serialize(temp));

        return respuesta;
    }

    public async Task<RespuestaPayload<PaginaGenerica<object>>> PaginaDespliegueAPI(Consulta consulta)
    {
        var temp = await this.PaginaDespliegue(consulta);
        RespuestaPayload<PaginaGenerica<object>> respuesta = JsonSerializer.Deserialize<RespuestaPayload<PaginaGenerica<object>>>(JsonSerializer.Serialize(temp));
        return respuesta;
    }

    #region Overrides para la personalización de la entidad LogoAplicacion
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

    public override async Task<Respuesta> Actualizar(string id, ActualizaInvitacion data)
    {
        var respuesta = new Respuesta();
        try
        {
            if (string.IsNullOrEmpty(id.ToString()) || data == null)
            {
                respuesta.HttpCode = HttpCode.BadRequest;
                return respuesta;
            }


            EntidadInvitacion actual = _dbSetFull.Find(Guid.Parse(id));

            if (actual == null)
            {
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


    public override async Task<RespuestaPayload<EntidadInvitacion>> UnicaPorId(string id)
    {
        var respuesta = new RespuestaPayload<EntidadInvitacion>();
        try
        {
            EntidadInvitacion actual = await _dbSetFull.FindAsync(Guid.Parse(id));
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

    public override async Task<Respuesta> Eliminar(string id)
    {
        var respuesta = new Respuesta();
        try
        {

            if (string.IsNullOrEmpty(id))
            {
                respuesta.HttpCode = HttpCode.BadRequest;
                return respuesta;
            }

            EntidadInvitacion actual = _dbSetFull.Find(Guid.Parse(id));
            if (actual == null)
            {
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

        };
        return invitacionDesplegar;
    }

    public override async Task<RespuestaPayload<ConsultaInvitacion>> Insertar(CreaInvitacion data)
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
                EntidadPlantillaInvitacion plantillaInvitacion = await DB.PlantillaInvitaciones.Where(x => x.AplicacionId == data.AplicacionId).FirstOrDefaultAsync();
                //EntidadLogoAplicacion logoAplicacion = await DB.LogoAplicaciones|
                //            .Where(x => x.AplicacionId == data.AplicacionId)
                //            .FirstOrDefaultAsync();
                
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
                respuesta.HttpCode = resultadoValidacion.Error?.HttpCode ?? HttpCode.None;
            }
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