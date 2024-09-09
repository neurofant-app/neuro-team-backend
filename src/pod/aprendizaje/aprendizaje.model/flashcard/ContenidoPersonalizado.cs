using MongoDB.Bson.Serialization.Attributes;

namespace aprendizaje.model.flashcard;

/// <summary>
/// Define tipos de contenido personalizado para las flashcards
/// El contenido debe ser una aplicación SPA de HTML que pueda accederse o descargarse en línea
/// </summary>
public class ContenidoPersonalizado
{
    /// <summary>
    /// Identificador único del tipo de flashcard
    /// </summary>
    [BsonId]
    public required Guid Id { get; set; }

    /// <summary>
    /// Nombre del tipo de flashcard
    /// </summary>
    [BsonElement("n")]
    public required ValorI18N<string> Nombre { get; set; }

    // Este objeto es para uso futuro 
    // permitirá definir flascahrds personalizados
    // creados por terceros
    // PENTIENTE DE DISEÑO
}
