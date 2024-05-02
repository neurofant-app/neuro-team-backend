namespace apigenerica.model.modelos;

/// <summary>
/// Constla básica para los endpoints
/// </summary>
public class Consulta
{
    /// <summary>
    /// Identificador único de la consulta 
    /// </summary>
    public string? Id { get; set; }

    /// <summary>
    /// Determina si el cache de datos debe ser anulado para la consulta
    /// </summary>
    public bool AnularCache { get; set; } = false;

    /// <summary>
    /// Especifica si la consulta debe reevaluar el conteo, para la página0 este valor siempre es true
    /// </summary>
    public bool Contar { get; set; } = false;


    /// <summary>
    /// Datos del paginado para la consulta
    /// </summary>
    public Paginado Paginado { get; set; } = new Paginado() { Indice = 0, Tamano = 25 };


    /// <summary>
    /// Filtros utilizados para los datos
    /// </summary>
    public List<Filtro>? Filtros { get; set; } = new List<Filtro>();

}
