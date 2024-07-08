using apigenerica.model.servicios;
using seguridad.modelo;
using seguridad.modelo.roles;


namespace seguridad.servicios.mysql;

public interface IServicioRol : IServicioEntidadHijoGenerica<Rol, CreaRol, ActualizaRol, ConsultaRol, string,string>
{
}
