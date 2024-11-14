using productos.model.comunes;
using System.Diagnostics.CodeAnalysis;

namespace productos.model.mercado;

/// <summary>
/// Un mercado ofrece productos y servicos de venta y renta para los usuarios
/// </summary>
[ExcludeFromCodeCoverage]
public class Mercado
{
    /// <summary>
    /// Identificador único del mercado
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Idioma del mercado si no se encuentra una coincidencia I18N, 
    /// si no se encuentra definido se toma el primer elemento de la lista
    /// </summary>
    public string? IdiomaDefault { get; set; }

    /// <summary>
    /// Nombre I18N del producto 
    /// </summary>
    public List<ValorI18N<string>> Nombre { get; set; } = [];

    /// <summary>
    /// Descripción I18N del producto 
    /// </summary>
    public List<ValorI18N<string>> Descripcion { get; set; } = [];

    
    /// <summary>
    /// URL de la imagen o base64 de la misma
    /// </summary>
    public List<ValorI18N<string?>> URLImagen { get; set; } = [];
}
