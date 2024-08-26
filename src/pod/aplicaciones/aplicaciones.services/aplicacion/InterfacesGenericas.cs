using apigenerica.model.servicios;
using aplicaciones.model;
using comunes.primitivas;
namespace aplicaciones.services.aplicacion;
/// <summary>
/// Interfaz para el servicio de Aplicacion
/// </summary>
public interface IServicioAplicacion : IServicioEntidadGenerica<EntidadAplicacion, CreaAplicacion, ActualizaAplicacion, ConsultaAplicacion, string>
{
    Task<RespuestaPayload<ConsultaAplicacionAnonima>> ConsultaAplicacion(string host, string? clave);
}

