using comunes.primitivas.atributos;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Diagnostics.CodeAnalysis;

namespace organizacion.model.usuariodominio;

/// <summary>
/// Entidad de los usuarios asociados al dominio
/// </summary>
[ExcludeFromCodeCoverage]
[EntidadDB]
public class UsuarioDominio
{
    /// <summary>
    /// Identificador único del usuario en el dominio
    /// </summary>
    [BsonId]
    public Guid Id { get; set; }

    /// <summary>
    /// Lista de identificadores de los dominios a los que el usuario pertenece
    /// </summary>
    [BsonElement("dmid")]
    public List<Guid> DominiosId { get; set; } = [];
    // Indexar

    /// <summary>
    /// Lista de dominios a los que el usuario está asociado 
    /// </summary>
    [BsonElement("doms")]
    public List<ElementoDominio> Dominios { get; set; } = [];

}
