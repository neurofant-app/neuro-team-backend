using MongoDB.Bson.Serialization.Attributes;
using System.Diagnostics.CodeAnalysis;

namespace evaluacion.model.evaluacion.variantes;

/// <summary>
/// Variante de evaluación aplicada a los alumnos
/// </summary>
[ExcludeFromCodeCoverage]
public class VarianteEvaluacion
{

    /// <summary>
    /// Identificador único de la variante de evaluación
    /// </summary>
    [BsonElement("id")]
    public Guid Id { get; set; }

    /// <summary>
    /// Nombre de la variante, si no se proporicione se asigna automáticamente
    /// con el nombre la evaluación y el Id
    /// </summary>
    [BsonElement("n")]
    public string? Nombre { get; set; }

    /// <summary>
    /// Identificador único del creador de la evaluación
    /// </summary>
    [BsonElement("cid")]
    public Guid CreadorId { get; set; }


    /// <summary>
    /// Fecha de creación de la evaluación
    /// </summary>
    [BsonElement("fc")]
    public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;


    /// <summary>
    /// Reactivos ordenados asociados a la evaluación
    /// </summary>
    [BsonElement("r")]
    public List<ReactivoEvaluacion> Reactivos { get; set; } = [];

    /// <summary>
    /// Total de reactivos en la variante
    /// </summary>
    [BsonElement("tr")]
    public int TotalReactivos { get; set; } = 0;

    /// <summary>
    /// Total de puntos de reactivos en la variante
    /// </summary>
    [BsonElement("tpu")]
    public int TotalPuntos { get; set; } = 0;

    /// <summary>
    /// Total de ejecuciones de la variante
    /// </summary>
    [BsonElement("tpu")]
    public int TotalEjecuciones { get; set; } = 0;

}
