using System.Diagnostics.CodeAnalysis;

namespace aprendizaje.model.precios;

/// <summary>
/// Datos de venta de la neurona
/// </summary>
[ExcludeFromCodeCoverage]
public class Precio
{
    /// <summary>
    /// Identificador único del precio
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Nombre para identiicar el precio
    /// </summary>
    public required string Nombre { get; set; }

    /// <summary>
    /// Tipo de precio de venta
    /// </summary>
    public TipoPrecio TipoPrecio { get; set; }

    /// <summary>
    /// Periodo aplicable en renta
    /// </summary>
    public TipoPeriodoRenta TipoPeriodoRenta { get; set; }

    /// <summary>
    /// Precio aplicable por pasarela de pago
    /// </summary>
    public TipoPasarelaPago TipoPasarelaPago { get; set; }

    /// <summary>
    /// Lista de precios por moneda
    /// </summary>
    List<PrecioMoneda> Precios { get; set; } = [];


    /// <summary>
    /// Impuestos aplicables adicionados al precio 
    /// </summary>
    public List<Impuesto> Impuestos { get; set; } = [];
}