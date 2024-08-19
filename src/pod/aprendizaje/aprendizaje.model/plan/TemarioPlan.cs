using MongoDB.Bson.Serialization.Attributes;

namespace aprendizaje.model;


/// <summary>
/// Define un temario que participa en un plan de estidios
/// </summary>
public class TemarioPlan
{
    /// <summary>
    /// Identificador único del temario en el plan
    /// </summary>
    [BsonId]
    public Guid Id { get; set; }

    /// <summary>
    /// IDentificador único del temario
    /// </summary>
    [BsonElement("tid")]
    public Guid TemarioId { get; set; }

    /// <summary>
    /// Elementos de temario necesarions para ingresar al tema
    /// </summary>
    [BsonElement("ts")]
    public List<Guid> TemariosSeriacion { get; set; } = [];
}
