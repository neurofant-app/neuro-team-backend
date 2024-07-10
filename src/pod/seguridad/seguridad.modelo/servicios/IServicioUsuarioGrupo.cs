using apigenerica.model.servicios;
using seguridad.modelo.roles;

namespace seguridad.modelo.servicios;

public interface IServicioUsuarioGrupo : IServicioEntidadHijoGenerica<UsuarioGrupo, CreaUsuarioGrupo, UsuarioGrupo, ConsultaUsuarioGrupo, string, string>
{
}

