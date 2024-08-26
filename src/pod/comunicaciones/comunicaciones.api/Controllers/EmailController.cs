using comunes.primitivas;
using comunicaciones.modelo;
using comunicaciones.servicios.email;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
namespace comunicaciones.api.Controllers;
[ApiController]
[Authorize]
[Route("email")]
public class EmailController : ControllerBase
{
    private readonly ILogger<EmailController> _logger;
    private readonly IServicioEmail servicioEmail;
    public EmailController(ILogger<EmailController> logger, IServicioEmail servicioEmail)
    {
        this._logger = logger;
        this.servicioEmail = servicioEmail;
    }

    [HttpGet("prueba-seguridad")]
    public async Task<IActionResult> Seguridad()
    {
        return Ok();
    }


    [HttpPost("EnviarCorreo")]
    [AllowAnonymous]
    [SwaggerOperation("Envia un correo ")]
    [SwaggerResponse(statusCode: 200, description: "El correo se ha satisfactoriamente")]
    [SwaggerResponse(statusCode: 400, description: "No se pudo enviar correo")]
    [SwaggerResponse(statusCode: 409, description: "Correo enviado anteriormente")]
    public async Task<IActionResult> EnviarCorreo([FromBody] MensajeEmail datos)
    {
        _logger.LogDebug("EmailController - EnviarCorreo {datos}", datos);
        var resultado = await servicioEmail.Enviar(datos);
        if (resultado.Ok)
        {
            _logger.LogDebug("EmailController -  resultado {ok} {code} {error}", resultado!.Ok, resultado!.HttpCode, resultado.Error);
            return Ok();
        }
        else
        {
            _logger.LogDebug("EmailController -  resultado {ok} {code} {error}", resultado!.Ok, resultado!.HttpCode, resultado.Error);
            return Conflict(resultado.Error);
        }
    }


}
