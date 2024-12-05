using comunes.primitivas.atributos;
using MongoDB.Bson.Serialization.Attributes;
using System.Diagnostics.CodeAnalysis;

namespace organizacion.model.usuariodominio;

/// <summary>
/// DTO para la inserción de unidades organizacionales en el dominio
/// </summary>
[ExcludeFromCodeCoverage]
[EntidadDB]
public class ElementoOU
{
    /// <summary>
    /// Identificador único de la unidad organizacional
    /// </summary>
    [BsonId]
    public Guid OUId { get; set; }

    /// <summary>
    /// IDentifica si la OU se encuentra activa
    /// </summary>
    [BsonElement("ac")]
    public bool Activa { get; set; }
}
