namespace aplicaciones.model;

public class PlantillaInvitacionInsertar
{
    /// <summary>
    /// Tipo de contenido de la plantilla
    /// </summary>
    public TipoContenido TipoContenido { get; set; }
    // Requerida
    // [I] [A] [D]

    /// <summary>
    /// Identificador único de la aplicación a la qu pertenece la plantilla
    /// </summary>
    public Guid AplicacionId { get; set; }
    // Requerida
    // [I] [A] [D]

    /// <summary>
    /// Contenido para la plantilla
    /// </summary>
    public required string Plantilla { get; set; }
    // Requerida TAMAÑO MAXIMO EN LA BASE DE DATOS
    // [I [A] [D]
}
