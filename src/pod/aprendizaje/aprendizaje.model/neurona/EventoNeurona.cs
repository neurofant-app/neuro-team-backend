using MongoDB.Bson.Serialization.Attributes;
using System.Diagnostics.CodeAnalysis;

namespace aprendizaje.model.neurona;

/// <summary>
/// Define un evento de actualización de una neurona publicada previamente
/// </summary>
[ExcludeFromCodeCoverage]
public class EventoNeurona
{
    /// <summary>
    /// Identificador único del evento
    /// </summary>
    [BsonId]
    public Guid Id { get; set; }

    /// <summary>
    /// Fecha del evento de actualización UTC
    /// </summary>
    [BsonElement("f")]
    public DateTime Fecha { get; set; }

    /// <summary>
    /// Tipo de evento ocurrido para la neurona
    /// </summary>
    [BsonElement("te")]
    public TipoEventoNeurona TipoEvento { get; set; }


    /// <summary>
    /// Comentarios de la actualización
    /// </summary>
    [BsonElement("c")]
    public List<ValorI18N<string>> Comentarios { get; set; } = [];
}
