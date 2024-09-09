using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using comunes.primitivas;
using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;
using comunes.interservicio.primitivas.extensiones;

namespace comunes.interservicio.primitivas.espaciotrabajo;

public class ProxyEspecioTrabajo : IProxyEspacioTrabajo
{
    private const string ESPACIOS_TRABAJOS_KEY = "ESPACIOS-TRABAJO-KEY";
    private readonly ILogger<ProxyEspecioTrabajo> logger;
    private readonly IServicioAutenticacionJWT autenticacionJWT;
    private readonly IDistributedCache _cache;
    private readonly ConfiguracionAPI configuracionAPI;
    private readonly HttpClient espacioTrabajoHttpClient;
    private readonly HostInterServicio host;
    
    public ProxyEspecioTrabajo(IOptions<ConfiguracionAPI> options, ILogger<ProxyEspecioTrabajo> logger, IServicioAutenticacionJWT autenticacionJWT,
        IHttpClientFactory httpClientFactory, IDistributedCache cache)
    {
        configuracionAPI = options.Value;
        this.host = configuracionAPI.ObtieneHost("espacioTrabajo");
        this.logger = logger;
        this.autenticacionJWT = autenticacionJWT;
        this._cache = cache;
        espacioTrabajoHttpClient = httpClientFactory.CreateClient();
        espacioTrabajoHttpClient.DefaultRequestHeaders.Add("Accept-Language", "es-MX");
    }

    private void ActualizaHeaders(string dominioId, string unidadOrgId)
    {
        espacioTrabajoHttpClient.DefaultRequestHeaders.Remove("x-d-id");
        espacioTrabajoHttpClient.DefaultRequestHeaders.Remove("x-uo-id");
        espacioTrabajoHttpClient.DefaultRequestHeaders.Add("x-d-id", dominioId);
        espacioTrabajoHttpClient.DefaultRequestHeaders.Add("x-uo-id", unidadOrgId);
    }

    public async Task<RespuestaPayload<List<EspacioTrabajoUsuario>>> EspacioTrabajoUsuario(string UsuarioId)
    {
        RespuestaPayload<List<EspacioTrabajoUsuario>> respuestaPayload = new RespuestaPayload<List<EspacioTrabajoUsuario>>();
        List<EspacioTrabajoUsuario> espacioTrabajoUsuarios = new();
        logger.LogDebug("ProxyEspecioTrabajo- Obteniendo EspaciosUsuario");

        if (host == null)
        {
            logger.LogError($"ProxyEspecioTrabajo - Host espacioTrabajo no configurado");
        }
        ActualizaHeaders("mi-dominio", "x-uo-id");
        try
        {

            var espaciosCache = _cache.GetString(ESPACIOS_TRABAJOS_KEY);

            if (espaciosCache != null)
            {
                espacioTrabajoUsuarios = JsonConvert.DeserializeObject<List<EspacioTrabajoUsuario>>(espaciosCache);
                respuestaPayload.Ok = true;
                respuestaPayload.Payload = espacioTrabajoUsuarios;
            }


            TokenJWT? jWT = null;
            if (string.IsNullOrEmpty(host.ClaveAutenticacion))
            {
                logger.LogError($"ProxyEspecioTrabajo - No hay una clave de autenticacion definida para EspacioTrabajo");
            }
            else
            {
                jWT = await autenticacionJWT!.TokenInterproceso(host.ClaveAutenticacion);
                if (jWT == null)
                {
                    logger.LogDebug("ProxyEspecioTrabajo - Error al obtener el token interservicio de JWT para EspacioTrabajo");
                }
                else
                {
                    logger.LogDebug($"ProxyEspecioTrabajo - Llamado remoto a {Path.Combine($"{host.UrlBase}/espacioTrabajo/usuario/{UsuarioId}")}");
                    espacioTrabajoHttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jWT.access_token);
                    var response = await espacioTrabajoHttpClient.GetAsync($"{host.UrlBase}/espaciotrabajo/usuario/{UsuarioId}");
                    logger.LogDebug($"ProxyEspecioTrabajo - Respuesta {response.StatusCode} {response.ReasonPhrase}");

                    string? contenidoRespuesta = await response.Content.ReadAsStringAsync();
                    

                    if (!response.IsSuccessStatusCode)
                    {
                        logger.LogError($"ProxyEspecioTrabajo - error llamada remota {response.ReasonPhrase} {contenidoRespuesta}");
                    }

                    espacioTrabajoUsuarios = JsonConvert.DeserializeObject<List<EspacioTrabajoUsuario>>(contenidoRespuesta);
                    _cache.SetString(ESPACIOS_TRABAJOS_KEY, JsonConvert.SerializeObject(espacioTrabajoUsuarios),
                        new DistributedCacheEntryOptions()
                        {
                            SlidingExpiration = TimeSpan.FromMinutes(5)
                        }); 
                    respuestaPayload.Ok = true;
                    respuestaPayload.Payload = espacioTrabajoUsuarios;
                }
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, $"ProxyEspecioTrabajo - al Obtener EspacioTrabajo {ex.Message}");
        }
        return respuestaPayload;
    }

    public Task<bool> EspacioTrabajoValidoUsuario(string UsuarioId, Guid EspacioTrabajoId)
    {
        throw new NotImplementedException();
    }


}
