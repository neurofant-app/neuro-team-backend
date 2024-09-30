#pragma warning disable CS8603 // Possible null reference return.
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
using apigenerica.model.interpretes;
using apigenerica.model.modelos;
using apigenerica.model.reflectores;
using apigenerica.model.servicios;
using comunes.primitivas;
using extensibilidad.metadatos;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using seguridad.modelo;
using seguridad.modelo.instancias;
using seguridad.modelo.servicios;
using System.Collections.Specialized;
using System.Text.Json;


namespace seguridad.servicios.mysql;
[ServicioEntidadAPI(entidad: typeof(InstanciaAplicacion),driver: Constantes.MYSQL)]
public class ServicioInstanciaAplicacionMySql : ServicioEntidadGenericaBase<InstanciaAplicacion, InstanciaAplicacion, InstanciaAplicacion, InstanciaAplicacion, string>,
    IServicioEntidadAPI, IServicioInstanciaAplicacion
{
    private readonly ILogger _logger;
    private readonly IReflectorEntidadesAPI reflector;
    private readonly IDistributedCache cache;
    private readonly IServicioAplicacion servicioAplicacion;
    private readonly IConfiguration configuration;

    public ServicioInstanciaAplicacionMySql(DBContextMySql contex, ILogger<ServicioInstanciaAplicacionMySql> logger,
        IReflectorEntidadesAPI Reflector, IDistributedCache cache, IServicioAplicacion servicioAplicacion, IConfiguration configuration) : base(contex, contex.InstanciaAplicacion , logger, Reflector, cache)
    {
        _logger = logger;
        reflector = Reflector;
        this.cache = cache;
        this.servicioAplicacion = servicioAplicacion;
        this.configuration = configuration;
        interpreteConsulta = new InterpreteConsultaExpresiones();
    }

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

    public async Task<RespuestaPayload<object>> InsertarAPI(JsonElement data, StringDictionary? parametros = null)
    {
        var add = data.Deserialize<InstanciaAplicacion>(JsonAPIDefaults());
        var temp = await this.Insertar(add);
        RespuestaPayload<object> respuesta = JsonSerializer.Deserialize<RespuestaPayload<object>>(JsonSerializer.Serialize(temp));
        return respuesta;
    }

    public async Task<Respuesta> ActualizarAPI(object id, JsonElement data, StringDictionary? parametros = null)
    {
        var update = data.Deserialize<InstanciaAplicacion>(JsonAPIDefaults());
        return await this.Actualizar((string)id, update);
    }

    public async Task<Respuesta> EliminarAPI(object id, JsonElement data, StringDictionary? parametros = null)
    {
        return await this.Eliminar((string)id);
    }

    public async Task<RespuestaPayload<object>> UnicaPorIdAPI(object id, StringDictionary? parametros = null)
    {
        var temp = await this.UnicaPorId((string)id);
        RespuestaPayload<object> respuesta = JsonSerializer.Deserialize<RespuestaPayload<object>>(JsonSerializer.Serialize(temp));
        return respuesta;
    }

    public async Task<RespuestaPayload<object>> UnicaPorIdDespliegueAPI(object id, StringDictionary? parametros = null)
    {
        var temp = await UnicaPorIdDespliegue((string)id);

        RespuestaPayload<object> respuesta = JsonSerializer.Deserialize<RespuestaPayload<object>>(JsonSerializer.Serialize(temp));
        return respuesta;
    }

    public async Task<RespuestaPayload<PaginaGenerica<object>>> PaginaAPI(Consulta consulta, StringDictionary? parametros = null)
    {
        var temp = await this.Pagina(consulta);
        RespuestaPayload<PaginaGenerica<object>> respuesta = JsonSerializer.Deserialize<RespuestaPayload<PaginaGenerica<object>>>(JsonSerializer.Serialize(temp));

        return respuesta;
    }

    public async Task<RespuestaPayload<PaginaGenerica<object>>> PaginaDespliegueAPI(Consulta consulta, StringDictionary? parametros = null)
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
        actual.PermisoGrupo = actualizacion.PermisoGrupo;
        actual.PermisoUsuarios = actualizacion.PermisoUsuarios;
        actual.RolGrupo = actualizacion.RolGrupo;
        actual.RolUsuarios = actualizacion.RolUsuarios;
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
            PermisoGrupo=data.PermisoGrupo,
            PermisoUsuarios=data.PermisoUsuarios,
            RolGrupo=data.RolGrupo,
            RolUsuarios=data.RolUsuarios,
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
            RolGrupo = data.RolGrupo,
            RolUsuarios = data.RolUsuarios,
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

    public override async Task<Respuesta> Eliminar(string id, StringDictionary? parametros = null)
    {
        var respuesta = new Respuesta();
        try
        {

            if (string.IsNullOrEmpty(id))
            {
                respuesta.HttpCode = HttpCode.BadRequest;
                return respuesta;
            }

            InstanciaAplicacion actual = _dbSetFull.Find(id);
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
        var rolesCache = _cache.GetString($"roles-{usuarioId}");             

        if (string.IsNullOrEmpty(rolesCache))
        {
            InstanciaAplicacion instanciaAplicacion = await _dbSetFull.Include(_ => _.RolUsuarios).FirstOrDefaultAsync(_ => _.ApplicacionId == Guid.Parse(aplicacionId) && dominioId == dominioId);
            var aplicacionResult = await servicioAplicacion.UnicaPorId(aplicacionId);

            if (aplicacionResult.Ok && instanciaAplicacion != null)
            {
                var aplicacion = (Aplicacion)aplicacionResult.Payload;
                List<string> rolesId = instanciaAplicacion.RolUsuarios.Where(_ => _.UsuarioId==usuarioId).Select(_ => _.RolId).ToList();
                if (rolesId.Any())
                {
                    foreach (var modulo in aplicacion.Modulos)
                    {
                        var rol = modulo.RolesPredefinidos.Where(_ => rolesId.Contains(_.RolId)).ToList();
                        roles.AddRange(rol);
                    }
                    var expiraEn = configuration.GetValue<double>("CacheConfig:TiempoExpira");
                    _cache.SetString(usuarioId, JsonSerializer.Serialize(roles),
                        new DistributedCacheEntryOptions()
                        {
                            SlidingExpiration = TimeSpan.FromSeconds(expiraEn)
                        });
                }
           
            }

        }
        else
        {
            roles =JsonSerializer.Deserialize<List<Rol>>(rolesCache);
        }
        return roles;
    }

    public async Task<List<Permiso>> GetPermisosAplicacionInterno(string aplicacionId, string usuarioId, string dominioId, string uOrgID)
    {
        List<Permiso> permisos = new List<Permiso>();
        var rolesCache = _cache.GetString($"permisos-{usuarioId}");

        if (string.IsNullOrEmpty(rolesCache))
        {
            InstanciaAplicacion instanciaAplicacion = await _dbSetFull.Include(_=>_.PermisoUsuarios).FirstOrDefaultAsync(_ => _.ApplicacionId == Guid.Parse(aplicacionId) && dominioId == dominioId);
            var aplicacionResult = await servicioAplicacion.UnicaPorId(aplicacionId);

            if (aplicacionResult.Ok && instanciaAplicacion != null)
            {
                var aplicacion = (Aplicacion)aplicacionResult.Payload;
                List<string> permisosId = instanciaAplicacion.PermisoUsuarios.Where(_ => _.UsuarioId == usuarioId).Select(_ => _.PermisoId).ToList();
                if(permisosId.Any())
                {
                    foreach (var modulo in aplicacion.Modulos)
                    {
                        var rol = modulo.Permisos.Where(_ => permisosId.Contains(_.PermisoId)).ToList();
                        permisos.AddRange(rol);
                    }
                    var expiraEn = configuration.GetValue<double>("CacheConfig:TiempoExpira");
                    _cache.SetString(usuarioId, JsonSerializer.Serialize(permisos),
                        new DistributedCacheEntryOptions()
                        {
                            SlidingExpiration = TimeSpan.FromSeconds(expiraEn)
                        });
                }
            }
        }
        else
        {
            permisos = JsonSerializer.Deserialize<List<Permiso>>(rolesCache);
        }
        return permisos;
    }
    #endregion
}
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
#pragma warning restore CS8603 // Possible null reference return.
