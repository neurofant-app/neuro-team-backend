using MongoDB.Bson.Serialization.Attributes;

namespace seguridad.modelo;

/// <summary>
/// Rol de seguridad asociado a una apliación
/// </summary>
public class Rol
{
    /// <summary>
    /// Identificador único del rol, se utiliza como clave para los roles y para la i18N, debe ser único en la lista de permisos de una app
    /// </summary>
    [BsonElement("id")]
    public required string RolId { get; set; }

    /// <summary>
    /// Lista de los identificadores de permisos asociados al rol
    /// </summary>
    [BsonElement("p")]
    public List<string> Permisos { get; set; } = [];

    /// <summary>
    /// DEfine si un rol ha sido creado por el administrador de sistema
    /// </summary>
    [BsonElement("c")]
    public bool Personalizado { get; set; }

    /// <summary>
    /// Nombre del rol para la UI, esto será calcolado en base al idioma o bien al crear roles personalizados
    /// </summary>
    [BsonElement("n")]
    public string? Nombre { get; set; }

    /// <summary>
    /// Descripción del rol para la UI, esto será calcolado en base al idioma o bien al crear roles personalizados
    /// </summary>
    [BsonElement("d")]
    public string? Descripcion { get; set; }
}
