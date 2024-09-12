using apigenerica.model.interpretes;
using apigenerica.model.modelos;
using apigenerica.model.reflectores;
using apigenerica.model.servicios;
using apigenerica.primitivas.aplicacion;
using comunes.interservicio.primitivas;
using comunes.primitivas;
using comunes.primitivas.configuracion.mongo;
using comunes.primitivas.seguridad;
using espaciotrabajo.model.espaciotrabajo;
using extensibilidad.metadatos;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using System.Text.Json;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace espaciotrabajo.services.espaciotrabajo;
[ServicioEntidadAPI(typeof(EspacioTrabajo))]
public class ServicioEspacioTrabajo : ServicioEntidadGenericaBase<EspacioTrabajo, EspacioTrabajo, EspacioTrabajo, EspacioTrabajo, string>,
    IServicioEntidadAPI, IServicioEspacioTrabajo
{
    //Variable temporal hasta implementar el HEADER para la identificación de la aplicación.
    const string NOMBRE_APP = "NeuroTeam";
    private ILogger<ServicioEspacioTrabajo> _logger;
    private readonly IReflectorEntidadesAPI reflector;
    private readonly IProveedorAplicaciones _proveedorAplicaciones;
    private readonly IProxySeguridad _proxySeguridad;
    private readonly IHttpContextAccessor httpContextAccessor;

    public ServicioEspacioTrabajo(ILogger<ServicioEspacioTrabajo> logger, IServicionConfiguracionMongo configuracionMongo,
        IReflectorEntidadesAPI reflector, IDistributedCache distributedCache, IProxySeguridad proxySeguridad, IHttpContextAccessor httpContextAccessor) : base(null, null, logger, reflector, distributedCache)
    {
        this._logger = logger;
        this.reflector = reflector;
        this._proxySeguridad = proxySeguridad;
        this.httpContextAccessor = httpContextAccessor;
        interpreteConsulta = new InterpreteConsultaExpresiones();

        var configuracionEntidad = configuracionMongo.ConexionEntidad(MongoDbContextEspacioTrabajo.NOMBRE_COLECCION_ESPACIOTRABAJO);
        if (configuracionEntidad == null)
        {
            string err = $"No existe configuracion de mongo para '{MongoDbContextEspacioTrabajo.NOMBRE_COLECCION_ESPACIOTRABAJO}'";
            _logger.LogError(err);
            throw new Exception(err);
        }

        try
        {
            _logger.LogDebug($"Mongo DB{configuracionEntidad.Esquema} colección {configuracionEntidad.Esquema} utilizando conexión default {string.IsNullOrEmpty(configuracionEntidad.Conexion)}");
            var cadenaConexion = string.IsNullOrEmpty(configuracionEntidad.Conexion) && string.IsNullOrEmpty(configuracionMongo.ConexionDefault())
                ? configuracionMongo.ConexionDefault()
                : string.IsNullOrEmpty(configuracionEntidad.Conexion)
                    ? configuracionMongo.ConexionDefault()
                    : configuracionEntidad.Conexion;
            var client = new MongoClient(cadenaConexion);

            _db = MongoDbContextEspacioTrabajo.Create(client.GetDatabase(configuracionEntidad.Esquema));
            _dbSetFull = ((MongoDbContextEspacioTrabajo)_db).EspaciosTrabajo;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error al inicializar mongo para '{MongoDbContextEspacioTrabajo.NOMBRE_COLECCION_ESPACIOTRABAJO}'");
            throw;
        }
    }
    private MongoDbContextEspacioTrabajo DB { get { return (MongoDbContextEspacioTrabajo)_db; } }
    public bool RequiereAutenticacion => true;

    public Entidad EntidadRepoAPI()
    {
        _logger.LogDebug("ServicioEspacioTrabajo-EntidadRepoAPI");
        return this.EntidadRepo();
    }

    public Entidad EntidadInsertAPI()
    {
        _logger.LogDebug("ServicioEspacioTrabajo-EntidadInsertAPI");
        return this.EntidadInsert();
    }

    public Entidad EntidadUpdateAPI()
    {
        _logger.LogDebug("ServicioEspacioTrabajo-EntidadUpdateAPI");
        return this.EntidadUpdate();
    }

    public Entidad EntidadDespliegueAPI()
    {
        _logger.LogDebug("ServicioEspacioTrabajo-DespliegueAPI");
        return this.EntidadDespliegue();
    }

    public void EstableceContextoUsuarioAPI(ContextoUsuario contexto)
    {
        _logger.LogDebug("ServicioEspacioTrabajo-ContextoUsuarioAPI");
        this._contextoUsuario = contexto;
    }

    public ContextoUsuario? ObtieneContextoUsuarioAPI()
    {
        _logger.LogDebug("ServicioEspacioTrabajo-ObtieneContextoUsuarioAPI");
        return ObtieneContextoUsuario();
    }

    public async Task<RespuestaPayload<object>> InsertarAPI(JsonElement data)
    {
        _logger.LogDebug("ServicioEspacioTrabajo-InsertarAPI-{data}", data);
        var add = data.Deserialize<EspacioTrabajo>(JsonAPIDefaults());
        var temp = await this.Insertar(add);
        RespuestaPayload<object> respuesta = JsonSerializer.Deserialize<RespuestaPayload<object>>(JsonSerializer.Serialize(temp));
        _logger.LogDebug("ServicioEspacioTrabajo-InsertarAPI resultado {ok} {code} {error}", respuesta!.Ok, respuesta!.HttpCode, respuesta.Error);
        return respuesta;
    }

    public async Task<Respuesta> ActualizarAPI(object id, JsonElement data)
    {
        _logger.LogDebug("ServicioEspacioTrabajo-ActualizarAPI-{data}", data);
        var update = data.Deserialize<EspacioTrabajo>(JsonAPIDefaults());
        Respuesta respuesta = await this.Actualizar((string)id, update);
        _logger.LogDebug("ServicioEspacioTrabajo-ActualizarAPI resultado {ok} {code} {error}", respuesta!.Ok, respuesta!.HttpCode, respuesta.Error);
        return respuesta;
    }

    public async Task<Respuesta> EliminarAPI(object id)
    {
        _logger.LogDebug("ServicioEspacioTrabajo-EliminarAPI");
        Respuesta respuesta = await this.Eliminar((string)id);
        _logger.LogDebug("ServicioEspacioTrabajo-EliminarAPI resultado {ok} {code} {error}", respuesta!.Ok, respuesta!.HttpCode, respuesta.Error);
        return respuesta;
    }

    public async Task<RespuestaPayload<object>> UnicaPorIdAPI(object id)
    {
        _logger.LogDebug("ServicioEspacioTrabajo-UnicaPorIdAPI");
        var temp = await this.UnicaPorIdDespliegue((string)id);
        RespuestaPayload<object> respuesta = JsonSerializer.Deserialize<RespuestaPayload<object>>(JsonSerializer.Serialize(temp));
        _logger.LogDebug("ServicioEspacioTrabajo-UnicaPorIdAPI resultado {ok} {code} {error}", respuesta!.Ok, respuesta!.HttpCode, respuesta.Error);
        return respuesta;
    }

    public async Task<RespuestaPayload<object>> UnicaPorIdDespliegueAPI(object id)
    {
        _logger.LogDebug("ServicioEspacioTrabajo-UnicaPorIdDespliegueAPI");
        var temp = await this.UnicaPorIdDespliegue((string)id);
        RespuestaPayload<object> respuesta = JsonSerializer.Deserialize<RespuestaPayload<object>>(JsonSerializer.Serialize(temp));
        _logger.LogDebug("ServicioEspacioTrabajo-UnicaPorIdDespliegueAPI resultado {ok} {code} {error}", respuesta!.Ok, respuesta!.HttpCode, respuesta.Error);
        return respuesta;
    }

    public async Task<RespuestaPayload<PaginaGenerica<object>>> PaginaAPI(Consulta consulta)
    {
        _logger.LogDebug("ServicioEspacioTrabajo-PaginaAPI-{consulta}", consulta);
        var temp = await this.Pagina(consulta);
        RespuestaPayload<PaginaGenerica<object>> respuesta = JsonSerializer.Deserialize<RespuestaPayload<PaginaGenerica<object>>>(JsonSerializer.Serialize(temp));
        _logger.LogDebug("ServicioEspacioTrabajo-PaginaAPI resultado {ok} {code} {error}", respuesta!.Ok, respuesta!.HttpCode, respuesta.Error);
        return respuesta;
    }

    public async Task<RespuestaPayload<PaginaGenerica<object>>> PaginaDespliegueAPI(Consulta consulta)
    {
        _logger.LogDebug("ServicioEspacioTrabajo-PaginaDespliegueAPI-{consulta}", consulta);
        var temp = await this.PaginaDespliegueAPI(consulta);
        RespuestaPayload<PaginaGenerica<object>> respuesta = JsonSerializer.Deserialize<RespuestaPayload<PaginaGenerica<object>>>(JsonSerializer.Serialize(temp));
        _logger.LogDebug("ServicioEspacioTrabajo-PaginaDespliegueAPI resultado {ok} {code} {error}", respuesta!.Ok, respuesta!.HttpCode, respuesta.Error);
        return respuesta;
    }

    #region Overrides para la personalización de la ENTIDAD => Curso

    public override async Task<ResultadoValidacion> ValidarActualizar(string id, EspacioTrabajo actualizacion, EspacioTrabajo original)
    {
        return new ResultadoValidacion() { Valido = true };
    }

    public override async Task<ResultadoValidacion> ValidarEliminacion(string id, EspacioTrabajo original)
    {
        return new ResultadoValidacion() { Valido = true };
    }

    public override async Task<ResultadoValidacion> ValidarInsertar(EspacioTrabajo data)
    {
        return new ResultadoValidacion() { Valido = true };
    }

    public override EspacioTrabajo ADTOFull(EspacioTrabajo data)
    {
        EspacioTrabajo espacioTrabajo = new()
        {
            Id = Guid.NewGuid(),
            Nombre = data.Nombre,
            TenantId = data.TenantId,
            AppId = data.AppId,
            Miembros = data.Miembros,
        };
        return espacioTrabajo;
    }

    public override EspacioTrabajo ADTOFull(EspacioTrabajo actualizacion, EspacioTrabajo actual)
    {
        actual.Nombre = actualizacion.Nombre;
        actual.TenantId = actualizacion.TenantId;
        actual.AppId = actualizacion.AppId;
        actual.Miembros = actualizacion.Miembros;
        return actual;
    }


    public override EspacioTrabajo ADTODespliegue(EspacioTrabajo data)
    {
        EspacioTrabajo espacioTrabajo = new()
        {
            Id = data.Id,
            Nombre = data.Nombre,
            TenantId = data.TenantId,
            AppId = data.AppId,
            Miembros = data.Miembros,
        };

        return espacioTrabajo;
    }

    public virtual async Task<Respuesta> Actualizar(string id, EspacioTrabajo data)
    {
        var respuesta = new Respuesta();
        try
        {
            if (string.IsNullOrEmpty(id) || data == null)
            {
                respuesta.Error = new ErrorProceso()
                {
                    Codigo = CodigosError.ESPACIOTRABAJO_ID_PAYLOAD_NO_INGRESADO,
                    Mensaje = "No ha sido proporcionado el Id ó Payload",
                    HttpCode = HttpCode.BadRequest
                };
                respuesta.HttpCode = HttpCode.BadRequest;
                return respuesta;
            }


            EspacioTrabajo actual = _dbSetFull.Find(Guid.Parse(id));

            if (actual == null)
            {
                respuesta.Error = new ErrorProceso()
                {
                    Codigo = CodigosError.ESPACIOTRABAJO_NO_ENCONTRADA,
                    Mensaje = "No existe un EspacioTrabajo con el Id proporcionado",
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
                respuesta.Error!.Codigo = CodigosError.ESPACIOTRABAJO_DATOS_NO_VALIDOS;
                respuesta.HttpCode = resultadoValidacion.Error?.HttpCode ?? HttpCode.None;
            }

        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "ServicioEspacioTrabajo-Actualizar {msg}", ex.Message);
            respuesta.Error = new ErrorProceso() { Codigo = CodigosError.ESPACIOTRABAJO_ERROR_DESCONOCIDO, HttpCode = HttpCode.ServerError, Mensaje = ex.Message };
            respuesta.HttpCode = HttpCode.ServerError;
        }

        return respuesta;
    }

    public override async Task<RespuestaPayload<EspacioTrabajo>> UnicaPorId(string id)
    {
        var respuesta = new RespuestaPayload<EspacioTrabajo>();
        try
        {
            EspacioTrabajo actual = await _dbSetFull.FindAsync(Guid.Parse(id));
            if (actual == null)
            {
                respuesta.Error = new ErrorProceso()
                {
                    Codigo = CodigosError.ESPACIOTRABAJO_ID_PAYLOAD_NO_INGRESADO,
                    Mensaje = "No existe un EspacioTrabajo con el Id proporcionado",
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
            _logger.LogError(ex, "ServicioEspacioTrabajo-UnicaPorId {msg}", ex.Message);
            respuesta.Error = new ErrorProceso() { Codigo = CodigosError.ESPACIOTRABAJO_ERROR_DESCONOCIDO, HttpCode = HttpCode.ServerError, Mensaje = ex.Message };
            respuesta.HttpCode = HttpCode.ServerError;
        }
        return respuesta;
    }

    public virtual async Task<Respuesta> Eliminar(string id)
    {
        var respuesta = new Respuesta();
        try
        {

            if (string.IsNullOrEmpty(id))
            {
                respuesta.Error = new ErrorProceso()
                {
                    Codigo = CodigosError.ESPACIOTRABAJO_ID_NO_INGRESADO,
                    Mensaje = "No ha sido proporcionado el Id",
                    HttpCode = HttpCode.BadRequest
                };
                respuesta.HttpCode = HttpCode.BadRequest;
                return respuesta;
            }

            EspacioTrabajo actual = _dbSetFull.Find(Guid.Parse(id));
            if (actual == null)
            {
                respuesta.Error = new ErrorProceso()
                {
                    Codigo = CodigosError.ESPACIOTRABAJO_ID_NO_INGRESADO,
                    Mensaje = "No existe un EspacioTrabajo con el Id proporcionado",
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
                respuesta.Error!.Codigo = CodigosError.ESPACIOTRABAJO_DATOS_NO_VALIDOS;
                respuesta.HttpCode = resultadoValidacion.Error?.HttpCode ?? HttpCode.None;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "ServicioEspacioTrabajo-Eliminar {msg}", ex.Message);
            respuesta.Error = new ErrorProceso() { Codigo = CodigosError.ESPACIOTRABAJO_ERROR_DESCONOCIDO, HttpCode = HttpCode.ServerError, Mensaje = ex.Message };
            respuesta.HttpCode = HttpCode.ServerError;
        }
        return respuesta;
    }

    public async Task<RespuestaPayload<List<EspacioTrabajoUsuario>>> ObtieneEspaciosUsuario(string usuarioId, string dominioId, string unidadOrgId)
    {
        _logger.LogDebug("ServicioEspacioTrabajo - ObtieneEspaciosUsuario {usuarioId}", usuarioId);
        RespuestaPayload<List<EspacioTrabajoUsuario>> respuestaPayload = new();
        var espaciosTrabajo = DB.EspaciosTrabajo.ToList();

        if(!espaciosTrabajo.Any())
        {
            respuestaPayload.Error = new ErrorProceso()
            {
                Mensaje = "No existen EspaciosTrabajo",
                Codigo = CodigosError.ESPACIOTRABAJO_NO_EXISTEN_ESPACIOSTRABAJO,
                HttpCode = HttpCode.NotFound
            };
            respuestaPayload.HttpCode = HttpCode.NotFound;
            return respuestaPayload;
        }

        EspacioTrabajoUsuario espacioTrabajoUsuario = new EspacioTrabajoUsuario();
        List<EspacioTrabajoUsuario> listaEspacioTrabajoUsuario = new List<EspacioTrabajoUsuario>();
        foreach (var x in espaciosTrabajo)
        {
            var userSearch = x.Miembros.Find(y => y.UsuarioId.Equals(usuarioId));

            if (userSearch != null)
            {
                var permisos = this._proxySeguridad.PermisosUsuario(x.AppId, usuarioId, dominioId, unidadOrgId).Result.Select(p => p.Nombre).ToList();
                var roles = this._proxySeguridad.RolesUsuario(x.AppId, usuarioId, dominioId, unidadOrgId).Result.Select(r => r.Nombre).ToList();

                EspacioTrabajoBase espacioTrabajoBase = new EspacioTrabajoBase() { Id = (Guid)x.Id, Nombre = x.Nombre, Permisos = permisos, Roles = roles };
                espacioTrabajoUsuario.Espacios.Add(espacioTrabajoBase);
            }

        }

        if (!espacioTrabajoUsuario.Espacios.Any())
        {
            respuestaPayload.Error = new ErrorProceso()
            {
                Mensaje = "Miembro no pertenece a ningún espacio de trabajo",
                Codigo = CodigosError.ESPACIOTRABAJO_MIEMBRO_NO_PERTENECE_A_NINGUN_ESPACIOTRABAJO,
                HttpCode = HttpCode.NotFound
            };
            respuestaPayload.HttpCode = HttpCode.NotFound;
            return respuestaPayload;
        }
        listaEspacioTrabajoUsuario.Add(espacioTrabajoUsuario);
        respuestaPayload.Ok = true;
        respuestaPayload.Payload = listaEspacioTrabajoUsuario;

        _logger.LogDebug("ServicioEspacioTrabajo - ObtieneEspaciosUsuario - resultado {Ok} {code} {error}", respuestaPayload!.Ok, respuestaPayload!.HttpCode, respuestaPayload.Error);
        return respuestaPayload; 
    }

    public async Task<Respuesta> ActualizaDbSetEspacioTrabajo(EspacioTrabajo espacioTrabajo)
    {
        _logger.LogDebug("ServicioEspacioTrabajo - ActualizaDbSetEspacioTrabajo {curso} ", espacioTrabajo);
        var respuesta = new Respuesta();

        this._dbSetFull.Update(espacioTrabajo);
        await _db.SaveChangesAsync();

        respuesta.Ok = true;
        _logger.LogDebug("ServicioEspacioTrabajo - ActualizaDbSetEspacioTrabajo resultado {ok} {code} {error}", respuesta!.Ok, respuesta!.HttpCode, respuesta.Error);
        return respuesta;
    }
    #endregion
}
