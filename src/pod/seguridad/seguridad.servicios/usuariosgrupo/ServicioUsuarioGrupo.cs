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
using System.Collections.Specialized;
using System.Text.Json;


namespace seguridad.servicios;
[ServicioEntidadAPI(entidad: typeof(UsuarioGrupo), driver: Constantes.MONGODB)]
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
        _logger.LogDebug("ServicioUsuarioGrupo-EntidadRepoAPI");
        return this.EntidadRepo();
    }
    public Entidad EntidadInsertAPI()
    {
        _logger.LogDebug("ServicioUsuarioGrupo-EntidadInsertAPI");
        return this.EntidadInsert();
    }
    public Entidad EntidadUpdateAPI()
    {
        _logger.LogDebug("ServicioUsuarioGrupo-EntidadUpdateAPI");
        return this.EntidadUpdate();
    }
    public Entidad EntidadDespliegueAPI()
    {
        _logger.LogDebug("ServicioUsuarioGrupo-EntidadDespliegueAPI");
        return this.EntidadDespliegue();
    }

    public void EstableceContextoUsuarioAPI(ContextoUsuario contexto)
    {
        _logger.LogDebug("ServicioUsuarioGrupo-EstableceContextoUsuarioAPi");
        this.EstableceContextoUsuario(contexto);
    }

    public void EstableceDbSet(string padreId)
    {
        _logger.LogDebug("ServicioUsuarioGrupo-EstableceDbSet {padreId}", padreId);
        grupo = _dbSetgrupoUsuarios.FirstOrDefault(_ => _.Id == Guid.Parse(padreId));
        this.Padreid = grupo!= null ? grupo.Id.ToString() : null;
        _logger.LogDebug("ServicioUsuarioGrupo-EstableceDbSet {padreId}", this.Padreid);
    }
    public ContextoUsuario? ObtieneContextoUsuarioAPI()
    {
        _logger.LogDebug("ServicioUsuarioGrupo-ObtieneContextoUsuarioAPI");
        return this._contextoUsuario;
    }

    public async Task<RespuestaPayload<object>> InsertarAPI(JsonElement data, StringDictionary? parametros = null)
    {
        _logger.LogDebug("ServicioUsuarioGrupo - InsertarAPI - {data}", data);
        var add = data.Deserialize<CreaUsuarioGrupo>(JsonAPIDefaults());
        var temp = await this.Insertar(add);
        RespuestaPayload<object> respuesta = JsonSerializer.Deserialize<RespuestaPayload<object>>(JsonSerializer.Serialize(temp));
        _logger.LogDebug("ServicioUsuarioGrupo - InsertarAPI - resultado {ok} {code} {error}", respuesta!.Ok, respuesta!.HttpCode, respuesta.Error);
        return respuesta;
    }

    public async Task<Respuesta> ActualizarAPI(object id, JsonElement data, StringDictionary? parametros = null)
    {
        _logger.LogDebug("ServicioUsuarioGrupo-ActualizarAPI {data}", data);
        var update = data.Deserialize<UsuarioGrupo>(JsonAPIDefaults());
        Respuesta respuesta = await this.Actualizar((string)id, update);
        _logger.LogDebug("ServicioUsuarioGrupo-ActualizarAPI resultado {ok} {code} {error}", respuesta!.Ok, respuesta!.HttpCode, respuesta.Error);
        return respuesta;
    }

    public async Task<Respuesta> EliminarAPI(object id, JsonElement data, StringDictionary? parametros = null)
    {
        _logger.LogDebug("ServicioUsuarioGrupo-EliminarAPI");
        Respuesta respuesta = await this.Eliminar((string)id);
        _logger.LogDebug("ServicioUsuarioGrupo-EliminarAPI resultado {ok} {code} {error}", respuesta!.Ok, respuesta!.HttpCode, respuesta.Error);
        return respuesta;
    }

    public async Task<RespuestaPayload<object>> UnicaPorIdAPI(object id, StringDictionary? parametros = null)
    {
        _logger.LogDebug("ServicioUsuarioGrupo-UnicaPorIdAPI");
        var temp = await this.UnicaPorId((string)id);
        RespuestaPayload<object> respuesta = JsonSerializer.Deserialize<RespuestaPayload<object>>(JsonSerializer.Serialize(temp));
        _logger.LogDebug("ServicioUsuarioGrupo-UnicarPorIdAPI resultado {ok} {code} {error", respuesta!.Ok, respuesta!.HttpCode, respuesta.Error);
        return respuesta;
    }

    public async Task<RespuestaPayload<object>> UnicaPorIdDespliegueAPI(object id, StringDictionary? parametros = null)
    {
        _logger.LogDebug("ServicioUsuarioGrupo-UnicaPorIdDespliegueAPI");
        var temp = await this.UnicaPorIdDespliegue((string)id);
        RespuestaPayload<object> respuesta = JsonSerializer.Deserialize<RespuestaPayload<object>>(JsonSerializer.Serialize(temp));
        _logger.LogDebug("ServicioUsuarioGrupo-UnicaPorIdDespliegueAPI resultado {ok} {code} {error}", respuesta!.Ok, respuesta!.HttpCode, respuesta.Error);
        return respuesta;
    }

    public async Task<RespuestaPayload<PaginaGenerica<object>>> PaginaAPI(Consulta consulta, StringDictionary? parametros = null)
    {
        _logger.LogDebug("ServicioUsuarioGrupo-PaginaAPI {consulta}", consulta);
        var temp = await this.Pagina(consulta);
        RespuestaPayload<PaginaGenerica<object>> respuesta = JsonSerializer.Deserialize<RespuestaPayload<PaginaGenerica<object>>>(JsonSerializer.Serialize(temp));
        _logger.LogDebug("ServicioUsuarioGrupo-PaginaAPI resultado {ok} {code} {error}", respuesta!.Ok, respuesta!.HttpCode, respuesta.Error);
        return respuesta;
    }

    public async Task<RespuestaPayload<PaginaGenerica<object>>> PaginaDespliegueAPI(Consulta consulta, StringDictionary? parametros = null)
    {
        _logger.LogDebug("ServicioPlantilla-PaginaDespliegueAPI-{consulta}", consulta);
        var temp = await this.PaginaDespliegue(consulta);
        RespuestaPayload<PaginaGenerica<object>> respuesta = JsonSerializer.Deserialize<RespuestaPayload<PaginaGenerica<object>>>(JsonSerializer.Serialize(temp));
        _logger.LogDebug("ServicioPlantilla-PaginaDespliegueAPI resultado {ok} {code} {error}", respuesta!.Ok, respuesta!.HttpCode, respuesta.Error);
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
    public override async Task<RespuestaPayload<ConsultaUsuarioGrupo>> Insertar(CreaUsuarioGrupo data, StringDictionary? parametros = null)
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
                respuesta.Error = resultadoValidacion.Error;
                respuesta.Error!.Codigo = CodigosError.SEGURIDAD_DATOS_NO_VALIDOS;
                respuesta.HttpCode = resultadoValidacion.Error?.HttpCode ?? HttpCode.None;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "ServicioUsuarioGrupo-Insertar {msg}", ex.Message);
            respuesta.Error = new ErrorProceso() { Codigo = CodigosError.SEGURIDAD_ERROR_DESCONOCIDO, HttpCode = HttpCode.ServerError, Mensaje = ex.Message };
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
                respuesta.Error = new ErrorProceso()
                {
                    Codigo = CodigosError.SEGURIDAD_USUARIOGRUPO_NO_ENCONTRADA,
                    Mensaje = "No existe un UsuarioGrupo con el Id proporcionado",
                    HttpCode = HttpCode.NotFound
                };
                respuesta.HttpCode = HttpCode.NotFound;
                return respuesta;
            }
            respuesta.Ok = true;
            respuesta.HttpCode = HttpCode.Ok;
            respuesta.Payload = respuesta.Payload = new UsuarioGrupo() {Id = Guid.NewGuid().ToString(), UsuarioId = actual };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "ServicioUsuarioGrupo-UnicaPorId {msg}", ex.Message);
            respuesta.Error = new ErrorProceso() { Codigo = CodigosError.SEGURIDAD_ERROR_DESCONOCIDO, HttpCode = HttpCode.ServerError, Mensaje = ex.Message };
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
                    Codigo = CodigosError.SEGURIDAD_USUARIOGRUPO_ID_PAYLOAD_NO_INGRESADO,
                    Mensaje = "No ha sido proporcionado el Id ó Payload",
                    HttpCode = HttpCode.BadRequest
                };
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
                respuesta.Error = new ErrorProceso()
                {
                    Codigo = CodigosError.SEGURIDAD_USUARIOGRUPO_ERROR_ELIMINAR,
                    Mensaje = "No ha sido posible ELIMINAR el UsuarioGrupo",
                    HttpCode = HttpCode.BadRequest
                };
                respuesta.HttpCode = HttpCode.BadRequest;
                return respuesta;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "ServicioUsuarioGrupo-Eliminar {msg}", ex.Message);
            respuesta.Error = new ErrorProceso() { Codigo = CodigosError.SEGURIDAD_ERROR_DESCONOCIDO, HttpCode = HttpCode.ServerError, Mensaje = ex.Message };
            respuesta.HttpCode = HttpCode.ServerError;
        }
        return respuesta;
    }
    public override async Task<PaginaGenerica<UsuarioGrupo>> ObtienePaginaElementos(Consulta consulta)
    {
        _logger.LogDebug("ServicioUsuarioGrupo - ObtienePaginaElementos - {consulta}", consulta);
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
