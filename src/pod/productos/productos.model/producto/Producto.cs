using productos.model.comunes;
using System.ComponentModel.DataAnnotations;

namespace productos.model.producto;

/// <summary>
/// Define un producto o servicio que es ofrecido a los clientes de las aplicaciones
/// </summary>
public class Producto
{
    /// <summary>
    /// Identificador único del producto
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Nombre I18N del producto 
    /// </summary>
    [MaxLength(250)]
    public List<ValorI18N<string>> Nombre { get; set; } = [];

    /// <summary>
    /// Descripción I18N del producto 
    /// </summary>
    public List<ValorI18N<string>> Descripcion { get; set; } = [];

    /// <summary>
    /// Lista de las categorías a las que pertenece el producto
    /// </summary>
    public List<Guid> CetegoriasId { get; set; } = [];
    // Indexar

    /// <summary>
    /// SKU del producto
    /// </summary>
    public string? Sku { get; set; }
    // Indexar


    /// <summary>
    /// Identifica si el producto es digital
    /// </summary>
    public bool Digital { get; set; } = true;


    /// <summary>
    /// Ambitos aplicables para la adquisisón del producto
    /// </summary>
    public List<AmbitoProducto> Ambitos { get; set; } = [];
}
