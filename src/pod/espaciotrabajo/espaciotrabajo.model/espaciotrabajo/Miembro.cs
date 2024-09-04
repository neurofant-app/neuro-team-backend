using MongoDB.Bson.Serialization.Attributes;

namespace espaciotrabajo.model.espaciotrabajo;

/// <summary>
/// Define un miembro del espacio de trabajo
/// </summary>
public class Miembro
{
    /// <summary>
    /// Identificador único del miembro
    /// </summary>
    [BsonElement("i")]
    public required string UsuarioId { get; set; }

}
