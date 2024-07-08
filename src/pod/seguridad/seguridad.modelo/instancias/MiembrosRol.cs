using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;

namespace seguridad.modelo.instancias;

/// <summary>
/// Almacena el vínculo entre usuarios y roles
/// </summary>
public class MiembrosRol
{
    /// <summary>
    /// IDentificador unico del rol
    /// </summary>
    [BsonElement("rid")]
    public required string RolId { get; set; }

    /// <summary>
    /// Lista de los usuarios asociados al rol
    /// </summary>
    [BsonElement("uid")]
    public List<string> UsuarioId { get; set; } = [];

    /// <summary>
    /// Lista de los grupos asociados al rol
    /// </summary>
    [BsonElement("gid")]
    public List<string> GrupoId { get; set; } = [];
}
