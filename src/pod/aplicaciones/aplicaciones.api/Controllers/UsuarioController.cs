using aplicaciones.model;
using aplicaciones.model.aplicaciones;
using aplicaciones.model.invitaciones;
using aplicaciones.services.dbContext;
using aplicaciones.services.invitacion;
using aplicaciones.services.proxy;
using comunes.primitivas;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace aplicaciones.api.Controllers;

/// <summary>
/// Servicios de gestion de la cuenta y perfil del usuario
/// </summary>
[Route("api/[controller]")]
[ApiController]
[Authorize]
public class UsuarioController : ControladorJwt
{
    private DbContextAplicaciones _localContext;
    private readonly IServicioInvitacion _servicioInvitacion;
    private readonly IProxyIdentityServices _proxyIdentityServices;
    private readonly ILogger<UsuarioController> _logger;
    public UsuarioController(DbContextAplicaciones context, ILogger<UsuarioController> logger, IServicioInvitacion ServicioInvitacion, IProxyIdentityServices proxyIdentityServices) : base(logger)
    {
        _localContext = context;
        _servicioInvitacion = ServicioInvitacion;
        _proxyIdentityServices = proxyIdentityServices;
        _logger = logger;
    }

    [HttpPost("password/recuperar")]
    [AllowAnonymous]
    [SwaggerOperation("Inicia el proceso de recupración de la contraseña de un usuario")]
    [SwaggerResponse(statusCode: 200, type: typeof(DTORecuperacionPassword), description: "Se envió una invitación al usuario vía email")]
    [SwaggerResponse(statusCode: 404, description: "Usuario inexistente")]
    [SwaggerResponse(statusCode: 400, description: "Datos incorrectos")]
    public async Task<IActionResult> RecuperarContrasena([FromQuery] string email)
    {
        // Verifica que el correo venga en un formato válido si no es asi devolver 400
        if (ValidaEmail(email))
        {
            // Utilizando el proxy de identidad realizar una llamada a RecuperaPasswordEmail y si devuelve cualquier codigo 
            //    diferente a 200 revolver 404
            var respuestaUsuario = await _proxyIdentityServices.RecuperaPasswordEmail(email);
            if (respuestaUsuario.Ok)
            {
                DTORecuperacionPassword dto = (DTORecuperacionPassword)respuestaUsuario.Payload;
                InvitacionInsertar invInsertar = new InvitacionInsertar()
                {
                    AplicacionId = new Guid("00000000-0000-0000-0000-000000000000"),
                    Email = dto.Email,
                    RolId = 0,
                    Nombre = dto.UserName,
                    Tipo = TipoComunicacion.RecuperacionContrasena,
                    Token = dto.TokenRecuperacion
                };
                var inv = await _servicioInvitacion.Insertar(invInsertar);
                if(inv.Ok)
                {
                    return Ok(inv.Payload);
                }
            }
            else
            {
                return NotFound();
            }

            // En caso de existir
            //    añadir un nuevo resgtrso de invitación con Tipo = RecuperacionContrasena y añadiendo el Token recibido
            //    enviar una invitación de recuperación utilizando el mismo template  de interforos y el PROXY de comunicaciones
            //    devolver OK
        }

        return Ok();
    }

    [HttpPost("password/restablecer/token")]
    [AllowAnonymous]
    [SwaggerOperation("Realiza el cambio de contraseña utilizando un token")]
    [SwaggerResponse(statusCode: 200, type: typeof(Respuesta), description: "La contraeña ha sido restablecida satisfactoriamente")]
    [SwaggerResponse(statusCode: 404, description: "Invitacion inexistente")]
    [SwaggerResponse(statusCode: 400, description: "Datos incorrectos")]
    public async Task<IActionResult> RestablecerContrasena([FromBody] DTOResetPassword dtoReset)
    {
        // Verificar que la invitacion exista con los datos del DTO, si no existe devolver NotFound()
        Invitacion invitacion = await _localContext.Invitaciones.Where(x => x.Id== dtoReset.InvitacionId).FirstOrDefaultAsync();
        if(invitacion != null)
        {
            ActualizarContrasena actualizarContrasena = new ActualizarContrasena() { Email = invitacion.Email, Password = dtoReset.NuevoPassword, Token = invitacion.Token };
            var response = await _proxyIdentityServices.EstablecePasswordToken(actualizarContrasena);
            if (response.Ok)
            {
                var r = await _servicioInvitacion.Eliminar(invitacion.Id.ToString());
                if (!r.Ok)
                {
                    _logger.LogWarning($"La invitacion {invitacion.Id} no pudo ser eliminada ");
                    return StatusCode(response.HttpCode.GetHashCode(), response.Error);
                }
                return Ok(r);
            }
            else
            {
                return StatusCode(response.HttpCode.GetHashCode(), response.Error);
            }
        }
        else
        {
            return NotFound();
        }
        // Si llamar al proxy de identidad para el método EstablecePasswordTokem con el valor del email 
        //          y token de la invitación y el nuevo password del DTO
        // Si el codigo de retorno es diferente de 200 devolver el error con statuscode
        // devolver OK si la contraseña ha sido restablecida
    }

}
