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
    /// Versión del anexo, esto puede suceder por que el contenido se actualiza con un nuevo
    /// archivo y permite a los dependientes mantener la relación con un contenido anterior
    /// Se incrementa en cada PUT
    /// </summary>
    [BsonElement("v")]
    public int Version { get; set; } = 1;

    /// <summary>
    /// Indica si el contenido es el activo, en aca actualizacion el mas reciente se torna activo
    /// </summary>
    [BsonElement("a")]
    public bool Activo { get; set; }
}
