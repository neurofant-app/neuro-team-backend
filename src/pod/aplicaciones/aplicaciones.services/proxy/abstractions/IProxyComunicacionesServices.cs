using api.comunicaciones;
using comunes.primitivas;

namespace aplicaciones.services.proxy.abstractions;

public interface IProxyComunicacionesServices
{
    Task<Respuesta> EnviarCorreo(MensajeEmail msj);
}
