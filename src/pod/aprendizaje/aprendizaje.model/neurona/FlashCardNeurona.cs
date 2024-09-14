using aprendizaje.model.flashcard;
using MongoDB.Bson.Serialization.Attributes;

namespace aprendizaje.model.neurona;

//  - - - DETALLES
//  
//  Esta entidad se almacena en mongo
//
//  - - - 


/// <summary>
/// Define el vínculo entre una neurona y sus flashcards
/// </summary>
public class FlashcardNeurona
{
    /// <summary>
    /// Identificador único de la flashcard
    /// </summary>
    [BsonElement("i")]
    public long FlashcardId { get; set; }

    /// <summary>
    /// Estado de la tarjeta
    /// </summary>
    [BsonElement("e")]
    public EstadoContenido Estado { get; set; }

    /// <summary>
    /// Identificador del usuario que creo la tarjeta
    /// </summary>
    public required string UsuarioId { get; set; }

    /// <summary>
    /// Fecha de creación de la tarjeta
    /// </summary>
    [BsonElement("f")]
    public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// IDentificador único del tema al que pertenece la tarjeta
    /// </summary>
    [BsonElement("ti")]
    public Guid TemaId { get; set; }

    /// <summary>
    /// Determina el tipo base para el concepto de la tarjeta
    /// Sólo son adminitidos los tipos simples
    /// </summary>
    [BsonElement("y")]
    public TipoBaseFlashcard TipoConcepto { get; set; } = TipoBaseFlashcard.Texto;

    /// <summary>
    /// Determina el tipo base del contenido de la tarjeta
    /// </summary>
    [BsonElement("z")]
    public TipoBaseFlashcard TipoContendo { get; set; }

    /// <summary>
    /// Si es true, sincroniza automáticamente los cambios del contenido en la galería
    /// </summary>
    [BsonElement("sg")]
    public bool SincronizarGaleria { get; set; } = false;
}

