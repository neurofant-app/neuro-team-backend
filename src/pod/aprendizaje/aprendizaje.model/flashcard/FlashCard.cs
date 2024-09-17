using System.Diagnostics.CodeAnalysis;

namespace aprendizaje.model.flashcard;

/// <summary>
/// Representa una tarjeta de memorización o flashcard para el almacenamiento remoto 
/// </summary>
[ExcludeFromCodeCoverage]
public class FlashCard: FlashcardBase
{
    /// <summary>
    /// Concepto del flashcard, el usuario regularmente lo ve com el título de la tarjeta 
    /// </summary>
    public List<ValorI18N<string>> Concepto { get; set; } = [];

    /// <summary>
    /// Contenido del flashcard para las de tipo multimedia y texto, puede almacenar referencia a medios existentes en la galería
    /// </summary>
    public List<ValorI18N<string>> Contenido { get; set; } = [];

    /// <summary>
    /// Texto Texto To Spech para el estudio auditivo
    /// </summary>
    public List<ValorI18N<string>> TextoTTS { get; set; } = [];
}
