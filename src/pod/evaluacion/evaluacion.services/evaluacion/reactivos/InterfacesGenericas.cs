using apigenerica.model.servicios;
using evaluacion.model.evaluacion.temas;
using evaluacion.model.reactivos;

namespace evaluacion.services.evaluacion;

public interface IServicioReactivo : IServicioEntidadGenerica<ReactivoTema, ReactivoCrear, ReactivoActualizar, ReactivoTema, Guid>
{
}
