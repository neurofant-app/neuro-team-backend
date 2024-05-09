using comunes.primitivas.atributos;
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;

namespace aplicaciones.model;
[EntidadDB]
public class EntidadConsentimiento
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
    [BsonElement("ti")]
    public TipoConsentimiento Tipo { get; set; }

    /// <summary>
    /// Idioma del recurso
    /// </summary>
    [BsonElement("i")]
    public string Idioma { get; set; }
    // 10 R

    /// <summary>
    /// Determina si el logo debe considerarce el default para un idioma no reconocido
    /// </summary>
    [BsonElement("ide")]
    public bool IdiomaDefault { get; set; }
    // R

    /// <summary>
    /// Url del logotipo o imagen compatible con navegadores en Base64 
    /// </summary>
    /// 
    [BsonElement("txt")]
    public string Texto { get; set; }
    // MAXIMO R

    /// <summary>
    /// Aplicacion relacionada
    /// </summary>
    [BsonIgnore]
    [JsonIgnore]
    public EntidadAplicacion Aplicacion { get; set; }
}
