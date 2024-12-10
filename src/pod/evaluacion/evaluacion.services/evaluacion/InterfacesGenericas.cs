using apigenerica.model.servicios;
using evaluacion.model.evaluacion;

namespace evaluacion.services.evaluacion;

public interface IServicioEvaluacion : IServicioEntidadGenerica<Evaluacion, EvaluacionInsertar, EvaluacionActualizar, EvaluacionDespliegue, Guid>
{
}
