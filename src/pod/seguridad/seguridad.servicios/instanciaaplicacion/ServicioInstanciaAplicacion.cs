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
using seguridad.modelo.instancias;
using seguridad.servicios.dbcontext;
using System.Text.Json;


namespace seguridad.servicios;
[ServicioEntidadAPI(entidad: typeof(InstanciaAplicacion))]
public class ServicioInstanciaAplicacion : ServicioEntidadGenericaBase<InstanciaAplicacion, InstanciaAplicacion, InstanciaAplicacion, InstanciaAplicacion, string>,
    IServicioEntidadAPI, IServicioInstanciaAplicacion
{
    private readonly ILogger _logger;
    private readonly IReflectorEntidadesAPI reflector;
    private readonly IServicioAplicacion servicioAplicacion;

    public ServicioInstanciaAplicacion(ILogger<ServicioInstanciaAplicacion> logger,
        IServicionConfiguracionMongo configuracionMongo,
        IReflectorEntidadesAPI Reflector, IDistributedCache cache,IServicioAplicacion servicioAplicacion) : base(null, null, logger, Reflector, cache)
    {
        _logger = logger;
        reflector = Reflector;
        this.servicioAplicacion = servicioAplicacion;
        interpreteConsulta = new InterpreteConsultaExpresiones();

        var configuracionEntidad = configuracionMongo.ConexionEntidad(MongoDbContext.NOMBRE_COLECCION_INSTANCIAAPLICAION);
        if (configuracionEntidad == null)
        {
            string err = $"No existe configuracion de mongo para '{MongoDbContext.NOMBRE_COLECCION_INSTANCIAAPLICAION}'";
            _logger.LogError(err);
            throw new Exception(err);
        }

        try
        {
            _logger.LogDebug($"Mongo DB {configuracionEntidad.Esquema} colección {configuracionEntidad.Esquema} utilizando conexión default {string.IsNullOrEmpty(configuracionEntidad.Conexion)}");
            var cadenaConexion = string.IsNullOrEmpty(configuracionEntidad.Conexion) && string.IsNullOrEmpty(configuracionMongo.ConexionDefault())
                ? configuracionMongo.ConexionDefault()
                : string.IsNullOrEmpty(configuracionEntidad.Conexion)
                    ? configuracionMongo.ConexionDefault()
                    : configuracionEntidad.Conexion;
            var client = new MongoClient(cadenaConexion);

            _db = MongoDbContext.Create(client.GetDatabase(configuracionEntidad.Esquema));
            _dbSetFull = ((MongoDbContext)_db).instanciaAplicacion;

        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error al inicializar mongo para '{MongoDbContext.NOMBRE_COLECCION_INSTANCIAAPLICAION}'");
            throw;
        }
    }
    private MongoDbContext DB { get { return (MongoDbContext)_db; } }
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
        var add = data.Deserialize<InstanciaAplicacion>(JsonAPIDefaults());
        var temp = await this.Insertar(add);
        RespuestaPayload<object> respuesta = JsonSerializer.Deserialize<RespuestaPayload<object>>(JsonSerializer.Serialize(temp));
        return respuesta;
    }

    public async Task<Respuesta> ActualizarAPI(object id, JsonElement data)
    {
        var update = data.Deserialize<InstanciaAplicacion>(JsonAPIDefaults());
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
        var temp = await UnicaPorIdDespliegue((string)id);

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
    public override async Task<ResultadoValidacion> ValidarInsertar(InstanciaAplicacion data)
    {
        ResultadoValidacion resultado = new();
        resultado.Valido = true;

        return resultado;
    }
    public override async Task<ResultadoValidacion> ValidarEliminacion(string id, InstanciaAplicacion original)
    {
        ResultadoValidacion resultado = new();
        resultado.Valido = true;
        return resultado;
    }

    public override async Task<ResultadoValidacion> ValidarActualizar(string id, InstanciaAplicacion actualizacion, InstanciaAplicacion original)
    {
        ResultadoValidacion resultado = new();

        resultado.Valido = true;

        return resultado;
    }

    public override InstanciaAplicacion ADTOFull(InstanciaAplicacion actualizacion, InstanciaAplicacion actual)
    {
        actual.DominioId = actualizacion.DominioId;
        actual.ApplicacionId = actualizacion.ApplicacionId;
        actual.RolesPersonalizados = actualizacion.RolesPersonalizados;
        actual.MiembrosRol = actualizacion.MiembrosRol;
        actual.MiembrosPermiso = actualizacion.MiembrosPermiso;
        return actual;
    }

    public override InstanciaAplicacion ADTOFull(InstanciaAplicacion data)
    {
        InstanciaAplicacion instanciaAplicacion = new InstanciaAplicacion()
        {
            Id = Guid.NewGuid().ToString(),
            DominioId = data.DominioId,
            ApplicacionId = data.ApplicacionId,
            RolesPersonalizados=data.RolesPersonalizados,
            MiembrosPermiso=data.MiembrosPermiso,
            MiembrosRol=data.MiembrosRol
        };
        return instanciaAplicacion;
    }
    public override InstanciaAplicacion ADTODespliegue(InstanciaAplicacion data)
    {
        return new InstanciaAplicacion
        {
            Id=data.Id,
            DominioId = data.DominioId,
            ApplicacionId = data.ApplicacionId,
            RolesPersonalizados = data.RolesPersonalizados,
            MiembrosPermiso = data.MiembrosPermiso,
            MiembrosRol = data.MiembrosRol
        };
    }

    public override async Task<Respuesta> Actualizar(string id, InstanciaAplicacion data)
    {
        var respuesta = new Respuesta();
        try
        {
            if (string.IsNullOrEmpty(id.ToString()) || data == null)
            {
                respuesta.HttpCode = HttpCode.BadRequest;
                return respuesta;
            }

            InstanciaAplicacion actual = _dbSetFull.Find(Guid.Parse(id));

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


    public override async Task<RespuestaPayload<InstanciaAplicacion>> UnicaPorId(string id)
    {
        var respuesta = new RespuestaPayload<InstanciaAplicacion>();
        try
        {
            InstanciaAplicacion actual = await _dbSetFull.FindAsync(Guid.Parse(id));
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

            InstanciaAplicacion actual = _dbSetFull.Find(Guid.Parse(id));
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

    public async Task<List<Rol>> GetRolesUsuarioInterno(string aplicacionId, string usuarioId, string dominioId, string uOrgID)
    {
        List<Rol> roles = new List<Rol>();

        InstanciaAplicacion instanciaAplicacion = await _dbSetFull.FirstOrDefaultAsync(_=>_.ApplicacionId==aplicacionId && dominioId==dominioId);
        var aplicacionResult = await servicioAplicacion.UnicaPorId(aplicacionId);

        if (aplicacionResult.Ok && instanciaAplicacion != null)
        {
            var aplicacion = (Aplicacion)aplicacionResult.Payload;
            List<string> rolesId= instanciaAplicacion.MiembrosRol.Where(_ => _.UsuarioId.Any(u => u == usuarioId)).Select(_ => _.RolId).ToList();

            foreach (var modulo in aplicacion.Modulos)
            {
               var rol = modulo.RolesPredefinidos.Where(_ => rolesId.Contains(_.RolId)).ToList();
               roles.AddRange(rol);
            }
        }        
        return roles;
    }

    public async Task<List<Permiso>> GetPermisosAplicacionInterno(string aplicacionId, string usuarioId, string dominioId, string uOrgID)
    {
        List<Permiso> permisos = new List<Permiso>();
        InstanciaAplicacion instanciaAplicacion = await _dbSetFull.FirstOrDefaultAsync(_ => _.ApplicacionId == aplicacionId && dominioId == dominioId);
        var aplicacionResult = await servicioAplicacion.UnicaPorId(aplicacionId);

        if (aplicacionResult.Ok && instanciaAplicacion!=null)
        {
            var aplicacion = (Aplicacion)aplicacionResult.Payload;
            List<string> permisosId = instanciaAplicacion.MiembrosPermiso.Where(_ => _.UsuarioId.Any(u => u == usuarioId)).Select(_ => _.PermisoId).ToList();
            foreach (var modulo in aplicacion.Modulos)
            {
                var rol = modulo.Permisos.Where(_ =>permisosId.Contains(_.PermisoId)).ToList();
                permisos.AddRange(rol);
            }
        }
        return permisos;
    }
    #endregion
}
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
#pragma warning restore CS8603 // Possible null reference return.
