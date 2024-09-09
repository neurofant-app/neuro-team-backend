namespace aprendizaje.model.flashcard;

/// <summary>
/// Vinculo a los medios de la galería
/// </summary>
public class VinculoContenidoGaleria
{
    /// <summary>
    /// Identificador único de la galería
    /// </summary>
    public Guid GaleriaId { get; set; }

    /// <summary>
    /// Identificador único del contenido
    /// </summary>
    public Guid ContenidoId { get; set; }

    /// <summary>
    /// Id de almacenamiento del contenido
    /// </summary>
    public string AlmacenamientoId { get; set; }
}
