using MongoDB.Bson.Serialization.Attributes;

namespace seguridad.modelo.instancias;

/// <summary>
/// Extiendoe los datos de la instancia para un dominio
/// </summary>
public class InstanciaAplicacion
{

    /// <summary>
    /// Identificador único de la dominio en el que aplica la configuracion, este Id será propoerionado por un sistema externo
    /// </summary>
    [BsonId(Order = 0)]
    public required string DominioId { get; set; }

    /// <summary>
    /// Identificador único de la aplicación, este Id será propoerionado por un sistema externo
    /// </summary>
    [BsonId(Order = 1)]
    public required string ApplicacionId { get; set; }

    /// <summary>
    /// Roles persoalizados asociados a la aplicación del dominio
    /// </summary>
    [BsonElement("r")]
    public List<Rol> RolesPersonalizados { get; set; } = [];

    /// <summary>
    /// Lista detalle de miembros de un rol
    /// </summary>
    [BsonElement("mr")]
    public List<MiembrosRol> MiembrosRol { get; set; } = [];

    /// <summary>
    /// Lista detalle de miembros con un permiso individual asociado
    /// </summary>
    [BsonElement("mp")]
    public List<MiembrosPermiso> MiembrosPermiso { get; set; } = [];
}
