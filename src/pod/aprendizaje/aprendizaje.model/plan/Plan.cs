using comunes.primitivas.atributos;
using MongoDB.Bson.Serialization.Attributes;
using System.Diagnostics.CodeAnalysis;

namespace aprendizaje.model;


/// <summary>
/// Define un plan de estudios
/// </summary>
[ExcludeFromCodeCoverage]
[EntidadDB]
public class Plan
{
    /// <summary>
    /// Identificador único del plan de estudios
    /// </summary>
    [BsonId]
    public Guid Id { get; set; }

    /// <summary>
    /// Didentificador único del espacio de trabajo al que pertenece el plan de estudios
    /// </summary>
    [BsonElement("eid")]
    public required string EspacioTrabajoId { get; set; }

    /// <summary>
    /// Idioma en los que se ofrece el contenido del plan de estudios
    /// </summary>
    [BsonElement("i")]
    public required string Idioma { get; set; }

    /// <summary>
    /// Nombre del plan de estudios
    /// </summary>
    [BsonElement("n")]
    public required string Nombre { get; set; }

    /// <summary>
    /// Versión del plan de estudios
    /// </summary>
    [BsonElement("v")]
    public string Version { get; set; } = "";

    /// <summary>
    /// Lista de temarios asociados al plan de estudios
    /// </summary>
    [BsonElement("ts")]
    public List<TemarioPlan> Temarios { get; set; } = [];

}
