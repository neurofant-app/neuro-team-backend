
using apigenerica.model.servicios;
using seguridad.modelo.roles;
using seguridad.modelo;

namespace seguridad.servicios.usuariosgrupo;

public interface IServicioUsuarioGrupo : IServicioEntidadHijoGenerica<UsuarioGrupo, CreaUsuarioGrupo, UsuarioGrupo, ConsultaUsuarioGrupo, string, string>
{
}

