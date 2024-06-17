using comunes.primitivas.seguridad;

namespace apigenerica.primitivas.seguridad;

public interface ICacheSeguridad
{

    /// <summary>
    /// Ontiene los roles asociados al usuario en una aplicación
    /// </summary>
    /// <param name="appId"></param>
    /// <param name="usuarioId"></param>
    /// <param name="dominioId"></param>
    /// <param name="unidadOrgId"></param>
    /// <returns></returns>
    Task<List<Rol>> RolesUsuario(string appId, string usuarioId, string dominioId, string unidadOrgId);


    /// <summary>
    /// Obtiene la lista de permisos asociados al usuario en una aplicacin
    /// </summary>
    /// <param name="appId"></param>
    /// <param name="usuarioId"></param>
    /// <param name="dominioId"></param>
    /// <param name="unidadOrgId"></param>
    /// <returns></returns>
    Task<List<Permiso>> PermisosUsuario(string appId, string usuarioId, string dominioId, string unidadOrgId);

}
