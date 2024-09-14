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
    /// Id único del contenido almacenado, se utiliza como identificador de versiones para el contenido 
    /// Cuando un Contenido camvbia se crea un nuevo archivo que tendrá un nuevo ID de almacenamiento
    /// </summary>
    [BsonElement("alid")]
    public required string AlmacenamientoId { get; set; }

    /// <summary>
    /// Tamaño del contenido en bytes
    /// </summary>
    [BsonElement("t")]
    public long Tamano { get; set; } = 0;

    /// <summary>
    /// Tipo MIME del contenido
    /// </summary>
    [BsonElement("tm")]
    public required string TipoMime { get; set; }

    /// <summary>
    /// Indica si el contenido es el activo, en aca actualizacion el mas reciente se torna activo
    /// </summary>
    [BsonElement("a")]
    public bool Activo { get; set; }
}
