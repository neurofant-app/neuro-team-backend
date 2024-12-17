using apigenerica.model.servicios;
using comunes.primitivas;
using evaluacion.model;
using evaluacion.model.evaluacion;
using evaluacion.model.reactivos;

namespace evaluacion.services.evaluacion;

public interface IServicioEvaluacion : IServicioEntidadGenerica<Evaluacion, EvaluacionInsertar, EvaluacionActualizar, EvaluacionDespliegue, Guid>
{
    Task<Respuesta> CambiarEstado(Guid evaluacionId, EstadoEvaluacion estadoEvaluacion);
    Task<Respuesta> ReactivoMultipleCrear(Guid evaluacionId, ReactivoMultipleCrear reactivos);
    Task<Respuesta> ReactivoMultipleEliminar(Guid evaluacionId, ReactivoMultipleEliminar reactivos);
}
