namespace aplicaciones.model;

public class ConsultaAplicacionAnonima
{
    /// <summary>
    /// Identificador único de la aplicación
    /// </summary>
    public string Clave { get; set; }
    /// <summary>
    /// Nombre de la aplicación que emite la invitación
    /// </summary>
    public required string Nombre { get; set; }

    public IEnumerable<EntidadPlantillaInvitacion> Plantillas { get; set; } = [];

    public IEnumerable<EntidadLogoAplicacion> Logotipos { get; set; } = [];

    public IEnumerable<EntidadConsentimiento> Consentimientos { get; set; } = [];
}
