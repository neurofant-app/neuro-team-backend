using MongoDB.Bson.Serialization.Attributes;

namespace aplicaciones.model;

public class CreaAplicacion
{
    /// <summary>
    /// Nombre de la aplicación que emite la invitación
    /// </summary>
    public required string Nombre { get; set; }

    /// <summary>
    /// Especifica si la aplicación se encuentra activa, solo es posible emitir notificaciones so lo está
    /// </summary>
    public bool Activa { get; set; }

    /// <summary>
    /// Clave de la aplicación
    /// </summary>
    [BsonElement("k")]
    public required string Clave { get; set; }

    /// <summary>
    /// Lista de Hosts asoiados a la aplicación
    /// </summary>
    [BsonElement("u")]
    public List<string>? Hosts { get; set; }

    /// <summary>
    /// Determina si la configuración es la de default
    /// </summary>
    [BsonElement("d")]
    public bool Default { get; set; } = false;
}
