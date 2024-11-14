using System.Diagnostics.CodeAnalysis;

namespace organizacion.model.unidadorganizacional;

/// <summary>
/// Entidad de almacenamiento de la UO
/// </summary>
[ExcludeFromCodeCoverage]
public class UnidadOrganizacional
{  
    /// <summary>
   /// Identificador único de la UO
   /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Nombre de la unidad organizacional
    /// </summary>
    public string Nombre { get; set; }

    /// <summary>
    /// Indica si la UO está activa
    /// </summary>
    public bool Activa { get; set; } = true;
}
