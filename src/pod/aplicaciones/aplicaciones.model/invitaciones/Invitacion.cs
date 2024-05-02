using System.Text.Json.Serialization;

namespace aplicaciones.model;

public class Invitacion
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

    /// <summary>
    /// Fecha de creación de la invitaciónwhatsapp
    /// </summary>
    public DateTime Fecha { get; set; } = DateTime.UtcNow;
    // Requerida 
    // [D]

    /// <summary>
    /// Esatdo de la invitación
    /// </summary>
    public EstadoInvitacion Estado { get; set; } = EstadoInvitacion.Nueva;
    // Requerida 
    // [D]


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
    public TipoComunicacion Tipo { get; set; } = TipoComunicacion.Registro;
    // Requerida 
    // [I] [D]

    /// <summary>
    /// Token del servicio
    /// </summary>
    public string? Token { get; set; }
    // Longitud 512
    // [I] [D]

    /// <summary>
    /// Aplicación asociada a la invitación
    /// </summary>
    [JsonIgnore]
    public Aplicacion Aplicacion { get; set; }

}
