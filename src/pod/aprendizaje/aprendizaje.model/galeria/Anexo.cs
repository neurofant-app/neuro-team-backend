using MongoDB.Bson.Serialization.Attributes;
using System.Diagnostics.CodeAnalysis;

namespace aprendizaje.model.galeria;


/// <summary>
/// Información del contenido
/// </summary>
[ExcludeFromCodeCoverage]
public class Anexo
{
    /// <summary>
    /// Id único del contenido almacenado
    /// </summary>
    [BsonId]
    public required string AlmacenamientoId { get; set; }

    /// <summary>
    /// Tamaño del contenido en bytes
    /// </summary>
    [BsonElement("t")]
    public long Tamano { get; set; } = 0;

    /// <summary>
    /// Versión del contenido, se actualiza automáticamente en cada PUT 
    /// siemrpe y cuando el hash del contenido sea distinto al de la versión más reciente
    /// </summary>
    [BsonElement("v")]
    public int Version { get; set; } = 1;

}
