
namespace comunes.primitivas.seguridad;


/// <summary>
/// Módulo de una aplicacion
/// </summary>
public class Modulo
{
    /// <summary>
    /// Identificador único del módulo, este Id será propoerionado por un sistema externo
    /// </summary>
    public required string ModuloId { get; set; }

    /// <summary>
    /// Lista de permisos asociados al modulo
    /// </summary>
    public List<Permiso> Permisos { get; set; } = [];

    /// <summary>
    /// Roles definidos para el módulo asociados a un conjunto de permisos
    /// </summary>
    public List<Rol> RolesPredefinidos { get; set; } = [];

    /// <summary>
    /// Nombre del módulo para la UI, esto será calcolado en base al idioa
    /// </summary>
    public string? Nombre { get; set; }

    /// <summary>
    /// Descripción del módulo para la UI, esto será calcolado en base al idioa
    /// </summary>
    public string? Descripcion { get; set; }

}
