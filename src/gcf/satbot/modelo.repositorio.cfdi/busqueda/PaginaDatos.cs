namespace modelo.repositorio.cfdi.busqqueda;

/// <summary>
/// Representa una página de datos devuelta como resultado de una consulta
/// </summary>
/// <typeparam name="T"></typeparam>
public class PaginaDatos<T>
{

    /// <summary>
    /// Número de la página que se esta devolviendo
    /// </summary>
    public int Indice { get; set; } = 0;

    /// <summary>
    /// Lista de elementos de la página actual 
    /// </summary>
    public List<T> Elementos { get; set; }

    /// <summary>
    /// Tamano de pagina solicitado
    /// </summary>
    public int Tamano { get; set; } = 25;

    /// <summary>
    /// Total de elementos encontrados durante la búsqueda
    /// </summary>
    public long Total { get; set; }

}
