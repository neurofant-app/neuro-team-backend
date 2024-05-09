using System.Text.Json.Serialization;

namespace aplicaciones.model;

public class ConsultaAplicacion
{
    /// <summary>
    /// Identificador único de la aplicación
    /// </summary>
    public Guid Id { get; set; }
    // Requerida 
    // [A] [D]

    /// <summary>
    /// Nombre de la aplicación que emite la invitación
    /// </summary>
    public required string Nombre { get; set; }
    // Requerida 200
    // [I] [A] [D]

    /// <summary>
    /// Especifica si la aplicación se encuentra activa, solo es posible emitir notificaciones so lo está
    /// </summary>
    public bool Activa { get; set; }
    // Requerida
    // [I] [A] [D]

    public IEnumerable<EntidadPlantillaInvitacion> Plantillas { get; set; } = [];

    public IEnumerable<EntidadLogoAplicacion> Logotipos { get; set; } = [];

    public IEnumerable<EntidadConsentimiento> Consentimientos { get; set; } = [];
}
