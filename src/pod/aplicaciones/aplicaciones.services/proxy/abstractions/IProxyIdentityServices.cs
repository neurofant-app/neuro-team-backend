using aplicaciones.model.aplicaciones;
using aplicaciones.model.invitaciones;
using comunes.primitivas;

namespace aplicaciones.services.proxy;

public interface IProxyIdentityServices
{
    Task<Respuesta> CreaUsuario(RegisterViewModel model);
    Task<RespuestaPayload<string>> RecuperaPasswordEmail(string email);
    Task<Respuesta> EstablecePasswordToken(ActualizarContrasena actualizarContrasena);
}
