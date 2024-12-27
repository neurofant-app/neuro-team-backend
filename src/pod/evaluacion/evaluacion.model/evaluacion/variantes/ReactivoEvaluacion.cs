using MongoDB.Bson.Serialization.Attributes;
using System.Diagnostics.CodeAnalysis;

namespace evaluacion.model.evaluacion.variantes;

/// <summary>
/// Define los elementos participantes en una variante de evluación
/// </summary>
[ExcludeFromCodeCoverage]
public class ReactivoEvaluacion
{
    /// <summary>
    /// Posisción del elemento dentro de la variante de evaluación
    /// </summary>
    [BsonElement("i")]
    public required int Posicion { get; set; }

    /// <summary>
    /// Identificador único del reactivo
    /// </summary>
    [BsonElement("id")]
    public required string ReactivoId { get; set; }

    /// <summary>
    /// Respuestas en el órden de presentacion de la variante
    /// </summary>
    public List<string> Respuestas { get; set; } = [];

}
