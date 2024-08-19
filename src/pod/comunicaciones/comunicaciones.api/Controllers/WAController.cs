using comunicaciones.modelo.whatsapp;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using comunicaciones.servicios.whatsapp;
using comunes.primitivas;
using Newtonsoft.Json;
using System.Text;
using System.IO;

namespace comunicaciones.api.Controllers;
[ApiController]
[Authorize]
[Route("whatsapp")]
public class WAController
{
    private readonly ILogger<EmailController> _logger;
    public readonly IServicioWhatsapp servicioWhatsapp;
    private readonly IConfiguration configuration;

    public WAController(ILogger<EmailController> logger, IServicioWhatsapp servicioWhatsapp,IConfiguration configuration) {
        this._logger = logger;
        this.servicioWhatsapp = servicioWhatsapp;
        this.configuration = configuration;
    }

    /// <summary>
    /// Se requiere activar el permiso de messages para que funcione wehook
    /// </summary>
    /// <param name="mode"></param>
    /// <param name="challenge"></param>
    /// <param name="verify_token"></param>
    /// <returns></returns>
    [HttpGet]
    [AllowAnonymous]
    [Route("webhook")]
    public string Webhook(
        [FromQuery(Name = "hub.mode")] string mode,
        [FromQuery(Name = "hub.challenge")] string challenge,
        [FromQuery(Name = "hub.verify_token")] string verify_token
    )
    {
        _logger.LogDebug($"Valiando token para puente de conexion");
        if (verify_token.Equals("WAbot"))
        {
            return challenge;
        }
        else
        {
            _logger.LogDebug($"Token no valido para puente de conexion");
            return "";
        }
    }
    /// <summary>
    /// Envia la respuesta obtenida desde whatsapp
    /// </summary>
    /// <param name="entry"></param>
    [HttpPost]
    [AllowAnonymous]
    [Route("webhook")]
    public async void  datos([FromBody] WebHookResponseModel entry)
    {
        string mensaje_recibido = entry.entry[0].changes[0].value.messages[0].text.body;
        string id_wa = entry.entry[0].changes[0].value.messages[0].id;
        string telefono_wa = entry.entry[0].changes[0].value.messages[0].from;
        string telefono = $"{telefono_wa.Substring(0,2)}{ telefono_wa.Substring(3,telefono_wa.Length-3)}";
        var url = Environment.GetEnvironmentVariable("urlCaptcha");
        var httpClient = new HttpClient() { BaseAddress = new Uri(url) };
        string path = Path.Combine(url, $"/accesocaptcha/facturacion/respuesta");

        _logger.LogDebug($" LLamada remota a la ruta {path}");
        var result = await httpClient.PostAsync(path, new StringContent(JsonConvert.SerializeObject(new MensajeWhatsapp(){Telefono=telefono,Mensaje=mensaje_recibido.ToUpper()}), Encoding.UTF8, "application/json"));
        if (result.IsSuccessStatusCode)
        {
            _logger.LogDebug($" LLamada remota a la ruta {path} finalizada satisfactoriamente");
        }
        else
        {
            _logger.LogDebug($" LLamada remota a la ruta {path}, NO se finalizo satisfactoriamente ");
        }
    }

    /// <summary>
    /// Envia Imagen de captcha por whatsapp
    /// </summary>
    /// <param name="mensaje"></param>
    /// <returns></returns>
    [HttpPost]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [Route("EnviarImg")]
    public async Task<Respuesta> EnviarImagen([FromBody] MensajeWhatsapp mensaje)
    {
        Respuesta respuesta = new();
        string idTelefonoEmpresa = configuration.GetValue<string>("WhatsappConfig:remitente"); 
        string Token = configuration.GetValue<string>("WhatsappConfig:token");
        string telefonoDestino = mensaje.Telefono;
        string urlBase = "https://graph.facebook.com/" + idTelefonoEmpresa + "/";
        var rutaImg = servicioWhatsapp.Base64ToImage(mensaje.Mensaje);
        string IdImg = servicioWhatsapp.SubirImagen(urlBase, Token, rutaImg);

        if(!string.IsNullOrEmpty(IdImg))
        {           
            respuesta = await servicioWhatsapp.EnviarImagen(IdImg, urlBase, Token, telefonoDestino);
            if(respuesta.Ok)
            {
                _logger.LogDebug($"Se Envio Imagen Captcha al telefono {telefonoDestino}");
                respuesta.Ok = true;
            }
            else
            {
                _logger.LogError($"No se Envio Imagen Captcha al telefono {telefonoDestino}");
            }
        }
        return respuesta;
    }



    /// <summary>
    /// Envia TEXTO 
    /// </summary>
    /// <param name="mensaje"></param>
    /// <returns></returns>
    [HttpPost]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [Route("EnviarTxt")]

    public async Task<Respuesta> EnviarTxt([FromBody] MensajeWhatsapp mensaje)
    {
        Respuesta respuesta = new();
        string idTelefonoEmpresa = configuration.GetValue<string>("WhatsappConfig:remitente");
        string Token = configuration.GetValue<string>("WhatsappConfig:token");
        string telefonoDestino = mensaje.Telefono;
        string urlBase = "https://graph.facebook.com/" + idTelefonoEmpresa + "/";
        respuesta = await servicioWhatsapp.EnviarTxt(urlBase, Token, telefonoDestino, mensaje.Mensaje);
        if (respuesta.Ok)
        {
            _logger.LogDebug($"Se Envio Msj al telefono {telefonoDestino}");
            respuesta.Ok = true;
        }
        else
        {
            _logger.LogError($"No se Envio el msj al telefono {telefonoDestino}");
        }
        return respuesta;
    }


}
