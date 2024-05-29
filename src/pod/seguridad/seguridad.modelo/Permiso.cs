using MongoDB.Bson.Serialization.Attributes;

namespace seguridad.modelo;

/// <summary>
/// Define un periso aplicable 
/// </summary>
public class Permiso
{
    /// <summary>
    /// Identificador único del permido, se utiliza como clave para los permisos por rol y para la i18N, debe ser único en la lista de permisos de una app
    /// </summary>
    [BsonElement("pid")]
    public required string PermisoId { get; set; }

    /// <summary>
    /// Determina el ámbito de apliación del permiso
    /// </summary>
    [BsonElement("pa")]
    public AmbitoPermiso Ambito { get; set; }


    /// <summary>
    /// Nombre del permiso para la UI, esto será calcolado en base al idioa
    /// </summary>
    [BsonElement("pn")]
    public string? Nombre { get; set; }

    /// <summary>
    /// Descripción del permiso para la UI, esto será calcolado en base al idioa
    /// </summary>
    [BsonElement("pd")]
    public string? Descripcion { get; set; }
}
