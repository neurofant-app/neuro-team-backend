using comunes.primitivas.seguridad;

namespace comunes.interservicio.primitivas;


public interface IProxySeguridad
{
    Task ActualizaSeguridad(List<Aplicacion> apps);

    /// <summary>
    /// Ontiene los roles asociados al usuario en una aplicación
    /// </summary>
    /// <param name="appId"></param>
    /// <param name="usuarioId"></param>
    /// <returns></returns>
    Task<List<Rol>> RolesUsuario(string appId, string usuarioId, string dominioId, string unidadOrgId);


    /// <summary>
    /// Obtiene la lista de permisos asociados al usuario en una aplicacin
    /// </summary>
    /// <param name="appId"></param>
    /// <param name="usuarioId"></param>
    /// <returns></returns>
    Task<List<Permiso>> PermisosUsuario(string appId, string usuarioId, string dominioId, string unidadOrgId);

}
