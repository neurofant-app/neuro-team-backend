using comunes.primitivas;
using comunicaciones.modelo;
using Microsoft.AspNetCore.Mvc;

namespace comunicaciones.servicios.email;

public interface IServicioEmail
{
    Task<Respuesta> Enviar(MensajeEmail msg);
}
