using MongoDB.Bson.Serialization.Attributes;
using System.Diagnostics.CodeAnalysis;

namespace evaluacion.model.evaluacion;

/// <summary>
/// Define el contenido de evauación por tema de la evaluacion
/// </summary>
[ExcludeFromCodeCoverage]
public class TemaEvaluacion
{

    /// <summary>
    /// Identificador único del tema contenido del la evaluación
    /// </summary>
    [BsonElement("tid")]
    public Guid TemaId { get; set; }

    /// <summary>
    /// Lista de reactivos asociados a la evaluación
    /// </summary>
    [BsonElement("rs")]
    public List<Guid> Reactivos { get; set; } = [];

    /// <summary>
    /// Lista de reactivos que deben estar presentes en todas las variantes de la evaluación
    /// </summary>
    [BsonElement("rsr")]
    public List<Guid> ReactivosRequeridos { get; set; } = [];




}

