namespace apigenerica.model.modelos;

/// <summary>
/// Filtro de datos para aplicar en las consultas
/// </summary>
public class Filtro
{
    /// <summary>
    /// Nombre o Id del campo para filtrar
    /// </summary>
    public string Campo { get; set; }

    /// <summary>
    /// Determina si el filtro debe ser negado por ejemplo no igual
    /// </summary>
    public bool Negar { get; set; }

    /// <summary>
    /// Operador aplicable al filtro
    /// </summary>
    public OperadorFiltro Operador { get; set; }

    /// <summary>
    /// Lista de valores utiliados para el filtro, en el caso de operadores unarios será siempre el primer elemento
    /// para operadores binarios se utilizarán el primero y el segundo valor de la lista
    /// </summary>
    public List<string> Valores { get; set; }

    /// <summary>
    /// Este valor se utiliza solo para los filtros de texto completo e indica el grado de similaridad
    /// al realizar búsquedas de texto 
    /// </summary>
    public int? NivelFuzzy { get; set; }
}
