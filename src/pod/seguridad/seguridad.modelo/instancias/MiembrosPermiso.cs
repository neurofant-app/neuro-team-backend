using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;

namespace seguridad.modelo.instancias;

/// <summary>
/// Espeifica los usuarios/grupos con un permiso individual asociado
/// </summary>
public class MiembrosPermiso
{
    /// <summary>
    /// IDentificador unico del permiso
    /// </summary>
    [BsonElement("pid")]
    public required string RolId { get; set; }

    /// <summary>
    /// Lista de los usuarios asociados al permiso
    /// </summary>
    [BsonElement("uid")]
    public List<string> UsuarioId { get; set; } = [];

    /// <summary>
    /// Lista de los grupos asociados al permiso
    /// </summary>
    [BsonElement("gid")]
    public List<string> GrupoId { get; set; } = [];
}
