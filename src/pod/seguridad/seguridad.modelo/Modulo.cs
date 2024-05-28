using MongoDB.Bson.Serialization.Attributes;

namespace seguridad.modelo;


/// <summary>
/// Módulo de una aplicacion
/// </summary>
public class Modulo
{
    /// <summary>
    /// Identificador único del módulo, este Id será propoerionado por un sistema externo
    /// </summary>
    [BsonElement("mid")]
    public required string ModuloId { get; set; }

    /// <summary>
    /// Lista de permisos asociados al modulo
    /// </summary>
    [BsonElement("mp")]
    public List<Permiso> Permisos { get; set; } = [];

    /// <summary>
    /// Roles definidos para el módulo asociados a un conjunto de permisos
    /// </summary>
    [BsonElement("mr")]
    public List<Rol> RolesPredefinidos { get; set; } = [];

    /// <summary>
    /// Nombre del módulo para la UI, esto será calcolado en base al idioa
    /// </summary>
    [BsonElement("mn")]
    public string? Nombre { get; set; }

    /// <summary>
    /// Descripción del módulo para la UI, esto será calcolado en base al idioa
    /// </summary>
    [BsonElement("md")]
    public string? Descripcion { get; set; }

}
