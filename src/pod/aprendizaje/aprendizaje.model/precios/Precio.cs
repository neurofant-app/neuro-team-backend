using MongoDB.Bson.Serialization.Attributes;
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
    [BsonId]
    public Guid Id { get; set; }

    /// <summary>
    /// Nombre para identiicar el precio
    /// </summary>
    [BsonElement("n")]
    public required string Nombre { get; set; }

    /// <summary>
    /// Tipo de precio de venta
    /// </summary>
    [BsonElement("tp")]
    public TipoPrecio TipoPrecio { get; set; }

    /// <summary>
    /// Periodo aplicable en renta
    /// </summary>
    [BsonElement("tpr")]
    public TipoPeriodoRenta TipoPeriodoRenta { get; set; }

    /// <summary>
    /// Precio aplicable por pasarela de pago
    /// </summary>
    [BsonElement("tpp")]
    public TipoPasarelaPago TipoPasarelaPago { get; set; }

    /// <summary>
    /// Lista de precios por moneda
    /// </summary>
    [BsonElement("p")]
    List<PrecioMoneda> Precios { get; set; } = [];


    /// <summary>
    /// Impuestos aplicables adicionados al precio 
    /// </summary>
    [BsonElement("im")]
    public List<Impuesto> Impuestos { get; set; } = [];
}