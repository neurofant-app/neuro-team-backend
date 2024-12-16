using MongoDB.Bson.Serialization.Attributes;
using System.Diagnostics.CodeAnalysis;

namespace evaluacion.model.participantes;

/// <summary>
/// Imagenes utilizadas para la evaluación
/// </summary>
[ExcludeFromCodeCoverage]
public class ImagenEvaluacion
{
    /// <summary>
    /// Posición de la image
    /// </summary>
    [BsonElement("i")]
    public int Indice { get; set; }

    /// <summary>
    /// Identificadores de las imagenes utilziadas para la calificación automática  
    /// </summary>
    [BsonElement("ai")]
    public string SistemaAlmacenamientoId { get; set; }
}
