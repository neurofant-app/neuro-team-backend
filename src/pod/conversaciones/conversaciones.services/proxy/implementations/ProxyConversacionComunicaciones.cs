using Amazon.Runtime.Internal.Util;
using comunes.interservicio.primitivas;
using comunes.primitivas;
using comunicaciones.modelo;
using comunicaciones.modelo.whatsapp;
using conversaciones.model;
using conversaciones.services.proxy.abstractions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace conversaciones.services.proxy.implementations;

public class ProxyConversacionComunicaciones : IProxyConversacionComunicaciones
{
    private readonly ILogger<ProxyConversacionComunicaciones> logger;
    private readonly IServicioAutenticacionJWT autenticacionJWT;
    private readonly ConfiguracionAPI configuracionAPI;
    private readonly HttpClient comunicacionesHttpClient;

    public ProxyConversacionComunicaciones(ILogger<ProxyConversacionComunicaciones> logger, IServicioAutenticacionJWT autenticacionJWT,
        IHttpClientFactory httpClientFactory, IOptions<ConfiguracionAPI> options)
    {
        this.logger = logger;
        this.autenticacionJWT = autenticacionJWT;
        configuracionAPI = options.Value;
        comunicacionesHttpClient = httpClientFactory.CreateClient("comunicaciones");
    }

    public async Task<Respuesta> EnvioCorreo(MensajeEmail msj)
    {
        Respuesta respuesta = new Respuesta();
        try
        {
            logger.LogDebug("ProxyConversacionComunicaciones - Envio de Correo");
            var host = configuracionAPI.ObtieneHost("comunicaciones");
            if (host == null)
            {
                respuesta.Error = new ErrorProceso() { Mensaje = $"ProxyConversacionComunicaciones - Host Comunicaciones no configurado", Codigo = "", HttpCode = HttpCode.UnprocessableEntity };
            }
            else
            {
                TokenJWT? jWT = null;
                if (string.IsNullOrEmpty(host.ClaveAutenticacion))
                {
                    respuesta.Error = new ErrorProceso() { Mensaje = $"ProxyConversacionComunicaciones - No hay una clave de autencicacion definida para Comunicaciones", Codigo = "", HttpCode = HttpCode.UnprocessableEntity };
                }
                else
                {
                    jWT = await autenticacionJWT!.TokenInterproceso(host.ClaveAutenticacion);
                    if (jWT == null)
                    {
                        logger.LogDebug("ProxyConversacionComunicaciones - Error al obtener el token interservicio de JWT para Comunicaciones");
                    }
                    else
                    {
                        comunicacionesHttpClient.BaseAddress = new Uri(host.UrlBase.TrimEnd('/'));
                        logger.LogDebug($"ProxyConversacionComunicaciones - Llamado remoto a {Path.Combine(comunicacionesHttpClient.BaseAddress.ToString(), "/email/EnviarCorreo")}");
                        var payload = new StringContent(JsonConvert.SerializeObject(msj), Encoding.UTF8, "application/json");
                        comunicacionesHttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jWT.access_token);
                        var response = await comunicacionesHttpClient.PostAsync($"/email/EnviarCorreo", payload);
                        logger.LogDebug($"ProxyConversacionComunicaciones - Respuesta {response.StatusCode} {response.ReasonPhrase}");

                        string? contenidoRespuesta = await response.Content.ReadAsStringAsync();

                        if (response.IsSuccessStatusCode)
                        {
                            respuesta.Ok = true;
                        }
                        else
                        {
                            respuesta.Error = new ErrorProceso() { Mensaje = $"ProxyConversacionComunicaciones - error llamaa remota {response.ReasonPhrase} {contenidoRespuesta}", Codigo = "", HttpCode = (HttpCode)response.StatusCode };

                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, $"ProxyConversacionComunicaciones - Error al enviar correo {ex.Message}");
            respuesta.Error = new ErrorProceso() { Mensaje = $"{ex.Message}", Codigo = "", HttpCode = HttpCode.ServerError };
        }

        return respuesta;
    }


    public async Task<Respuesta> EnvioWhatsApp(MensajeWhatsapp mensajeWhatsapp)
    {
        Respuesta respuesta = new Respuesta();
        try
        {
            logger.LogDebug("ProxyConversacionComunicaciones - Envio de Correo");
            var host = configuracionAPI.ObtieneHost("comunicaciones");
            if (host == null)
            {
                respuesta.Error = new ErrorProceso() { Mensaje = $"ProxyConversacionComunicaciones - Host Comunicaciones no configurado", Codigo = "", HttpCode = HttpCode.UnprocessableEntity };
            }
            else
            {
                TokenJWT? jWT = null;
                if (string.IsNullOrEmpty(host.ClaveAutenticacion))
                {
                    respuesta.Error = new ErrorProceso() { Mensaje = $"ProxyConversacionComunicaciones - No hay una clave de autencicacion definida para Comunicaciones", Codigo = "", HttpCode = HttpCode.UnprocessableEntity };
                }
                else
                {
                    jWT = await autenticacionJWT!.TokenInterproceso(host.ClaveAutenticacion);
                    if (jWT == null)
                    {
                        logger.LogDebug("ProxyConversacionComunicaciones - Error al obtener el token interservicio de JWT para Comunicaciones");
                    }
                    else
                    {
                        comunicacionesHttpClient.BaseAddress = new Uri(host.UrlBase.TrimEnd('/'));
                        var payload = new StringContent(JsonConvert.SerializeObject(mensajeWhatsapp), Encoding.UTF8, "application/json");
                        comunicacionesHttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jWT.access_token);
                        HttpResponseMessage response = new();
                        switch (mensajeWhatsapp.Tipo)
                        {
                            case Constantes.TipoMensaje.texto:
                                logger.LogDebug($"ProxyConversacionComunicaciones - Llamado remoto a {Path.Combine(comunicacionesHttpClient.BaseAddress.ToString(), "/whatsapp/EnviarTxt")}");
                                response = await comunicacionesHttpClient.PostAsync($"/whatsapp/EnviarTxt", payload);
                                break;
                            case Constantes.TipoMensaje.img:
                                logger.LogDebug($"ProxyConversacionComunicaciones - Llamado remoto a {Path.Combine(comunicacionesHttpClient.BaseAddress.ToString(), "/whatsapp/EnviarImg")}");
                                response = await comunicacionesHttpClient.PostAsync($"/whatsapp/EnviarImg", payload);
                                break;
                        }
                        logger.LogDebug($"ProxyConversacionComunicaciones - Respuesta {response.StatusCode} {response.ReasonPhrase}");

                        string? contenidoRespuesta = await response.Content.ReadAsStringAsync();

                        if (response.IsSuccessStatusCode)
                        {
                            respuesta.Ok = true;
                        }
                        else
                        {
                            respuesta.Error = new ErrorProceso() { Mensaje = $"ProxyConversacionComunicaciones - error llamaa remota {response.ReasonPhrase} {contenidoRespuesta}", Codigo = "", HttpCode = (HttpCode)response.StatusCode };
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, $"ProxyConversacionComunicaciones - Error al enviar correo {ex.Message}");
            respuesta.Error = new ErrorProceso() { Mensaje = $"{ex.Message}", Codigo = "", HttpCode = HttpCode.ServerError };
        }

        return respuesta;
    }
}
