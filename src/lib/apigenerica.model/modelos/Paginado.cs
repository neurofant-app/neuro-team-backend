namespace apigenerica.model.modelos;

/// <summary>
/// Define los parámetros para realizar el paginado de datos
/// </summary>
public class Paginado
{
    /// <summary>
    /// Númro de página a solicitar, comienza en cero
    /// </summary>
    public int Indice { get; set; }

    /// <summary>
    /// Máximo de elementos a  devolver
    /// </summary>
    public int Tamano { get; set; }

    /// <summary>
    /// Método de ordenamiento de los datos
    /// </summary>
    public Ordenamiento? Ordenamiento { get; set; }

    /// <summary>
    /// Nombre o Id de la columnga utilizada para el ordenamiento
    /// </summary>
    public string? ColumnaOrdenamiento { get; set; }

}
