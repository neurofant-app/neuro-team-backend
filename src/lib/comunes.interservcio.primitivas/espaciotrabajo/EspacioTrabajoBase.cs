namespace comunes.interservicio.primitivas;

/// <summary>
/// Información básica de un espacio de trabajo
/// </summary>
public class EspacioTrabajoBase
{
    /// <summary>
    /// Identificador del espacio de trabajo
    /// </summary>
    public required Guid Id { get; set; }

    /// <summary>
    /// Nombre del espacio de trabajo
    /// </summary>
    public required string Nombre { get; set; }

    /// <summary>
    /// Listado de permisos asociados al usuario en el espacio de trabajo
    /// </summary>
    public List<string> Permisos { get; set; } = [];


    /// <summary>
    /// Listado de roles asociados al usuario en el espacio de trabajo
    /// </summary>
    public List<string> Roles { get; set; } = [];


}
