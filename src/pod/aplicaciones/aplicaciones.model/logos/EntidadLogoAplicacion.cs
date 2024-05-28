using comunes.primitivas.atributos;
using comunes.primitivas.I18N;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;

namespace aplicaciones.model;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

/// <summary>
/// Logo perteneciente a una aplciación
/// </summary>
[EntidadDB]
public class EntidadLogoAplicacion : IInternacionalizable
{
    /// <summary>
    /// Identificador único del logo
    /// </summary>
    [BsonId]
    public virtual Guid Id { get; set; }
    // R

    /// <summary>
    /// Identificador único de la aplicación a la que pertenece el logo
    /// </summary>
    [BsonElement("aid")]
    public required Guid AplicacionId { get; set; }
    // R


    /// <summary>
    /// Tipo de logo
    /// </summary>
    [BsonElement("tl")]
    public TipoLogo Tipo { get; set; }
    // R


    /// <summary>
    /// Idioma del recurso
    /// </summary>
    [BsonElement("i")]
    public string Idioma { get; set; }
    // R 10


    /// <summary>
    /// Determina si el logo debe considerarce el default para un idioma no reconocido
    /// </summary>
    [BsonElement("ide")]
    public bool IdiomaDefault { get; set; }
    // R 

    /// <summary>tua
    /// Url del logotipo o imagen compatible con navegadores en Base64 
    /// </summary>
    [BsonElement("lo")]
    public string? LogoURLBase64{ get; set; }
    // R MAX

    /// <summary>
    /// Determina si el logo almacenaodo se encuentra en formato vectorial
    /// </summary>
    [BsonElement("svg")]
    public bool EsSVG { get; set; } = false;
    // R

    /// <summary>
    /// DEtermina si el logo es una URL pública
    /// </summary>
    [BsonElement("url")]
    public bool EsUrl { get; set; } = false;
    // R

    /// <summary>
    /// Aplicacion relacionada
    /// </summary>
    [BsonIgnore]
    [JsonIgnore]
    public EntidadAplicacion Aplicacion { get; set; }

}
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
