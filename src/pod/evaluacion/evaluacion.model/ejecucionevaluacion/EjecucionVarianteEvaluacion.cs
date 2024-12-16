using MongoDB.Bson.Serialization.Attributes;
using System.Diagnostics.CodeAnalysis;

namespace evaluacion.model.ejecucionevaluacion;

/// <summary>
/// Elamcena los datos de la ejecución de una variante de evaluación
/// </summary>
[ExcludeFromCodeCoverage]
public class EjecucionVarianteEvaluacion
{
    /// <summary>
    /// Identificador único de le ejecución de la variante de evaluación
    /// </summary>
    [BsonId]
    public Guid Id { get; set; }

    /// <summary>
    /// Identificador de la evaluación 
    /// </summary>
    [BsonElement("eid")]
    public Guid EvaluacionId { get; set; }

    /// <summary>
    /// Identificador de la variante de evaluación 
    /// </summary>
    [BsonElement("vid")]
    public Guid VarianteId { get; set; }

    /// <summary>
    /// Total de participantes en la evaluación
    /// </summary>
    [BsonElement("tp")]
    public int TotalParticipantes { get; set; } = 0;

}
