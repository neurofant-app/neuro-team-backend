using comunes.primitivas.atributos;
using seguridad.modelo.relaciones;
using System.Text.Json.Serialization;
namespace seguridad.modelo.instancias;

/// <summary>
/// Extiendoe los datos de la instancia para un dominio
/// </summary>
[EntidadDB]
public class InstanciaAplicacionMysql{

    /// <summary>
    /// Identificador único de la instancia de configuración
    /// </summary>

    public string? Id { get; set; }

    /// <summary>
    /// Identificador único de la dominio en el que aplica la configuracion, este Id será propoerionado por un sistema externo
    /// </summary>

    public required string DominioId { get; set; }

    /// <summary>
    /// Identificador único de la aplicación, este Id será propoerionado por un sistema externo
    /// </summary>
 
    public required Guid ApplicacionId { get; set; }

    /// <summary>
    /// Roles persoalizados asociados a la aplicación del dominio
    /// </summary>

    public List<Rol> RolesPersonalizados { get; set; } = [];


   [JsonIgnore]
    public Aplicacion Aplicacion { get; set; }
    public List<RolGrupo> RolGrupo { get; set; } 
    public List<RolUsuario> RolUsuarios{ get; set; }
    public List<PermisoGrupo> PermisoGrupo { get; set; }
    public List<PermisoUsuario> PermisoUsuarios { get; set; }

}
