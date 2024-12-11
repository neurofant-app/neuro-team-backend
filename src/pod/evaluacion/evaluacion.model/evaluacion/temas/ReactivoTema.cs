using MongoDB.Bson.Serialization.Attributes;
using System.Diagnostics.CodeAnalysis;

namespace evaluacion.model.evaluacion.temas;

/// <summary>
/// Propiedades de reactivvos en la evaluación
/// </summary>
[ExcludeFromCodeCoverage]
public class ReactivoTema
{
    /// <summary>
    /// Identificador único del reactivo
    /// </summary>
    [BsonElement("i")]
    public required string Id { get; set; }

    /// <summary>
    /// Determina si el reactivo el requerido en todas las veriantes de la evaluación
    /// </summary>
    [BsonElement("r")]
    public bool Obligatorio { get; set; }

    /// <summary>
    /// Puntos asociados al reactivo en la evaluación
    /// </summary>
    [BsonElement("p")]
    public int Puntaje { get; set; } = 1;

    /// <summary>
    /// Dificultad del reactivo
    /// </summary>
    [BsonElement("d")]
    public DificultadReactivo? Dificultad { get; set; } = DificultadReactivo.Desconocida;

}
