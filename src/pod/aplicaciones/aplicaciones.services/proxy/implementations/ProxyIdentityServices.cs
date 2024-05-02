using aplicaciones.model.aplicaciones;
using aplicaciones.model.invitaciones;
using comunes.interservicio.primitivas;
using comunes.primitivas;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace aplicaciones.services.proxy.implementations;


/// <summary>
/// Agrupa las llamadas remotas al servicio de identidad
/// </summary>
public class ProxyIdentityServices: IProxyIdentityServices
{
    private readonly ILogger<ProxyIdentityServices> logger;
    private readonly IServicioAutenticacionJWT autenticacionJWT;
    private readonly ConfiguracionAPI configuracionAPI;
    private readonly HttpClient identityHttpClient;
    public ProxyIdentityServices(ILogger<ProxyIdentityServices> logger, IServicioAutenticacionJWT autenticacionJWT, 
        IHttpClientFactory httpClientFactory, IOptions<ConfiguracionAPI> options) { 
        this.logger = logger;
        this.autenticacionJWT = autenticacionJWT;
        configuracionAPI = options.Value;
        identityHttpClient = httpClientFactory.CreateClient("identity");
    }


    public async Task<Respuesta> CreaUsuario(RegisterViewModel model)
    {
        Respuesta respuesta = new Respuesta();
        try
        {
            logger.LogDebug("ProxyIdentityServices - Creando usuario");
            var host = configuracionAPI.ObtieneHost("identity");
            if(host== null)
            {
                respuesta.Error = new ErrorProceso() { Mensaje = $"ProxyIdentityServices - Host identity no configurado", Codigo = "", HttpCode = HttpCode.UnprocessableEntity } ;
            } else
            {
                TokenJWT? jwt = null;
                if (string.IsNullOrEmpty(host.ClaveAutenticacion))
                {
                    respuesta.Error = new ErrorProceso() { Mensaje = $"ProxyIdentityServices - No hay una clave de autenticacion definida para identity", Codigo = "", HttpCode = HttpCode.UnprocessableEntity };
                }
                else {
                    jwt = await autenticacionJWT!.TokenInterproceso(host.ClaveAutenticacion);
                    if (jwt == null)
                    {
                        logger.LogDebug("ProxyIdentityServices - Error al obtener el token interservicio de JWT para Identity");
                    } else
                    {
                        identityHttpClient.BaseAddress = new Uri(host.UrlBase.TrimEnd('/'));
                        logger.LogDebug($"ProxyIdentityServices - LLamado remoto a {Path.Combine(identityHttpClient.BaseAddress.ToString(), "/account/Register")}");

                        var payload = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");
                        identityHttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt.access_token);
                        var response = await identityHttpClient.PostAsync($"/account/Register", payload);

                        logger.LogDebug($"ProxyIdentityServices - Respuesta {response.StatusCode} {response.ReasonPhrase}");

                        string? contenidoRespuesta = await response.Content.ReadAsStringAsync();

                        if (response.IsSuccessStatusCode)
                        {
                            respuesta.Ok = true;

                        } else
                        {
                            respuesta.Error = new ErrorProceso() { Mensaje = $"ProxyIdentityServices - error llamaa remota {response.ReasonPhrase} {contenidoRespuesta}", Codigo = "", HttpCode = (HttpCode)response.StatusCode };

                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, $"ProxyIdentityServices - Error al crear usuario {ex.Message}");

            respuesta.Error = new ErrorProceso() { Mensaje = $"{ex.Message}", Codigo = "", HttpCode = HttpCode.ServerError };
        }

        return respuesta;
    }

    public async Task<Respuesta> EstablecePasswordToken(ActualizarContrasena actualizarContrasena)
    {
        Respuesta respuesta = new Respuesta();
        try
        {
            logger.LogDebug("ProxyIdentityServices - Password Token");
            var host = configuracionAPI.ObtieneHost("Identity");
            if (host == null)
            {
                respuesta.Error = new ErrorProceso() { Mensaje = $"ProxyIdentityServices - Host Identity no configurado", Codigo = "", HttpCode = HttpCode.UnprocessableEntity };
            }
            else
            {
                TokenJWT? jwt = null;
                if (string.IsNullOrEmpty(host.ClaveAutenticacion))
                {
                    respuesta.Error = new ErrorProceso() { Mensaje = $"ProxyIdentityServices - No hay una clave de autencicacion definida para Identity", Codigo = "", HttpCode = HttpCode.UnprocessableEntity };
                }
                else
                {
                    jwt = await autenticacionJWT!.TokenInterproceso(host.ClaveAutenticacion);
                    if (jwt == null)
                    {
                        logger.LogDebug("ProxyIdentityServices - Error al obtener el token interservicio de JWT para Identity");
                    }
                    else
                    {
                        identityHttpClient.BaseAddress = new Uri(host.UrlBase.TrimEnd('/'));
                        logger.LogDebug($"ProxyIdentityServices - LLamado remoto a {Path.Combine(identityHttpClient.BaseAddress.ToString(), "/account/password/token")}");

                        var payload = new StringContent(JsonConvert.SerializeObject(actualizarContrasena), Encoding.UTF8, "application/json");
                        identityHttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt.access_token);
                        var response = await identityHttpClient.PostAsync($"/account/password/token", payload);
                        logger.LogDebug($"ProxyIdentityServices - Respuesta {response.StatusCode} {response.ReasonPhrase}");

                        string? contenidoRespuesta = await response.Content.ReadAsStringAsync();

                        if (response.IsSuccessStatusCode)
                        {
                            respuesta.Ok = true;

                        }
                        else
                        {
                            respuesta.Error = new ErrorProceso() { Mensaje = $"ProxyIdentityServices - error llamaa remota {response.ReasonPhrase} {contenidoRespuesta}", Codigo = "", HttpCode = (HttpCode)response.StatusCode };

                        }
                    }
                }
            }
        }
        catch(Exception ex) 
        {
            logger.LogError(ex, $"ProxyIdentityServices - Error al enviar correo {ex.Message}");
            respuesta.Error = new ErrorProceso() { Mensaje = $"{ex.Message}", Codigo = "", HttpCode = HttpCode.ServerError };
        }
        return respuesta;
    }

    public async Task<RespuestaPayload<string>> RecuperaPasswordEmail(string email)
    {
        RespuestaPayload<string> respuesta = new RespuestaPayload<string>();
        try
        {
            logger.LogDebug("ProxyIdentityServices - Recupera Password Email");
            var host = configuracionAPI.ObtieneHost("Identity");
            if (host == null)
            {
                respuesta.Error = new ErrorProceso() { Mensaje = $"ProxyIdentityServices - Host Identity no configurado", Codigo = "", HttpCode = HttpCode.UnprocessableEntity };
            }
            else
            {
                TokenJWT? jwt = null;
                if (string.IsNullOrEmpty(host.ClaveAutenticacion))
                {
                    respuesta.Error = new ErrorProceso() { Mensaje = $"ProxyIdentityServices - No hay una clave de autencicacion definida para Identity", Codigo = "", HttpCode = HttpCode.UnprocessableEntity };
                }
                else
                {
                    jwt = await autenticacionJWT!.TokenInterproceso(host.ClaveAutenticacion);
                    if (jwt == null)
                    {
                        logger.LogDebug("ProxyIdentityServices - Error al obtener el token interservicio de JWT para Identity");
                    }
                    else
                    {
                        identityHttpClient.BaseAddress = new Uri(host.UrlBase.TrimEnd('/'));
                        logger.LogDebug($"ProxyIdentityServices - LLamado remoto a {Path.Combine(identityHttpClient.BaseAddress.ToString(), "/account/password/recuperar")}");

                        var payload = new StringContent(JsonConvert.SerializeObject(email), Encoding.UTF8, "application/json");
                        identityHttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt.access_token);
                        var response = await identityHttpClient.GetAsync($"/account/password/recuperar?email="+email);

                        logger.LogDebug($"ProxyIdentityServices - Respuesta {response.StatusCode} {response.ReasonPhrase}");

                        string? contenidoRespuesta = await response.Content.ReadAsStringAsync();

                        var info = JsonConvert.DeserializeObject<DTORecuperacionPassword>(contenidoRespuesta);

                        if (response.IsSuccessStatusCode)
                        {
                            respuesta.Ok = true;
                            respuesta.Payload = info;
                        }
                        else
                        {
                            respuesta.Error = new ErrorProceso() { Mensaje = $"ProxyIdentityServices - error llamaa remota {response.ReasonPhrase} {contenidoRespuesta}", Codigo = "", HttpCode = (HttpCode)response.StatusCode };

                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, $"ProxyIdentityServices - Error al enviar correo {ex.Message}");
            respuesta.Error = new ErrorProceso() { Mensaje = $"{ex.Message}", Codigo = "", HttpCode = HttpCode.ServerError };
        }
        return respuesta;
    }
}
