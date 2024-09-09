namespace aprendizaje.model.flashcard;
public enum TipoBaseFlashcard
{
    /// <summary>
    /// Sólo contiene texto
    /// </summary>
    Texto = 0,
    /// <summary>
    /// Sólo contiene una imagen
    /// </summary>
    Imagen = 1,
    /// <summary>
    /// Sólo contiene un audio
    /// </summary>
    Audio = 2,
    /// <summary>
    /// Sólo contiene un video
    /// </summary>
    Video = 3,
    /// <summary>
    /// Contenido multimedia en forma de un HTML
    /// </summary>
    Multimedia = 4,
    /// <summary>
    /// Flascards personalizadas por 3os
    /// </summary>
    Personalizado = 5
}
