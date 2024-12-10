using MongoDB.Bson.Serialization.Attributes;
using System.Diagnostics.CodeAnalysis;

namespace evaluacion.model.variantes;

/// <summary>
/// Variante de evaluación aplicada a los alumnos
/// </summary>
[ExcludeFromCodeCoverage]
public class VarianteEvaluacion
{
    /// <summary>
    /// Identificador único de la variante de evaluación
    /// </summary>
    [BsonId]
    public Guid Id { get; set; }

    /// <summary>
    /// Identificador único de la evaluación
    /// </summary>
    [BsonElement("eid")]
    public Guid EvaluacionId { get; set; }

    /// <summary>
    /// Reactivos ordenados asociados a la evaluación
    /// </summary>
    [BsonElement("r")]
    public List<ReactivoEvaluacion> Reactivos { get; set; } = [];

}
