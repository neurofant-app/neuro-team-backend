using comunes.primitivas.atributos;
using MongoDB.Bson.Serialization.Attributes;
using seguridad.modelo.relaciones;
using System.Text.Json.Serialization;

namespace seguridad.modelo.instancias;

/// <summary>
/// Extiendoe los datos de la instancia para un dominio
/// </summary>
[EntidadDB]
public class InstanciaAplicacion
{

    /// <summary>
    /// Identificador único de la instancia de configuración
    /// </summary>
    [BsonId]
    public virtual string? Id { get; set; }

    /// <summary>
    /// Identificador único de la dominio en el que aplica la configuracion, este Id será propoerionado por un sistema externo
    /// </summary>
    [BsonElement("did")]
    public virtual required string DominioId { get; set; }

    /// <summary>
    /// Identificador único de la aplicación, este Id será propoerionado por un sistema externo
    /// </summary>
    [BsonElement("aid")]
    public virtual required Guid ApplicacionId { get; set; }

    /// <summary>
    /// Roles persoalizados asociados a la aplicación del dominio
    /// </summary>
    [BsonElement("r")]
    public virtual List<Rol> RolesPersonalizados { get; set; } = [];

    /// <summary>
    /// Lista detalle de miembros de un rol
    /// </summary>
    [BsonElement("mr")]

    public virtual List<MiembrosRol> MiembrosRol { get; set; } = [];

    /// <summary>
    /// Lista detalle de miembros con un permiso individual asociado
    /// </summary>
    [BsonElement("mp")]
    public virtual List<MiembrosPermiso> MiembrosPermiso { get; set; } = [];

    #region MySQL
    // Propeidades relacionales utilizadas por MySQL

    [JsonIgnore]
    [BsonIgnore]
    public Aplicacion? Aplicacion { get; set; }


    [BsonIgnore]
    public List<RolGrupo>? RolGrupo { get; set; }

    [BsonIgnore]
    public List<RolUsuario>? RolUsuarios { get; set; }

    [BsonIgnore]
    public List<PermisoGrupo>? PermisoGrupo { get; set; }
 
    [BsonIgnore]
    public List<PermisoUsuario>? PermisoUsuarios { get; set; }
    
    #endregion

}
