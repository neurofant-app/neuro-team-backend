using aprendizaje.model.flashcard;
using comunes.primitivas;

namespace aprendizaje.services.almacenamientoNeurona;

public interface IServicioAlmacenamientoNeurona
{
    //  Crea la estructura base de folders de almacenamiento la neurona
    Task<Respuesta> CreaFolderBaseNeurona(string NeuronaId);

    // Guarda una flaschard en el almacenamiento, si la flashcard ya existe reemplaza el contenido
    Task<Respuesta> CreaActualizaFlashcard(string NeuronaId, string FlashcardId, FlashCard JsonFlashcard);

    // Elimina una flashcard del almacenamiento
    Task<Respuesta> EliminaFlashcard(string NeuronaId, string FlashcardId);

    // Obtiene una flashcard almacenada en base a su Id
    Task<RespuestaPayload<FlashCard>> ObtieneFlashcard(string NeuronaId, string FlashcardId);

    // Determina la existencia de un Blob (puede ser un directorio ó un flashcard)
    Task<Respuesta> ExisteFlashCard(string NeuronaId, string FlashcardId);
}
