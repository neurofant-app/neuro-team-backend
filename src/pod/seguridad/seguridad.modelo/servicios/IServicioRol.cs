using apigenerica.model.servicios;
using seguridad.modelo.roles;

namespace seguridad.modelo.servicios;

public interface IServicioRol : IServicioEntidadHijoGenerica<Rol, CreaRol, ActualizaRol, ConsultaRol, string, string>
{
}