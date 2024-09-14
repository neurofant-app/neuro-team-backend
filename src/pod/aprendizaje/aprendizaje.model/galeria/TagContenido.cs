using comunes.primitivas.atributos;
using MongoDB.Bson.Serialization.Attributes;
using System.Diagnostics.CodeAnalysis;

namespace aprendizaje.model.galeria;

/// <summary>
/// Define tags para los elementos de la galería
/// </summary>
[ExcludeFromCodeCoverage]
[EntidadDB]
public class TagContenido
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
    public List<ValorI18N<string>> Tag { get; set; } = [];
}
