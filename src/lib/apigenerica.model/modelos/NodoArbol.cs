using System.Diagnostics.CodeAnalysis;

namespace apigenerica.model.modelos;

/// <summary>
/// Define los nodos de un arbol en una estructura jerárquica
/// </summary>
/// <typeparam name="T">Tipo de datos de la carga util</typeparam>
[ExcludeFromCodeCoverage]
public class NodoArbol<T>
{
    /// <summary>
    /// Identificador único del nodo serializado como string
    /// </summary>
    public required string Id { get; set; }

    /// <summary>
    /// Texto a mostrar en el nodo
    /// </summary>
    public required string Texto { get; set; }

    /// <summary>
    /// Identificador único del nodo padre serializado como string
    /// </summary>
    public string? PadreId { get; set; }

    /// <summary>
    /// Carga util del nodo
    /// </summary>
    public T? Payload { get; set; }

}
