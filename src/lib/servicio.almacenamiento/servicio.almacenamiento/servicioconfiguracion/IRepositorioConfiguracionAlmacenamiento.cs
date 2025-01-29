using servicio.almacenamiento.configuraciones;

namespace servicio.almacenamiento.servicioconfiguracion;

/// <summary>
/// Proporciona los métodos para acceder a la configuración de un proveedor de almacenamiento
/// </summary>
public interface IRepositorioConfiguracionAlmacenamiento
{
    /// <summary>
    /// Obtiene la configuración de un proveedor de almacenamiento
    /// </summary>
    /// <param name="servicio">Nombre del servicio</param>
    /// <param name="servicioId">Identificador del servicio cuando puedan existr varias configuraciones identificadas</param>
    /// <returns></returns>
    Task<ConfiguracionProveedor?> ObtieneConfiguracion(string servicio, string? servicioId);
}
