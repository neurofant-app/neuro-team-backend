namespace servicio.almacenamiento;

/// <summary>
/// Interfaz genérica para la lextura y escritora de almacenamiento
/// </summary>
public interface IFabricaProveedorAlmacenamiento
{
    /// <summary>
    /// Obtiene un servicio de almacenamiento especializado
    /// </summary>
    /// <param name="servicio">tipo de servicio</param>
    /// <param name="servicioId">id del servicio</param>
    /// <returns>Un proveedor de servicios de almacenamiento</returns>
    Task<IProveedorAlmacenamiento?> ObtieneProveedor(string servicio, string? servicioId);
}
