using productos.model.comunes;

namespace productos.model.categoria;

/// <summary>
/// Define una categoría de productos
/// </summary>
public class Categoria
{
    /// <summary>
    /// Identificador único de la categoría
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Identificador único del mercado de la categoría
    /// </summary>
    public Guid MercadoId { get; set; }

    /// <summary>
    /// Idioma de la categoría si no se encuentra una coincidencia I18N, 
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
    /// En el caso de las subcategorías coniene el Id de la categoría Padre
    /// </summary>
    public Guid? CategoríaPadreId { get; set; }

    /// <summary>
    /// URL de la imagen o base64 de la misma
    /// </summary>
    public List<ValorI18N<string?>> URLImagen { get; set; } = [];


    /// <summary>
    /// URL base del contenido permite mostrar páginas web de un CMS en el estilo {URL}/{IdProducto}
    /// </summary>
    public List<ValorI18N<string?>> URLBaseContenido { get; set; } = [];

    /// <summary>
    /// Tipo de entidad o servicio al cual ser[an mapeados los productos de la categoría
    /// por ejemplo para Neurofant Neurona
    /// </summary>
    public string? ClaseInterna { get; set; }

    /// <summary>
    /// Propiedad de la entidad mapeada que se vincula a los productos de la categoría 
    /// por ejemplo el Id de una neurona de Neurofant
    /// </summary>
    public string? ClaseInternaId { get; set; }

    /// <summary>
    /// Especifica si la categoría es visible en el mercado
    /// </summary>
    public bool Visible { get; set; }
}
