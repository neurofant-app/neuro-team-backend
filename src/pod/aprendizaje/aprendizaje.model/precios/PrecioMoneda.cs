using System.Diagnostics.CodeAnalysis;

namespace aprendizaje.model.precios;

/// <summary>
/// DEfine un precio utilziando una moneda base
/// </summary>
[ExcludeFromCodeCoverage]
public class PrecioMoneda
{
    /// <summary>
    /// Código de la moneda por ejemplo USD o MXP
    /// </summary>
    public required string CodigoMoneda { get; set; }

    /// <summary>
    /// Precio de vanta
    /// </summary>
    public decimal Precio { get; set; }
}
