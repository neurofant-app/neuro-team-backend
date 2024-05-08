using comunes.primitivas.atributos;

namespace aplicaciones.model;
[CQRSCrear]
public class CreaInvitacion
{
    /// <summary>
    /// Identificadeor único de la aplicación que genera la invitacions
    /// </summary>
    public required Guid AplicacionId { get; set; }
    // Requerida 
    // [A] [I] [D]

    /// <summary>
    /// Email de contacto del invitado
    /// </summary>
    public string Email { get; set; }
    // 250
    // [D]

    /// <summary>
    /// Identificador unico del rol solicitado en la invitaci[on
    /// </summary>
    public int? RolId { get; set; }
    // Requerida 
    // [D]

    /// <summary>
    /// Nombre del destinatario de la invitacion
    /// </summary>
    public string Nombre { get; set; }

    /// <summary>
    /// Define el tipo de invitacion
    /// </summary>
    public TipoComunicacion Tipo { get; set; }
    // Requerida 
    // [I] [D]

    /// <summary>
    /// Token del servicio
    /// </summary>
    public string? Token { get; set; }
    // Longitud 512
    // [I] [D]
}
