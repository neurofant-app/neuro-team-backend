using comunes.primitivas.atributos;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;

namespace aplicaciones.model;
[EntidadDB]
public class EntidadInvitacion
{
    [BsonId]
    /// <summary>
    /// IDentificador únido de la entidad
    /// </summary>
    public virtual Guid Id { get; set; }
    // Requerida 
    // [A] [D]

    /// <summary>
    /// Identificadeor único de la aplicación que genera la invitacions
    /// </summary>
    [BsonElement("aid")]
    public required Guid AplicacionId { get; set; }
    // Requerida 
    // [A] [I] [D]

    /// <summary>
    /// Fecha de creación de la invitaciónwhatsapp
    /// </summary>
    [BsonElement("f")]
    public DateTime Fecha { get; set; } = DateTime.UtcNow;
    // Requerida 
    // [D]

    /// <summary>
    /// Esatdo de la invitación
    /// </summary>
    [BsonElement("ei")]
    public EstadoInvitacion Estado { get; set; } = EstadoInvitacion.Nueva;
    // Requerida 
    // [D]


    /// <summary>
    /// Email de contacto del invitado
    /// </summary>
    [BsonElement("e")]
    public string Email { get; set; }
    // 250
    // [D]

    /// <summary>
    /// Identificador unico del rol solicitado en la invitaci[on
    /// </summary>
    [BsonElement("rid")]
    public int? RolId { get; set; }
    // Requerida 
    // [D]

    /// <summary>
    /// Nombre del destinatario de la invitacion
    /// </summary>
    [BsonElement("n")]
    public string Nombre { get; set; }

    /// <summary>
    /// Define el tipo de invitacion
    /// </summary>
    [BsonElement("tc")]
    public TipoComunicacion Tipo { get; set; } = TipoComunicacion.Registro;
    // Requerida 
    // [I] [D]

    /// <summary>
    /// Token del servicio
    /// </summary>
    [BsonElement("t")]
    public string? Token { get; set; }
    // Longitud 512
    // [I] [D]

    /// <summary>
    /// Aplicación asociada a la invitación
    /// </summary>
    [BsonIgnore]
    [JsonIgnore]
    public Aplicacion Aplicacion { get; set; }
}
