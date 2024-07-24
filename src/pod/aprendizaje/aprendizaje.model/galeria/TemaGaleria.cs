using comunes.primitivas.atributos;
using MongoDB.Bson.Serialization.Attributes;
using System.Diagnostics.CodeAnalysis;

namespace aprendizaje.model.galeria;

/// <summary>
/// Define temas o tags para los elementos de la galería
/// </summary>
[ExcludeFromCodeCoverage]
[EntidadDB]
public class TemaGaleria
{
    /// <summary>
    /// Identificador único del tema
    /// </summary>
    [BsonId]
    public int Id { get; set; }

    /// <summary>
    /// Nombre del tema
    /// </summary>
    [BsonElement("n")]
    public required string Nombre { get; set; }
}
