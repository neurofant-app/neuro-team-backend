#pragma warning disable CS8603 // Possible null reference return.
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
using apigenerica.model.modelos;
using apigenerica.model.reflectores;
using apigenerica.model.servicios;
using comunes.primitivas;
using comunes.primitivas.configuracion.mongo;
using conversaciones.model;
using conversaciones.services.dbcontext;
using extensibilidad.metadatos;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using System.Text.Json;
using Plantilla = conversaciones.model.Plantilla;

namespace conversaciones.services.plantilla;
[ServicioEntidadAPI(entidad:typeof(Plantilla))]
public class ServicioPlantilla : ServicioEntidadGenericaBase<Plantilla, Plantilla, Plantilla, Plantilla, string>,
    IServicioEntidadAPI, IServicioPlantilla
{
    private readonly ILogger _logger;
    private readonly IReflectorEntidadesAPI reflector;
    private readonly IDistributedCache cache;

    public ServicioPlantilla(ILogger<ServicioPlantilla> logger,
        IServicionConfiguracionMongo configuracionMongo,
        IReflectorEntidadesAPI Reflector, IDistributedCache Cache) : base (null, null, logger, Reflector, Cache)
    {
        _logger = logger;
        reflector = Reflector;
        cache = Cache;

        var configuracionEntidad = configuracionMongo.ConexionEntidad(MongoDbContextConversaciones.NOMBRE_COLECCION_PLANTILLA);
        if(configuracionEntidad == null)
        {
            string err = $"No existe configuración de mongo para '{MongoDbContextConversaciones.NOMBRE_COLECCION_PLANTILLA}";
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
            _dbSetFull = ((MongoDbContextConversaciones)_db).Plantilla;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error al inicializar mongo para '{MongoDbContextConversaciones.NOMBRE_COLECCION_PLANTILLA}'");
            throw;
        }
    }

    private MongoDbContextConversaciones DB { get {  return (MongoDbContextConversaciones)_db; } }
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
        var add = data.Deserialize<Plantilla>(JsonAPIDefaults());
        var temp = await this.Insertar(add);
        RespuestaPayload<object> respuesta = JsonSerializer.Deserialize<RespuestaPayload<object>>(JsonSerializer.Serialize(temp));
        return respuesta;
    }

    public async Task<Respuesta> ActualizarAPI(object id, JsonElement data)
    {
        var update = data.Deserialize<Plantilla>(JsonAPIDefaults());
        return await this.Actualizar((string)id, update);
    }

    public async Task<Respuesta> EliminarAPI(object id)
    {
        return await this.Eliminar((string)id);
    }

    public async Task<RespuestaPayload<object>> UnicaPorIdAPI(Object id)
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

    #region Overrides para la personalización de la entidad Plantilla
    public override async Task<ResultadoValidacion> ValidarInsertar(Plantilla plantilla)
    {
        ResultadoValidacion resultado = new()
        {
            Valido = true
        };
        return resultado;
    }

    public override async Task<ResultadoValidacion> ValidarActualizar(string id, Plantilla actualizacion, Plantilla original)
    {
        ResultadoValidacion resultado = new()
        {
            Valido = true
        };
        return resultado;
    }

    public override Plantilla ADTOFull(Plantilla actualizacion, Plantilla actual)
    {
        actual.Contenidos = actualizacion.Contenidos;
        actual.AplicacionId = actualizacion.AplicacionId;
        actual.DeUsuario = actualizacion.DeUsuario;
        actual.FechaCreacion = actualizacion.FechaCreacion;
        return actual;
    }

    public override Plantilla ADTODespliegue(Plantilla data)
    {
        Plantilla plantilla = new Plantilla()
        {
            Id = data.Id,
            Contenidos = data.Contenidos,
            AplicacionId = data.AplicacionId,
            DeUsuario = data.DeUsuario,
            UsuarioId = data.UsuarioId,
            FechaCreacion = DateTime.UtcNow
        };
        return plantilla;
    }

    public override Plantilla ADTOFull(Plantilla data)
    {
        Plantilla plantilla = new Plantilla()
        {
            Id = data.Id,
            Contenidos = data.Contenidos,
            AplicacionId = data.AplicacionId,
            DeUsuario = data.DeUsuario,
            UsuarioId = data.UsuarioId,
            FechaCreacion = data.FechaCreacion
        };
        return plantilla;
    }


    #endregion
}
