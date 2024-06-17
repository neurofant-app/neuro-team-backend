using comunes.primitivas.seguridad;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using comunes.interservicio.primitivas.extensiones;
using apigenerica.primitivas.seguridad;
using Newtonsoft.Json;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Serialization;
using System.Net.Http.Headers;
using System.Text;
using Microsoft.Extensions.Logging;

namespace comunes.interservicio.primitivas.seguridad;

/// <summary>
/// Mantiene un cache de seguridad y control de acceso en el servicio
/// </summary>
public class CacheSeguridad : ICacheSeguridad
{
    private readonly IProxySeguridad proxySeguridad;
    private readonly IDistributedCache _cache;
    private readonly DistributedCacheEntryOptions cacheOptions;
 
    public CacheSeguridad (IProxySeguridad proxySeguridad, IDistributedCache cache, IConfiguration configuration) {

        this.proxySeguridad = proxySeguridad;
        this._cache = cache;
        var minutos = configuration.GetValue<int?>("MinutosCacheSeguridad");
        cacheOptions = (minutos ?? 5).ExpiraCacheEnMinutos();
    }

    /// <summary>
    /// Genera la clave de acceso al cache
    /// </summary>
    /// <param name="appId"></param>
    /// <param name="usuarioId"></param>
    /// <param name="clave"></param>
    /// <returns></returns>
    private string GeneraClaveCache(string appId, string usuarioId, string clave) => $"{appId}-{usuarioId}-{clave}";
    

    public async Task<List<Permiso>> PermisosUsuario(string appId, string usuarioId, string dominioId, string unidadOrgId)
    {
        string clave = GeneraClaveCache(appId, usuarioId, "permisos");
        List<Permiso> permisos = new();
        var permisoCache = _cache.GetString(clave);

        if (string.IsNullOrEmpty(permisoCache))
        {
            permisos = await proxySeguridad.PermisosUsuario(appId,usuarioId,dominioId,unidadOrgId);
            _cache.Set(usuarioId, permisos.ToByteArray(), cacheOptions);
        }
        else
        {
            permisos = JsonConvert.DeserializeObject<List<Permiso>>(permisoCache);
        }
        return permisos;

    }


    public async Task<List<Rol>> RolesUsuario(string appId, string usuarioId, string dominioId, string unidadOrgId)
    {
        string clave = GeneraClaveCache(appId, usuarioId, "roles");
        List<Rol> roles = new();
        var rolesCache = _cache.GetString(clave);

        if (string.IsNullOrEmpty(rolesCache))
        {
            roles = await proxySeguridad.RolesUsuario(appId, usuarioId, dominioId, unidadOrgId);
            _cache.Set(usuarioId, roles.ToByteArray(), cacheOptions);
        }
        else
        {
            roles = JsonConvert.DeserializeObject<List<Rol>>(rolesCache);
        }
        return roles;

    }
}
