#pragma warning disable CS8603 // Possible null reference return.
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
using apigenerica.model.interpretes;
using apigenerica.model.modelos;
using apigenerica.model.reflectores;
using apigenerica.model.servicios;
using comunes.primitivas;
using comunes.primitivas.configuracion.mongo;
using conversaciones.model;
using conversaciones.services.dbcontext;
using conversaciones.services.plantilla;
using extensibilidad.metadatos;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using System.Text.Json;

namespace conversaciones.services.conversacion;
[ServicioEntidadAPI(entidad: typeof(Conversacion))]
public class ServicioConversacion : ServicioEntidadGenericaBase<Conversacion, Conversacion, Conversacion, Conversacion, string>,
    IServicioEntidadAPI, IServicioConversacion
{
    private readonly ILogger _logger;
    private readonly IReflectorEntidadesAPI reflector;
    private readonly IDistributedCache cache;

    public ServicioConversacion(ILogger<ServicioConversacion> logger,
        IServicionConfiguracionMongo configuracionMongo,
        IReflectorEntidadesAPI Reflector, IDistributedCache Cache) : base(null, null, logger, Reflector, Cache)
    {
        _logger = logger;
        reflector = Reflector;
        cache = Cache;

        var configuracionEntidad = configuracionMongo.ConexionEntidad(MongoDbContextConversaciones.NOMBRE_COLECCION_CONVERSACION);
        if (configuracionEntidad == null)
        {
            string err = $"No existe configuración de mongo para '{MongoDbContextConversaciones.NOMBRE_COLECCION_CONVERSACION}";
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
            _dbSetFull = ((MongoDbContextConversaciones)_db).Conversacion;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error al inicializar mongo para '{MongoDbContextConversaciones.NOMBRE_COLECCION_CONVERSACION}'");
            throw;
        }
    }

    private MongoDbContextConversaciones DB { get { return (MongoDbContextConversaciones)_db; } }
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
        var add = data.Deserialize<Conversacion>(JsonAPIDefaults());
        var temp = await this.Insertar(add);
        RespuestaPayload<object> respuesta = JsonSerializer.Deserialize<RespuestaPayload<object>>(JsonSerializer.Serialize(temp));
        return respuesta;
    }

    public async Task<Respuesta> ActualizarAPI(object id, JsonElement data)
    {
        var update = data.Deserialize<Conversacion>(JsonAPIDefaults());
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
    public override async Task<ResultadoValidacion> ValidarInsertar(Conversacion plantilla)
    {
        ResultadoValidacion resultado = new()
        {
            Valido = true
        };
        return resultado;
    }

    public override async Task<ResultadoValidacion> ValidarActualizar(string id, Conversacion actualizacion, Conversacion original)
    {
        ResultadoValidacion resultado = new()
        {
            Valido = true
        };
        return resultado;
    }

    public override Conversacion ADTOFull(Conversacion actualizacion, Conversacion actual)
    {
        actual.Emisor = actualizacion.Emisor;
        actual.Participantes = actualizacion.Participantes;
        actual.Canal = actualizacion.Canal;
        actual.Nombre = actualizacion.Nombre;
        actual.FechaCreacion = actualizacion.FechaCreacion;
        actual.FechaActualizacion = actualizacion.FechaActualizacion;
        actual.CantidadMensajes = actualizacion.CantidadMensajes;
        actual.Mensajes = actualizacion.Mensajes;
        actual.Unidireccional = actualizacion.Unidireccional;
        return actual;
    }

    public override Conversacion ADTODespliegue(Conversacion data)
    {
        Conversacion conversacion = new Conversacion()
        {
            Id = data.Id,
            Emisor = data.Emisor,
            Participantes = data.Participantes,
            Canal = data.Canal,
            Nombre = data.Nombre,
            FechaCreacion = data.FechaCreacion,
            FechaActualizacion = data.FechaActualizacion,
            CantidadMensajes = data.CantidadMensajes,
            Mensajes = data.Mensajes,
            Unidireccional = data.Unidireccional

        };
        return conversacion;
    }

    public override Conversacion ADTOFull(Conversacion data)
    {
        Conversacion conversacion = new Conversacion()
        {
            Id = data.Id,
            Emisor = data.Emisor,
            Participantes = data.Participantes,
            Canal = data.Canal,
            Nombre = data.Nombre,
            FechaCreacion = data.FechaCreacion,
            FechaActualizacion = data.FechaActualizacion,
            CantidadMensajes = data.CantidadMensajes,
            Mensajes = data.Mensajes,
            Unidireccional = data.Unidireccional
        };
        return conversacion;
    }
    #endregion
}
#pragma warning disable CS8603 // Possible null reference return.
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.