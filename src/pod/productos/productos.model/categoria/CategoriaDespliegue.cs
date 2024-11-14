using productos.model.comunes;

namespace productos.model.categoria;

/// <summary>
/// DTO de despliegue de una categoría de productos
/// </summary>
public class CategoriaDespliegue
{
    /// <summary>
    /// Identificador único de la categoría
    /// </summary>
    public Guid Id { get; set; }


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
    public List<ValorI18N<string?>> URLImagem { get; set; } = [];
}
