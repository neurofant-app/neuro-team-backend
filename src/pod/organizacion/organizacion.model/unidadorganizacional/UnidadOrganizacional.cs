using comunes.primitivas.atributos;
using MongoDB.Bson.Serialization.Attributes;
using System.Diagnostics.CodeAnalysis;

namespace organizacion.model.unidadorganizacional;

/// <summary>
/// Entidad de almacenamiento de la UO
/// </summary>
[ExcludeFromCodeCoverage]
[EntidadDB]
public class UnidadOrganizacional
{
    /// <summary>
    /// Identificador único de la UO
    /// </summary>
    [BsonId]
    public Guid Id { get; set; }

    /// <summary>
    /// Nombre de la unidad organizacional
    /// </summary>
    [BsonElement("nm")]
    public string Nombre { get; set; }

    /// <summary>
    /// Indica si la UO está activa
    /// </summary>
    [BsonElement("at")]
    public bool Activa { get; set; } = true;
}
