using comunes.primitivas.atributos;
using MongoDB.Bson.Serialization.Attributes;
using System.Diagnostics.CodeAnalysis;

namespace organizacion.model.usuariodominio;

/// <summary>
/// Define los vinculos de un usuario con un dominio
/// </summary>
[ExcludeFromCodeCoverage]
[EntidadDB]
public class ElementoDominio
{
    /// <summary>
    /// Identificador único del dominio al que pertenece el usuario
    /// </summary>
    [BsonId]
    public Guid DominioId { get; set; }

    /// <summary>
    /// Indica si el usuario está activo en el dominio, si el usuario se encuentra inactivo también lo estará para todas las UO
    /// </summary>
    [BsonElement("a")]
    public bool Activo { get; set; } = true;

    /// <summary>
    /// Lista de unidades organizacionlaes a las que pertenece el usuario
    /// </summary>
    [BsonElement("ou")]
    public List<ElementoOU> OUIds { get; set; } = [];

}
