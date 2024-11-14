using System.Diagnostics.CodeAnalysis;

namespace organizacion.model.pim;

/// <summary>
/// Licenciamiento del producto en la organización
/// </summary>
[ExcludeFromCodeCoverage]
public class LicenciaProducto
{
    /// <summary>
    /// Versión del produto licenciado
    /// </summary>
    public string Version { get; set; }

    /// <summary>
    /// Elementos asociados a la licencia
    /// </summary>
    public List<ElementoLicencia> Elementos { get; set; } = [];
}
