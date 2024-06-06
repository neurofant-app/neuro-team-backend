namespace comunes.primitivas.seguridad;

/// <summary>
/// Define un periso aplicable 
/// </summary>
public class Permiso
{
    /// <summary>
    /// Identificador único del permido, se utiliza como clave para los permisos por rol y para la i18N, debe ser único en la lista de permisos de una app
    /// </summary>
    public required string PermisoId { get; set; }

    /// <summary>
    /// Determina el ámbito de apliación del permiso
    /// </summary>
    public AmbitoPermiso Ambito { get; set; }


    /// <summary>
    /// Nombre del permiso para la UI, esto será calcolado en base al idioa
    /// </summary>
    public string? Nombre { get; set; }

    /// <summary>
    /// Descripción del permiso para la UI, esto será calcolado en base al idioa
    /// </summary>
    public string? Descripcion { get; set; }
}
