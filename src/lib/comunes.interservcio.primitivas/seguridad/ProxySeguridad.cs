using comunes.interservicio.primitivas;
using comunes.primitivas.seguridad;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json.Nodes;

namespace comunes.interservicio.primitivas;

public class ProxySeguridad:IProxySeguridad
{
    private readonly ILogger<ProxySeguridad> logger;
    private readonly IServicioAutenticacionJWT autenticacionJWT;
    private readonly ConfiguracionAPI configuracionAPI;
    private readonly HttpClient seguridadHttpClient;

    public ProxySeguridad(ILogger<ProxySeguridad> logger, IServicioAutenticacionJWT autenticacionJWT,
        IHttpClientFactory httpClientFactory, IOptions<ConfiguracionAPI> options)
    {
        this.logger =logger;
        this.autenticacionJWT = autenticacionJWT;
        configuracionAPI = options.Value;
        seguridadHttpClient = new HttpClient();
    }


    public async Task ActualizaSeguridad(List<Aplicacion> apps)
    {
        logger.LogDebug("ProxySeguridad- Actualizando Permisos");
        var host = configuracionAPI.ObtieneHost("seguridad");
        if (host == null)


        {
            logger.LogError($"ProxySeguridad - Host seguridad no configurado");
        }
        seguridadHttpClient.BaseAddress = new Uri(host.UrlBase.TrimEnd('/'));
        seguridadHttpClient.DefaultRequestHeaders.Add("x-d-id","mi-dominio");
        seguridadHttpClient.DefaultRequestHeaders.Add("x-uo-id","mi-ou");
        seguridadHttpClient.DefaultRequestHeaders.Add("Accept-Language", "es-MX");
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
                    logger.LogDebug("ProxyComunicacionesServices - Error al obtener el token interservicio de JWT para aplicaciones");
                }
                else
                {
                    foreach (var app in apps)
                    {
                        logger.LogDebug($"ProxySeguridad - Llamado remoto a {Path.Combine(seguridadHttpClient.BaseAddress.ToString(), $"/api/Aplicacion/Entidad/{app.ApplicacionId}")}");
                        var payload = new StringContent(JsonConvert.SerializeObject(app,new JsonSerializerSettings() { ContractResolver = new CamelCasePropertyNamesContractResolver() }), Encoding.UTF8, "application/json");
                        seguridadHttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jWT.access_token);
                        var response = await seguridadHttpClient.PutAsync($"api/Aplicacion/Entidad/{app.ApplicacionId}", payload);
                        logger.LogDebug($"ProxySeguridad - Respuesta {response.StatusCode} {response.ReasonPhrase}");

                        string? contenidoRespuesta = await response.Content.ReadAsStringAsync();

                        if (!response.IsSuccessStatusCode)
                         {
                        logger.LogError($"ProxyComunicacionesServices - error llamada remota {response.ReasonPhrase} {contenidoRespuesta}");
                         }          
                    }
                }
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, $"ProxyComunicacionesServices - Error al enviar correo {ex.Message}");
        }
        
    }
}
