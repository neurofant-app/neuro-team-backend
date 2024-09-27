using comunes.primitivas.atributos;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace aprendizaje.model.flashcard;

/// <summary>
/// Representa una tarjeta de memorización o flashcard para el almacenamiento remoto 
/// </summary>
[ExcludeFromCodeCoverage]
[EntidadDB]
public class FlashCard: FlashcardBase
{
        
    /// <summary>
    /// Concepto del flashcard, el usuario regularmente lo ve com el título de la tarjeta 
    /// </summary>
    [BsonElement("c")]
    public List<ValorI18N<string>> Concepto { get; set; } = [];

    /// <summary>
    /// Contenido del flashcard para las de tipo multimedia y texto, puede almacenar referencia a medios existentes en la galería
    /// </summary>
    [BsonElement("cn")]
    public List<ValorI18N<string>> Contenido { get; set; } = [];

    /// <summary>
    /// Texto Texto To Spech para el estudio auditivo
    /// </summary>
    [BsonElement("txt")]
    public List<ValorI18N<string>> TextoTTS { get; set; } = [];
}
