using apigenerica.model.modelos;
using apigenerica.primitivas.seguridad;
using comunes.interservicio.primitivas.extensiones;
using comunes.primitivas.atributos;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Polly;
using System.Reflection;

namespace comunes.interservicio.primitivas.seguridad;

public class CacheAtributos: ICacheAtributos
{
    private readonly IDistributedCache _cache;
    private readonly DistributedCacheEntryOptions cacheOptions;

    public CacheAtributos(IDistributedCache cache, IConfiguration configuration)
    {
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
    private string GeneraClaveCache(string appId,string clave) => $"{appId}-{clave}";


    public async Task<List<AtributosMetodo>> AtributosServicio(Type tipoServicio)
    {
        List<AtributosMetodo> atributosFinal = new();
        if (tipoServicio != null)
        {
            string clave = GeneraClaveCache(tipoServicio.Name,"atributos");
 
            var atributosCache = _cache.GetString(clave);

            if (string.IsNullOrEmpty(atributosCache))
            {
                foreach (var metodo in tipoServicio.GetRuntimeMethods())
                {
                    if (metodo.CustomAttributes.Any())
                    {
                        var atrTemp = new List<string>();

                        foreach (var item in metodo.GetCustomAttributes())
                        {                           
                            if (item is RolAttribute r)
                            {
                                atrTemp.Add(r.RolId);
                            }
                            if (item is PermisoAttribute p)
                            {
                                atrTemp.Add(p.PermisoId);
                            }
                        }
                        atributosFinal.Add(
                                new AtributosMetodo()
                                {
                                    MetodoId = metodo.Name,
                                    atributosId = atrTemp
                                });                       
                        
                    }
                }
                _cache.Set(clave, atributosFinal.ToByteArray(), cacheOptions);
            }
            else
            {
                atributosFinal = JsonConvert.DeserializeObject<List<AtributosMetodo>>(atributosCache);
            }

        }
        return atributosFinal;
    }


}
