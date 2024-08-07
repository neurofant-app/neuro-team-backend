﻿using aplicaciones.model;
using aplicaciones.model.aplicaciones;
using aplicaciones.model.invitaciones;
using aplicaciones.services.dbcontext;
using aplicaciones.services.invitacion;
using aplicaciones.services.proxy;
using comunes.primitivas;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;

namespace aplicaciones.api.Controllers;

/// <summary>
/// Servicios de gestion de la cuenta y perfil del usuario
/// </summary>
[Route("api/[controller]")]
[ApiController]
[Authorize]
public class UsuarioController : ControladorJwt
{
    private readonly IServicioEntidadInvitacion _servicioInvitacion;
    private readonly IProxyIdentityServices _proxyIdentityServices;
    private readonly ILogger<UsuarioController> _logger;
    public UsuarioController(ILogger<UsuarioController> logger, IServicioEntidadInvitacion ServicioInvitacion, IProxyIdentityServices proxyIdentityServices) : base(logger)
    {
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
        if (ValidaEmail(email))
        {
            var respuestaUsuario = await _proxyIdentityServices.RecuperaPasswordEmail(email);
            if (respuestaUsuario.Ok)
            {
                DTORecuperacionPassword dto = (DTORecuperacionPassword)respuestaUsuario.Payload;
                CreaInvitacion invInsertar = new CreaInvitacion()
                {
                    AplicacionId = new Guid("3a1b8fd8-3818-4f64-95a9-492c7db3435d"),
                    Email = dto.Email,
                    RolId = 0,
                    Nombre = dto.UserName,
                    Tipo = TipoComunicacion.RecuperacionContrasena,
                    Token = dto.TokenRecuperacion
                };
                var inv = await _servicioInvitacion.Insertar(invInsertar);
                if (inv.Ok)
                {
                    return Ok(inv.Payload);
                }
            }
            else
            {
                return NotFound();
            }
        }
        return BadRequest();
    }

    [HttpPost("password/restablecer/token")]
    [AllowAnonymous]
    [SwaggerOperation("Realiza el cambio de contraseña utilizando un token")]
    [SwaggerResponse(statusCode: 200, type: typeof(Respuesta), description: "La contraeña ha sido restablecida satisfactoriamente")]
    [SwaggerResponse(statusCode: 404, description: "Invitacion inexistente")]
    [SwaggerResponse(statusCode: 400, description: "Datos incorrectos")]
    public async Task<IActionResult> RestablecerContrasena([FromBody] DTOResetPassword dtoReset)
    {
        RespuestaPayload<EntidadInvitacion> respuesta = await _servicioInvitacion.UnicaPorId(dtoReset.InvitacionId.ToString());
        if (respuesta != null)
        {
            var invitacion = (EntidadInvitacion)respuesta.Payload;
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
    }

}