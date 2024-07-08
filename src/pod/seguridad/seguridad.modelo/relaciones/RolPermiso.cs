
using MongoDB.Bson.Serialization.Attributes;

namespace seguridad.modelo;

public class RolPermiso
{

    public required string Id { get; set; }
    public required string RolId { get; set; }
    public required string PermisoId { get; set; }

    public Permiso Permiso { get; set; }

    public Rol Rol { get; set; }
}
