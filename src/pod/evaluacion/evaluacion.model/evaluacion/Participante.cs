using MongoDB.Bson.Serialization.Attributes;
using System.Diagnostics.CodeAnalysis;

namespace evaluacion.model.evaluacion;


/// <summary>
/// Define un participante del proceso de evaluacion
/// </summary>
[ExcludeFromCodeCoverage]
public class Participante
{

    /// <summary>
    /// Identificador único del participante
    /// </summary>
    [BsonElement("i")]
    public required string Id { get; set; }

    /// <summary>
    /// IDentificador de la variante de evaluación aplicada
    /// </summary>
    [BsonElement("v")]
    public Guid? VarianteId { get; set; }

    /// <summary>
    /// Procentaje de aciertos de la evaluacion
    /// </summary>
    [BsonElement("p")]
    public decimal? Porcentaje { get; set; }

    /// <summary>
    /// Fecha de calificación
    /// </summary>
    [BsonElement("fc")]
    public DateTime? FechaCalificacion { get; set; }

}
