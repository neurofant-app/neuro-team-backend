namespace comunes.interservicio.primitivas;

/// <summary>
/// Proxy para solicitar datos de los espacios de trabajo
/// </summary>
public interface IProxyEspacioTrabajo
{

    /// <summary>
    /// Obtiene la lista de espacios de trabajo que estan asociados a un usuario
    /// </summary>
    /// <param name="UsuarioId"></param>
    /// <returns></returns>
    Task<List<EspacioTrabajoUsuario>> EspacioTrabajoUsuario(string UsuarioId);

    /// <summary>
    /// Determina si un usuario tiene acceso a un espacio de trabajo
    /// </summary>
    /// <param name="UsuarioId"></param>
    /// <param name="EspacioTrabajoId"></param>
    /// <returns></returns>
    Task<bool> EspacioTrabajoValidoUsuario(string UsuarioId, Guid EspacioTrabajoId);
}
