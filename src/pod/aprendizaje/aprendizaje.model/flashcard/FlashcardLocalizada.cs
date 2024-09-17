namespace aprendizaje.model.flashcard;

/// <summary>
/// REpreserntación de la flashcar para su almacenamiento en la base de datos 
/// del lado del cliente, por ejemplo cifrada en SQLLite
/// 
/// Se encuentra localizada en el idioma de adquisición de la neurona
/// 
/// </summary>
public class FlashcardLocalizada: FlashcardBase
{
    /// <summary>
    /// Concepto del flashcard, el usuario regularmente lo ve com el título de la tarjeta 
    /// </summary>
    public required string Concepto { get; set; }

    /// <summary>
    /// Contenido del flashcard para las de tipo multimedia y texto, 
    /// puede almacenar referencia a medios existentes en la galería
    /// </summary>
    public required string Contenido { get; set; }

    /// <summary>
    /// Texto Texto To Spech para el estudio auditivo
    /// </summary>
    public string? TextoTTS { get; set; }
}
