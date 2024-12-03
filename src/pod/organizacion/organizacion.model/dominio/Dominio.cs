using comunes.primitivas.atributos;
using MongoDB.Bson.Serialization.Attributes;
using organizacion.model.unidadorganizacional;
using System.Diagnostics.CodeAnalysis;

namespace organizacion.model.dominio;

/// <summary>
/// El dominio es el contenedor de todos los recursos de una cuenta
/// </summary>
[ExcludeFromCodeCoverage]
[EntidadDB]
public class Dominio
{
    /// <summary>
    /// Identificador único del dominio
    /// </summary>
    [BsonId]
    public Guid Id { get; set; }

    /// <summary>
    /// Nombre de dominio
    /// </summary>
    [BsonElement("n")]
    public string Nombre { get; set; }

    /// <summary>
    /// Identificador único del origen del dominio
    /// </summary>
    [BsonElement("o")]
    public Guid OrigenId { get; set; }

    /// <summary>
    /// Tipo de origen del dominio
    /// </summary>
    [BsonElement("t")]
    public TipoOrigenDominio TipoOrigen { get; set; } = TipoOrigenDominio.Usuario;

    /// <summary>
    /// Indica si el dominio está activo
    /// </summary>
    [BsonElement("act")]
    public bool Activo { get; set; } = true;


    /// <summary>
    /// Unidades organizacionales asociadas al dominio
    /// </summary>
    [BsonElement("uos")]
    public List<UnidadOrganizacional> UnidadesOrganizacionales { get; set; } = [];

 
}
