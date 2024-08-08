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
using extensibilidad.metadatos;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using System.Text.Json;
using Plantilla = conversaciones.model.Plantilla;
namespace conversaciones.services.plantilla.contenido;
[ServicioEntidadAPI(entidad:typeof(Contenido))]
public class ServicioContenido : ServicioEntidadHijoGenericaBase<Contenido,Contenido,Contenido,Contenido,string>,
    IServicioEntidadHijoAPI,IServicioContenido
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

    string IServicioEntidadHijoAPI.TipoPadreId { get => this.TipoPadreId; set => this.TipoPadreId = value; }

    string IServicioEntidadHijoAPI.Padreid { get => this.plantilla.Id ?? null; set => EstableceDbSet(value); }

    public async Task<Respuesta> ActualizarAPI(object id, JsonElement data)
    {
        var update = data.Deserialize<Contenido>(JsonAPIDefaults());
        return await this.Actualizar((string)id, update);
    }

    public async Task<Respuesta> EliminarAPI(object id)
    {
        return await this.Eliminar((string)id);
    }

    public Entidad EntidadDespliegueAPI()
    {
        return this.EntidadDespliegue();
    }

    public Entidad EntidadInsertAPI()
    {
        return this.EntidadInsert();
    }

    public Entidad EntidadRepoAPI()
    {
        return this.EntidadRepo();
    }

    public Entidad EntidadUpdateAPI()
    {
        return this.EntidadUpdate();
    }

    public void EstableceContextoUsuarioAPI(ContextoUsuario contexto)
    {
        this.EstableceContextoUsuario(contexto);
    }

    public void EstableceDbSet(string padreId)
    {
        plantilla = _dbSetPlantilla.FirstOrDefault(_ => _.Id == padreId);
        this.Padreid = plantilla != null ? plantilla.Id : null;
    }
    public async Task<RespuestaPayload<object>> InsertarAPI(JsonElement data)
    {
        var add = data.Deserialize<Contenido>(JsonAPIDefaults());
        var temp = await this.Insertar(add);
        RespuestaPayload<Object> respuesta = JsonSerializer.Deserialize<RespuestaPayload<object>>(JsonSerializer.Serialize(temp));
        return respuesta;
    }
    public ContextoUsuario? ObtieneContextoUsuarioAPI()
    {
        return this._contextoUsuario;
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

    #region Overrides para la personalizacion de le ENTIDAD => Contenido
    public override async Task<ResultadoValidacion> ValidarInsertar(Contenido data)
    {
        ResultadoValidacion resultado = new();
        resultado.Valido = true;
        return resultado;
    }
    public override async Task<ResultadoValidacion> ValidarEliminacion(string id, Contenido original)
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

    public override async Task<PaginaGenerica<Contenido>> ObtienePaginaElementos(Consulta consulta)
    {
        Entidad entidad = reflectorEntidades.ObtieneEntidad(typeof(Contenido));
        var Elementos = Enumerable.Empty<Contenido>().AsQueryable();
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
