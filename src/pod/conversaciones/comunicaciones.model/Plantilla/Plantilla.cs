using MongoDB.Bson.Serialization.Attributes;

namespace comunicaciones.model;
/// <summary>
/// Plantilla para envio de mensaje
/// </summary>
public class Plantilla
{
    /// <summary>
    /// Identificador único de la plantilla puede ser autogenerado o definido
    /// como un conjunto de valores.
    /// </summary>
    [BsonId]
    public string Id { get; set; }
    [BsonElement("lc")]
    public List<Contenido> Contenidos { get; set; }
    /// <summary>
    /// Identificador de la aplicación  que utiliza la plantilla
    /// </summary>
    [BsonElement("aid")]
    public string? AplicacionId { get; set; }
    /// <summary>
    /// Es una plantilla utilizada por el usuario
    /// </summary>
    [BsonElement("du")]
    public bool DeUsuario { get; set; }
    /// <summary>
    /// Usuario creado por la plantilla
    /// </summary>
    [BsonElement("uid")]
    public string? UsuarioId { get; set; }
    [BsonElement("fc")]
    public DateTime FechaCreacion { get; set; }
    [BsonElement("fe")]
    public DateTime? FechaEnvio { get; set; }
}
