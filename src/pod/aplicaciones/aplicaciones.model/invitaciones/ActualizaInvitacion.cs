using comunes.primitivas.atributos;

namespace aplicaciones.model;
[CQRSActualizar]
public class ActualizaInvitacion
{
    /// <summary>
    /// IDentificador únido de la entidad
    /// </summary>
    public Guid Id { get; set; }
    // Requerida 
    // [A] [D]

    /// <summary>
    /// Identificadeor único de la aplicación que genera la invitacions
    /// </summary>
    public required Guid AplicacionId { get; set; }
    // Requerida 
    // [A] [I] [D]
}
