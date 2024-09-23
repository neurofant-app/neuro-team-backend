using apigenerica.model.servicios;
using aprendizaje.model.flashcard;
using comunes.primitivas;

namespace aprendizaje.services.neurona.flashcard;

public interface IServicioFlashCard : IServicioEntidadHijoGenerica<FlashCard, FlashCard, FlashCard, FlashCard, string, string>
{ 
    Task<ResultadoValidacion> ValidacionActualizarFlashCard(string id, FlashCard actualizacion);
    Task<ResultadoValidacion> ValidacionEliminarFlashCard(string id);
}
