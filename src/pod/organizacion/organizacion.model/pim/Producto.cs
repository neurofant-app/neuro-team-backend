using System.Diagnostics.CodeAnalysis;

namespace organizacion.model.pim;

/// <summary>
/// Datos del producto asociados al dominio
/// </summary>
[ExcludeFromCodeCoverage]
public class Producto
{
    /// <summary>
    /// Identificador únido del producto
    /// </summary>
    public Guid ProductoId { get; set; }

    /// <summary>
    /// Estatus del producto en el dominio
    /// </summary>
    public bool Activo { get; set; }

    /// <summary>
    /// Licencias del producto
    /// </summary>
    public List<LicenciaProducto> Licencias { get; set; } = [];

}
