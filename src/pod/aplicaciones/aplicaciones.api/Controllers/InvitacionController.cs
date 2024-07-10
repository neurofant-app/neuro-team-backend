using aplicaciones.model;
using aplicaciones.model.invitaciones;
using aplicaciones.services.invitacion;
using aplicaciones.services.proxy;
using aplicaciones.services.proxy.implementations;
using comunes.interservicio.primitivas;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Annotations;
using System.Text;

namespace aplicaciones.api.Controllers;

[Route("invitacion")]
[Authorize]
[ApiController]
public class InvitacionController : ControladorJwt
{
    private readonly IServicioEntidadInvitacion _servicioInvitacion;
    private readonly IProxyIdentityServices _proxyIdentityServices;
    private readonly ILogger<InvitacionController> _logger;

    public InvitacionController(ILogger<InvitacionController> logger, IServicioEntidadInvitacion ServicioInvitacion, IProxyIdentityServices proxyIdentityServices):base(logger) {
        _servicioInvitacion = ServicioInvitacion;
        _proxyIdentityServices = proxyIdentityServices;        
        _logger = logger;
    }

    [HttpGet("prueba-seguridad")]
    public async Task<IActionResult> Seguridad()
    {
        return Ok();
    }


    [HttpPost("confirmar/{id}")]
    [AllowAnonymous]
    [SwaggerOperation("Realiza la confirmación de una invitación")]
    [SwaggerResponse(statusCode: 200, description: "La confirmación ha ocurrido satisfactoriamente")]
    [SwaggerResponse(statusCode: 400, description: "Datos incorrectos")]
    [SwaggerResponse(statusCode: 404, description: "Invitación no localizada o inexistente")]
    public async Task<ActionResult<TokenJWT>> Confirmar([FromRoute] Guid id, [FromBody]Confirmacion confirmacion)
    {
        var respuestaInivtacion = await _servicioInvitacion.UnicaPorId(id.ToString());

        if (respuestaInivtacion.Ok)
        {
            if (ValidaPassword(confirmacion.Password))
            {
                EntidadInvitacion invitacion = (EntidadInvitacion)respuestaInivtacion.Payload!;
                RegisterViewModel usuario = new() { Email = invitacion.Email, Password = confirmacion.Password };
                var respuestaUsuario = await _proxyIdentityServices.CreaUsuario(usuario);
                if (respuestaUsuario.Ok)
                {
                    var r = await _servicioInvitacion.Eliminar(invitacion.Id.ToString());
                    if (!r.Ok) {
                        _logger.LogWarning($"La invitacion {id} no pudo ser eliminada ");
                    }
                    return Ok();

                }
                else
                {
                    _logger.LogWarning($"El usuario no pudo crearse {JsonConvert.SerializeObject(respuestaUsuario.Error)}");
                    return StatusCode((int)respuestaUsuario.HttpCode, respuestaUsuario.Error);
                }
            }
            else
            {
                _logger.LogDebug($"Contraseña no válida");
                return BadRequest("Contraseña no válida");
            }

        }
        else
        {
            _logger.LogDebug($"Ocurrió un error al obetener la invitacion {JsonConvert.SerializeObject(respuestaInivtacion.Error)}");
        }
        return StatusCode((int)respuestaInivtacion.HttpCode, respuestaInivtacion.Error);
    }

}
