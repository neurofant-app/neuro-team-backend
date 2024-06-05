using apigenerica.model.servicios;
using controlescolar.modelo.instructores;

namespace controlescolar.servicios;

public interface IServicioEntidadInstructor : IServicioEntidadGenerica<EntidadInstructor, CreaInstructor, ActualizaInstructor, ConsultaInstructor, string>
{
}
