using comunes.primitivas;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace comunes.interservicio.primitivas.identidad;

public class ProxyIdentidad : IProxyIdentidad
{
    private readonly ILogger<ProxyIdentidad> _logger;
    private readonly IServicioAutenticacionJWT autenticacionJWT;
    private readonly ConfiguracionAPI configuracionAPI;
    private readonly HttpClient identidadHttpClient;
    private readonly HostInterServicio host;

    public ProxyIdentidad(IOptions<ConfiguracionAPI> options, ILogger<ProxyIdentidad> logger, IServicioAutenticacionJWT autenticacionJWT, IHttpClientFactory httpClientFactory)
    {
        configuracionAPI = options.Value;
        this.host = configuracionAPI.ObtieneHost("Identity");
        this._logger = logger;
        this.autenticacionJWT = autenticacionJWT;
        identidadHttpClient = httpClientFactory.CreateClient();
    }

    public async Task<bool> ExisteUsuarioId(string UsuarioId)
    {
        bool existeUsuario = false;
        _logger.LogDebug("ProxyIdentidad - ExisteUsuarioId {UsuarioId}", UsuarioId);
        if (host == null) _logger.LogError("ProxyIdentidad - ExisteUsuarioId {UsuarioId}", UsuarioId);
        try
        {
            TokenJWT? jWT = null;
            if (string.IsNullOrEmpty(host.ClaveAutenticacion))
            {
                _logger.LogError($"ProxyIdentidad - No hay una clase de autenticación definida para Identidad");
                return existeUsuario;
            }
            else
            {
                jWT = await autenticacionJWT!.TokenInterproceso(host.ClaveAutenticacion);
                if (jWT == null)
                {
                    _logger.LogDebug($"ProxyIdentidad - Error al obtener el token interservicio de JWT cache de seguridad");
                    return existeUsuario;
                }
                else
                { 
                    _logger.LogDebug($"ProxyIdentidad - Llamado remoto a {Path.Combine($"{host.UrlBase}/controlacceso/usuario/{UsuarioId}")}");
                    identidadHttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jWT.access_token);
                    var response = await identidadHttpClient.PostAsync($"{host.UrlBase}/account/usuario/{UsuarioId}", null);
                    _logger.LogDebug($"ProxyIdentidad - Respuesta {response.StatusCode} {response.ReasonPhrase}");

                    string? contenidoRespuesta = await response.Content.ReadAsStringAsync();

                    if(!response.IsSuccessStatusCode)
                    {
                        _logger.LogError($"ProxyIdentidad - error llamada remota {response.ReasonPhrase} {contenidoRespuesta}");
                        return existeUsuario;
                    }

                    existeUsuario = true;
                }  
            }

        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "ProxyIdentidad - al obtener Seguridad el cahe {msg}", ex.Message);
        }
        return existeUsuario;
    }
}
