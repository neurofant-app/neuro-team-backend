using MongoDB.Bson.Serialization.Attributes;
using System.Diagnostics.CodeAnalysis;

namespace evaluacion.model.evaluacion.temas;

/// <summary>
/// Define el contenido de evauación por tema de la evaluacion
/// </summary>
[ExcludeFromCodeCoverage]
public class TemaEvaluacion
{
    /// <summary>
    /// Identificador único del temario asociado a la evaluación
    /// </summary>
    [BsonElement("t")]
    public Guid TemarioId { get; set; }

    /// <summary>
    /// Identificador único del tema contenido del la evaluación
    /// </summary>
    [BsonElement("tid")]
    public Guid TemaId { get; set; }

    /// <summary>
    /// Lista de reactivos asociados a la evaluación
    /// </summary>
    [BsonElement("rs")]
    public List<ReactivoTema> Reactivos { get; set; } = [];
}

