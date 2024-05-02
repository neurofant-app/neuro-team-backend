using System.Text.Json.Serialization;

namespace aplicaciones.model;

/// <summary>
/// Define laplantilla de contenido para las comunicaciones con los usuarios del sistema
/// </summary>
public class PlantillaInvitacion
{
    /// <summary>
    /// Identificador único de la plantila
    /// </summary>
    public Guid Id { get; set; }
    // Requerida
    // [A] [D]

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
    //


    /// <summary>
    /// Aplicación asociada a la invitación
    /// </summary>
    [JsonIgnore]
    public Aplicacion Aplicacion { get; set; }
}
