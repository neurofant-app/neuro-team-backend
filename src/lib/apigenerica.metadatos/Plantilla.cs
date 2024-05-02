namespace extensibilidad.metadatos;

/// <summary>
/// Grupos de propieades de extensión de datos
/// </summary>
public class Plantilla
{
    /// <summary>
    /// IDentificador único de la plantilla
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Propiedades asociadas a la plantilla
    /// </summary>
    public List<Propiedad>? Propiedades { get; set; }
}
