using comunes.primitivas.extensiones;
using comunes.primitivas.seguridad;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Serilog.Core;
using System.Net.Http.Headers;
using System.Text;


namespace comunes.interservicio.primitivas;

public class ProxySeguridad:IProxySeguridad
{
    private readonly ILogger<ProxySeguridad> logger;
    private readonly IServicioAutenticacionJWT autenticacionJWT;
    private readonly ConfiguracionAPI configuracionAPI;
    private readonly HttpClient seguridadHttpClient;
    private readonly HostInterServicio host;

    public ProxySeguridad(IOptions<ConfiguracionAPI> options,ILogger<ProxySeguridad> logger, IServicioAutenticacionJWT autenticacionJWT,
        IHttpClientFactory httpClientFactory)
    {
        configuracionAPI = options.Value;
        this.host = configuracionAPI.ObtieneHost("seguridad");
        this.logger =logger;
        this.autenticacionJWT = autenticacionJWT;
        seguridadHttpClient = httpClientFactory.CreateClient();
        seguridadHttpClient.DefaultRequestHeaders.Add("Accept-Language", "es-MX");
    }
    private void ActualizaHeaders(string dominioId, string unidadOrgId)
    {
        seguridadHttpClient.DefaultRequestHeaders.Remove("x-d-id");
        seguridadHttpClient.DefaultRequestHeaders.Remove("x-uo-id");
        seguridadHttpClient.DefaultRequestHeaders.Add("x-d-id", dominioId);
        seguridadHttpClient.DefaultRequestHeaders.Add("x-uo-id", unidadOrgId);
    }

