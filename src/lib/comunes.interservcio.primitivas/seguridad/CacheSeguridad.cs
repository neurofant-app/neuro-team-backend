using comunes.primitivas.seguridad;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using comunes.interservicio.primitivas.extensiones;
using apigenerica.primitivas.seguridad;

namespace comunes.interservicio.primitivas.seguridad;

/// <summary>
/// Mantiene un cache de seguridad y control de acceso en el servicio
/// </summary>
public class CacheSeguridad : ICacheSeguridad
{
    private readonly IProxySeguridad proxySeguridad;
    private readonly IDistributedCache cache;
    private readonly DistributedCacheEntryOptions cacheOptions;
 
    public CacheSeguridad(IProxySeguridad proxySeguridad, IDistributedCache cache, IConfiguration configuration) {
        this.proxySeguridad = proxySeguridad;
        this.cache = cache;

        // VErificar si esto funciona, debe dejar 5 minutos por default si no existe la entrada MinutosCacheSeguridad en el appsettings
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
    
    public Task<Permiso> PermisosUsuario(string appId, string usuarioId, string dominioId, string unidadOrgId)
    {
        string clave = GeneraClaveCache(appId, usuarioId, "permisos");

        // VErificar si existe la entrada en cache con la clave
        // SI existe regresar el valor
        // SI no existe llamar al proxy y almcenar el resultado (aun cuando sea una lista vacia)
        //           Almacenar elresultado en el cache con algo simila a
        //
        //          cache.Set(clave, objeto.ToByteArray(), cacheOptions);



        throw new NotImplementedException();
    }


    public Task<Rol> RolesUsuario(string appId, string usuarioId, string dominioId, string unidadOrgId)
    {
        string clave = GeneraClaveCache(appId, usuarioId, "roles");

        // VErificar si existe la entrada en cache con la clave
        // SI existe regresar el valor
        // SI no existe llamar al proxy y almcenar el resultado (aun cuando sea una lista vacia)
        //           Almacenar elresultado en el cache con algo simila a
        //
        //          cache.Set(clave, objeto.ToByteArray(), cacheOptions);


        throw new NotImplementedException();
    }
}
