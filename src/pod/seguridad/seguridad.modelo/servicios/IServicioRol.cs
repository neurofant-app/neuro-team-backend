using apigenerica.model.servicios;
using seguridad.modelo.roles;

namespace seguridad.modelo.servicios;

public interface IServicioRol : IServicioEntidadGenerica<Rol, CreaRol, ActualizaRol, ConsultaRol, string>
{
}