    public async Task ActualizaSeguridad(List<Aplicacion> apps)
    {
        var logInfo = ExtensionesLog.InfoMetodo().PrefijoLog();
        if (host == null)
        {
            logger.LogError("{0} {1}", logInfo, "No hay configuración para el Host de seguridad");
            return;
        }

        
        object?[] objects = new object?[] { "A", DateTime.Now, apps };
        logger.PrefixDebug("ejemplo", objects);
        logger.PrefixDebug("MI texto");


        ActualizaHeaders("mi-dominio", "x-uo-id");
        try
        {
            object?[] param = new object?[] { "apps", "more" };
            

            TokenJWT? jWT = null;
            if (string.IsNullOrEmpty(host.ClaveAutenticacion))
            {
                logger.LogError($"ProxySeguridad - ActualizaSeguridad - No hay una clave de autencicacion definida para Seguridad");
            }
            else
            {
                jWT = await autenticacionJWT!.TokenInterproceso(host.ClaveAutenticacion);
                if (jWT == null)
                {
                    logger.LogDebug("ProxySeguridad - ActualizaSeguridad - Error al obtener el token interservicio de JWT para aplicaciones");
                }
                else
                {
                    string rutaAplicacion = $"{host.UrlBase}/entidad/aplicacion";
                    foreach (var app in apps)
                    {
                        string rutaAppId = $"{rutaAplicacion}/{app.ApplicacionId}";
                        logger.LogDebug($"ProxySeguridad - ActualizaSeguridad - Aplicacion {app.Nombre} {app.ApplicacionId}");
                        var payload = new StringContent(JsonConvert.SerializeObject(app,new JsonSerializerSettings() { ContractResolver = new CamelCasePropertyNamesContractResolver() }), Encoding.UTF8, "application/json");
                        logger.LogDebug($"ProxySeguridad - ActualizaSeguridad {0} {1}", rutaAppId, payload);
                        seguridadHttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jWT.access_token);
                        var response = await seguridadHttpClient.PutAsync(rutaAppId, payload);
                        logger.LogDebug($"ProxySeguridad - Respuesta {response.StatusCode} {response.ReasonPhrase}");

                        string? contenidoRespuesta = await response.Content.ReadAsStringAsync();

                        if (!response.IsSuccessStatusCode)
                         {
                        logger.LogError($"ProxySeguridad - error llamada remota {response.ReasonPhrase} {contenidoRespuesta}");
                         }          
                    }
                }
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, $"ProxySeguridad - al Actualizar Seguridad {ex.Message}");
        }
        
    }

    public async Task<List<Permiso>> PermisosUsuario(string appId, string usuarioId, string dominioId, string unidadOrgId)
    {
        List<Permiso> permisos= new();
        logger.LogDebug("ProxySeguridad- Obteniendo Permisos del cache");
        if (host == null) logger.LogError($"ProxySeguridad - Host seguridad no configurado");
        ActualizaHeaders(dominioId, unidadOrgId);
        try
        {
            TokenJWT? jWT = null;
            if (string.IsNullOrEmpty(host.ClaveAutenticacion))
            {
                logger.LogError($"ProxySeguridad - No hay una clave de autencicacion definida para Seguridad");
            }
            else
            {
                jWT = await autenticacionJWT!.TokenInterproceso(host.ClaveAutenticacion);
                if (jWT == null)
                {
                    logger.LogDebug("ProxySeguridad - Error al obtener el token interservicio de JWT cache de seguridad");
                }
                else
                {

                    logger.LogDebug($"ProxySeguridad - Llamado remoto a {Path.Combine($"{host.UrlBase}/controlacceso/interno/permisos/{appId}/{usuarioId}")}");
                   seguridadHttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jWT.access_token);
                    var response = await seguridadHttpClient.GetAsync($"{host.UrlBase}/controlacceso/interno/permisos/{appId}/{usuarioId}");
                    logger.LogDebug($"ProxySeguridad - Respuesta {response.StatusCode} {response.ReasonPhrase}");

                    string? contenidoRespuesta = await response.Content.ReadAsStringAsync();

                    if (!response.IsSuccessStatusCode)
                    {
                        logger.LogError($"ProxySeguridad - error llamada remota {response.ReasonPhrase} {contenidoRespuesta}");
                    }
                    permisos = JsonConvert.DeserializeObject<List<Permiso>>(contenidoRespuesta);
                }
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, $"ProxySeguridad - Error al obtener permisos edl Cache de Seguridad {ex.Message}");
        }
        return permisos;
    }

    public async Task<List<Rol>> RolesUsuario(string appId, string usuarioId, string dominioId, string unidadOrgId)
    {
        List<Rol>roles = new();
        logger.LogDebug("ProxySeguridad- obteniendo roles del cache");
        if (host == null)logger.LogError($"ProxySeguridad - Host seguridad no configurado");
        ActualizaHeaders(dominioId, unidadOrgId);
        try
        {
            TokenJWT? jWT = null;
            if (string.IsNullOrEmpty(host.ClaveAutenticacion))
            {
                logger.LogError($"ProxySeguridad - No hay una clave de autencicacion definida para Seguridad");
            }
            else
            {
                jWT = await autenticacionJWT!.TokenInterproceso(host.ClaveAutenticacion);
                if (jWT == null)
                {
                    logger.LogDebug("ProxySeguridad - Error al obtener el token interservicio de JWT para cache seguridad");
                }
                else
                {
                    logger.LogDebug($"ProxySeguridad - Llamado remoto a {Path.Combine($"{host.UrlBase}/controlacceso/interno/roles/{appId}/{usuarioId}")}");
                    seguridadHttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jWT.access_token);
                    var response = await seguridadHttpClient.GetAsync($"{host.UrlBase}/controlacceso/interno/roles/{appId}/{usuarioId}");
                    logger.LogDebug($"ProxySeguridad - Respuesta {response.StatusCode} {response.ReasonPhrase}");

                    string? contenidoRespuesta = await response.Content.ReadAsStringAsync();

                    if (!response.IsSuccessStatusCode)
                    {
                        logger.LogError($"ProxySeguridad - error llamada remota {response.ReasonPhrase} {contenidoRespuesta}");
                    }
                    roles = JsonConvert.DeserializeObject<List<Rol>>(contenidoRespuesta);
                }
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, $"ProxySeguridad - al obtener Seguridad el cache {ex.Message}");
        }
        return roles;
    }
}
