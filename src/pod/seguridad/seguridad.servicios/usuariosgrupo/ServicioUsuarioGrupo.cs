#pragma warning disable CS8603 // Possible null reference return.
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
using apigenerica.model.interpretes;
using apigenerica.model.modelos;
using apigenerica.model.reflectores;
using apigenerica.model.servicios;
using comunes.primitivas;
using comunes.primitivas.configuracion.mongo;
using extensibilidad.metadatos;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using seguridad.modelo;
using seguridad.modelo.roles;
using seguridad.modelo.servicios;
using seguridad.servicios.dbcontext;
using System.Text.Json;


namespace seguridad.servicios;
[ServicioEntidadAPI(entidad: typeof(UsuarioGrupo))]
public class ServicioUsuarioGrupo : ServicioEntidadHijoGenericaBase<UsuarioGrupo, CreaUsuarioGrupo, UsuarioGrupo, ConsultaUsuarioGrupo, string>,
    IServicioEntidadHijoAPI, IServicioUsuarioGrupo
{
    private readonly ILogger _logger;

    private readonly IReflectorEntidadesAPI reflector;
    private GrupoUsuarios? grupo;
    private DbSet<GrupoUsuarios>? _dbSetgrupoUsuarios;
    public ServicioUsuarioGrupo(ILogger<ServicioRol> logger,
        IServicionConfiguracionMongo configuracionMongo,
        IReflectorEntidadesAPI Reflector, IDistributedCache cache) : base(null, null, logger, Reflector, cache)
    {
        _logger = logger;
        reflector = Reflector;
        interpreteConsulta = new InterpreteConsultaExpresiones();

        var configuracionEntidad = configuracionMongo.ConexionEntidad(MongoDbContext.NOMBRE_COLECCION_GRUPOUSUARIOS);
        if (configuracionEntidad == null)
        {
            string err = $"No existe configuracion de mongo para '{MongoDbContext.NOMBRE_COLECCION_GRUPOUSUARIOS}'";
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
            _dbSetgrupoUsuarios = ((MongoDbContext)_db).GrupoUsuarios;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error al inicializar mongo para '{MongoDbContext.NOMBRE_COLECCION_GRUPOUSUARIOS}'");
            throw;
        }
    }
    private MongoDbContext DB { get { return (MongoDbContext)_db; } }
    public bool RequiereAutenticacion => true;

    string IServicioEntidadHijoAPI.TipoPadreId { get => this.TipoPadreId; set => this.TipoPadreId = value; }
    string IServicioEntidadHijoAPI.Padreid { get => this.grupo.Id.ToString() ?? null; set => EstableceDbSet(value); }

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

    public void EstableceDbSet(string padreId)
    {
        grupo = _dbSetgrupoUsuarios.FirstOrDefault(_ => _.Id == Guid.Parse(padreId));
        this.Padreid = grupo!= null ? grupo.Id.ToString() : null;
    }
    public ContextoUsuario? ObtieneContextoUsuarioAPI()
    {
        return this._contextoUsuario;
    }

    public async Task<RespuestaPayload<object>> InsertarAPI(JsonElement data)
    {
        var add = data.Deserialize<CreaUsuarioGrupo>(JsonAPIDefaults());
        var temp = await this.Insertar(add);
        RespuestaPayload<object> respuesta = JsonSerializer.Deserialize<RespuestaPayload<object>>(JsonSerializer.Serialize(temp));
        return respuesta;
    }

    public async Task<Respuesta> ActualizarAPI(object id, JsonElement data)
    {
        throw new NotImplementedException();
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
    public override async Task<ResultadoValidacion> ValidarInsertar(CreaUsuarioGrupo data)
    {
        ResultadoValidacion resultado = new();
        resultado.Valido = grupo != null && !grupo.UsuarioId.Any(_=>_==data.UsuarioId);
        return resultado;
    }
    public override async Task<ResultadoValidacion> ValidarEliminacion(string id, UsuarioGrupo original)
    {
        ResultadoValidacion resultado = new();
        resultado.Valido = grupo != null ? true : false;
        return resultado;
    }

    public override async Task<ResultadoValidacion> ValidarActualizar(string id, UsuarioGrupo actualizacion, UsuarioGrupo original)
    {
        ResultadoValidacion resultado = new();

        resultado.Valido = true;

        return resultado;
    }

    public override UsuarioGrupo ADTOFull(UsuarioGrupo actualizacion, UsuarioGrupo actual)
    {
        return actual;
    }

    public override UsuarioGrupo ADTOFull(CreaUsuarioGrupo data)
    {
        UsuarioGrupo rol = new UsuarioGrupo()
        {
            Id=Guid.NewGuid().ToString(),
            UsuarioId = data.UsuarioId
        };
        return rol;
    }
    public override ConsultaUsuarioGrupo ADTODespliegue(UsuarioGrupo data)
    {
        return new ConsultaUsuarioGrupo
        {
          UsuarioId= data.UsuarioId
        };
    }
    public override async Task<RespuestaPayload<ConsultaUsuarioGrupo>> Insertar(CreaUsuarioGrupo data)
    {
        var respuesta = new RespuestaPayload<ConsultaUsuarioGrupo>();

        try
        {
            var resultadoValidacion = await ValidarInsertar(data);
            if (resultadoValidacion.Valido)
            {
                var entidad = ADTOFull(data);
                grupo.UsuarioId.Add(entidad.UsuarioId);
                _dbSetgrupoUsuarios.Update(grupo);
                await _db.SaveChangesAsync();
                respuesta.Ok = true;
                respuesta.HttpCode = HttpCode.Ok;
                respuesta.Payload = ADTODespliegue(entidad);
            }
            else
            {
                respuesta.HttpCode = resultadoValidacion.Error?.HttpCode ?? HttpCode.BadRequest;
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

    public override async Task<RespuestaPayload<UsuarioGrupo>> UnicaPorId(string id)
    {
        var respuesta = new RespuestaPayload<UsuarioGrupo>();
        try
        {
            string actual = grupo.UsuarioId.FirstOrDefault(_=>_==id);
            if (string.IsNullOrEmpty(actual))
            {                
                respuesta.HttpCode = HttpCode.NotFound;
                return respuesta;
            }
            respuesta.Ok = true;
            respuesta.HttpCode = HttpCode.Ok;
            respuesta.Payload = respuesta.Payload = new UsuarioGrupo() {Id = Guid.NewGuid().ToString(), UsuarioId = actual };
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
            var actual = grupo.UsuarioId.FirstOrDefault(_ => _ == id);
            if (string.IsNullOrEmpty(actual))
            {
                respuesta.Ok = true;
                respuesta.HttpCode = HttpCode.Ok;
                return respuesta;
            }
            UsuarioGrupo usuario= new UsuarioGrupo() { Id = Guid.NewGuid().ToString(), UsuarioId = actual };
            var resultadoValidacion = await ValidarEliminacion(id, usuario);
            if (resultadoValidacion.Valido)
            {
                grupo.UsuarioId.Remove(actual);
                _dbSetgrupoUsuarios.Update(grupo);
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
    public override async Task<PaginaGenerica<UsuarioGrupo>> ObtienePaginaElementos(Consulta consulta)
    {
        var Elementos =grupo!=null && grupo.UsuarioId.Any() ? grupo.UsuarioId.AsQueryable() : new List<string>().AsQueryable();
        var ElementosFinal = new List<UsuarioGrupo>();
        var pagina = Elementos.Paginado(consulta);
        if (pagina.Elementos.Any()) pagina.Elementos.ForEach(i => { ElementosFinal.Add(new UsuarioGrupo() { Id = Guid.NewGuid().ToString(), UsuarioId = i }); });
        return new PaginaGenerica<UsuarioGrupo>
        {
            ConsultaId = pagina.ConsultaId,
            Elementos = ElementosFinal,
            Milisegundos = 0,
            Paginado = pagina.Paginado,
            Total = pagina.Total,
        };
    }

    #endregion
}
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
#pragma warning restore CS8603 // Possible null reference return.
