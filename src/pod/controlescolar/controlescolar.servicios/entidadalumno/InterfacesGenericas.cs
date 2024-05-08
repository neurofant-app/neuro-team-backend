
using apigenerica.model.servicios;
using controlescolar.modelo.alumnos;
using controlescolar.modelo.campi;

namespace controlescolar.servicios.entidadalumno;

public interface IServicioEntidadAlumno : IServicioEntidadGenerica<EntidadAlumno, CreaAlumno, ActualizaAlumno, ConsultaAlumno, string>
{
}
