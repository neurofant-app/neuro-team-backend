using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace modelo.repositorio.cfdi.busqueda;

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
    /// Operador aplicable al filtro
    /// </summary>
    public OperadorFiltro Operador { get; set; }
    /// <summary>
    /// Valor que describe si el filtro sera una negado
    /// </summary>
    public bool Negado { get; set; } = false;

    /// <summary>
    /// Lista de valores utilizados para el filtro, en el caso de operadores unarios será siempre el primer elemento
    /// para operadores binarios se utilizarán el primero y el segundo valor de la lista
    /// </summary>
    public List<string> Valores { get; set; }

    /// <summary>
    /// Este valor se utiliza solo para los filtros de texto completo e indica el grado de similaridad
    /// al realizar búsquedas de texto 
    /// </summary>
    public int? NivelFuzzy { get; set; }

}
