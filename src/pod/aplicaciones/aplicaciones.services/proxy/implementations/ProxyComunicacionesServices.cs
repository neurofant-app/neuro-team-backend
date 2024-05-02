using api.comunicaciones;
using aplicaciones.services.proxy.abstractions;
using comunes.interservicio.primitivas;
using comunes.primitivas;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace aplicaciones.services.proxy.implementations;

public class ProxyComunicacionesServices : IProxyComunicacionesServices
{
    private readonly ILogger<ProxyIdentityServices> logger;
    private readonly IServicioAutenticacionJWT autenticacionJWT;
    private readonly ConfiguracionAPI configuracionAPI;
    private readonly HttpClient comunicacionesHttpClient;

    public ProxyComunicacionesServices(ILogger<ProxyIdentityServices> logger, IServicioAutenticacionJWT autenticacionJWT,
        IHttpClientFactory httpClientFactory, IOptions<ConfiguracionAPI> options) 
    {
        this.logger = logger;
        this.autenticacionJWT = autenticacionJWT;
        configuracionAPI = options.Value;
        comunicacionesHttpClient = httpClientFactory.CreateClient("comunicaciones");
    }

    public async Task<Respuesta> EnviarCorreo(MensajeEmail msj)
    {
        Respuesta respuesta = new Respuesta();
        try
        {
            logger.LogDebug("ProxyComunicacionesServices - Envio de Correo");
            var host = configuracionAPI.ObtieneHost("comunicaciones");
            if (host == null)
            {
                respuesta.Error = new ErrorProceso() { Mensaje = $"ProxyComunicacionesServies - Host Comunicaciones no configurado", Codigo = "", HttpCode = HttpCode.UnprocessableEntity };
            }
            else
            {
                TokenJWT? jWT = null;
                if (string.IsNullOrEmpty(host.ClaveAutenticacion))
                {
                    respuesta.Error = new ErrorProceso() { Mensaje = $"ProxyComunicacionesServices - No hay una clave de autencicacion definida para Comunicaciones", Codigo = "", HttpCode = HttpCode.UnprocessableEntity };
                }
                else
                {
                    jWT = await autenticacionJWT!.TokenInterproceso(host.ClaveAutenticacion);
                    if(jWT == null)
                    {
                        logger.LogDebug("ProxyComunicacionesServices - Error al obtener el token interservicio de JWT para Comunicaciones");
                    }
                    else
                    {
                        comunicacionesHttpClient.BaseAddress = new Uri(host.UrlBase.TrimEnd('/'));
                        logger.LogDebug($"ProxyComunicacionesServices - Llamado remoto a {Path.Combine(comunicacionesHttpClient.BaseAddress.ToString(), "/email/EnviarCorreo")}");
                        var payload = new StringContent(JsonConvert.SerializeObject(msj), Encoding.UTF8, "application/json");
                        comunicacionesHttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jWT.access_token);
                        var response = await comunicacionesHttpClient.PostAsync($"/email/EnviarCorreo", payload);
                        logger.LogDebug($"ProxyComunicacionesServices - Respuesta {response.StatusCode} {response.ReasonPhrase}");

                        string? contenidoRespuesta = await response.Content.ReadAsStringAsync();

                        if(response.IsSuccessStatusCode)
                        {
                            respuesta.Ok = true;
                        }
                        else
                        {
                            respuesta.Error = new ErrorProceso() { Mensaje = $"ProxyComunicacionesServices - error llamaa remota {response.ReasonPhrase} {contenidoRespuesta}", Codigo = "", HttpCode = (HttpCode)response.StatusCode };

                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, $"ProxyComunicacionesServices - Error al enviar correo {ex.Message}");
            respuesta.Error = new ErrorProceso() { Mensaje = $"{ex.Message}", Codigo = "", HttpCode = HttpCode.ServerError };
        }

        return respuesta;
    }
}
