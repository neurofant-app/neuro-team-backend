﻿using comunes.primitivas.seguridad;

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
    Task<Rol> RolesUsuario(string appId, string usuarioId);


    /// <summary>
    /// Obtiene la lista de permisos asociados al usuario en una aplicacin
    /// </summary>
    /// <param name="appId"></param>
    /// <param name="usuarioId"></param>
    /// <returns></returns>
    Task<Permiso> PermisosUsuario(string appId, string usuarioId);

}
