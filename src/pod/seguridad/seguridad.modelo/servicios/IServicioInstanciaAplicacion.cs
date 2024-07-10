using apigenerica.model.servicios;
using seguridad.modelo.instancias;

namespace seguridad.modelo.servicios;

public interface IServicioInstanciaAplicacion : IServicioEntidadGenerica<InstanciaAplicacion, InstanciaAplicacion, InstanciaAplicacion, InstanciaAplicacion, string>
{
    Task<List<Rol>> GetRolesUsuarioInterno(string aplicacionId, string usuarioId, string dominioId, string uOrgID);
    Task<List<Permiso>> GetPermisosAplicacionInterno(string aplicacionId, string usuarioId, string dominioId, string uOrgID);
}

