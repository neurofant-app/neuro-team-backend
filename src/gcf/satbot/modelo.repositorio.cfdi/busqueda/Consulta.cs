namespace modelo.repositorio.cfdi.busqueda;

/// <summary>
/// 
/// </summary>
public class Consulta
{

    /// <summary>
    /// Identificador único de la búsqueda, el cliente debe almacenar este valro 
    /// y enviarlo en cada request para arpovechar los beneficio de caché
    /// </summary>
    public string? Id { get; set; } = Guid.NewGuid().ToString();

    /// <summary>
    /// Numero de página a obtener
    /// </summary>
    public int Indice { get; set; } = 0;

    /// <summary>
    /// Número máximo de elementos para ser devueltos por la página
    /// </summary>
    public int? Tamano { get; set; } = 25;

    /// <summary>
    /// Método de ordenamiento para la la página de datos en base a la columna seleccionada
    /// </summary>
    public Ordenamiento? Ordenamiento { get; set; } = busqueda.Ordenamiento.DESC;

    /// <summary>
    /// Nombre o Id de la columna para realizar el ordenamiento
    /// </summary>
    public string? ColumnaOrdenamiento { get; set; } = "FechaCFDI";

    /// <summary>
    /// FIltros a aplicar para la búsqueda
    /// </summary>
    public List<Filtro>? Filtros { get; set; }

    /// <summary>
    /// Determina si es necesario realizar el cálculo de totales 
    /// </summary>
    public bool RecalcularTotales { get; set; }
}
