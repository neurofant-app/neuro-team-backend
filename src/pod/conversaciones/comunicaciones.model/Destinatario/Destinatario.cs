using MongoDB.Bson.Serialization.Attributes;

namespace comunicaciones.model;

/// <summary>
/// Identificador único de una persona dependiendo del tipo de medio
/// </summary>
public class Destinatario
{
    /// <summary>
    /// En el caso SMS, WhatsApp Y Telegram será el número de teléfono
    /// En el caso de Correo Electrónico será el Email.
    /// </summary>
    [BsonId]
    public  string  MedioId { get; set; }
    /// <summary>
    /// Describe quién será el participante
    /// </summary>
    [BsonElement("t")]
    public TipoParticipante Tipo { get; set; }
    /// <summary>
    /// Cuando no se tenga información de sesión del usuario bastará con el MedioId
    /// En caso contrario el destinatario podrá incluir un UsuarioId que permite filtrar conversaciones
    /// </summary>
    [BsonElement("uid")]
    public string? UsuarioId { get; set; }
    /// <summary>
    /// Nombre del destinatario
    /// </summary>
    [BsonElement("n")]
    public string? Nombre { get; set; }
    /// <summary>
    /// Identificador corto que hace referencia a el Id del medio
    /// </summary>
    [BsonElement("cid")]
    public string? CortoId { get; set; }
}
