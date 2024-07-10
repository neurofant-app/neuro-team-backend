using apigenerica.model.servicios;
using aplicaciones.model;

namespace aplicaciones.services.invitacion;

/// <summary>
/// Interfaz para el servicio de Invitacion
/// </summary>
public interface IServicioEntidadInvitacion : IServicioEntidadGenerica<EntidadInvitacion, CreaInvitacion, ActualizaInvitacion, ConsultaInvitacion, string>
{
}
