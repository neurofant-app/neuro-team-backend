using System.Diagnostics.CodeAnalysis;

namespace organizacion.model.unidadorganizacional;

/// <summary>
/// DTO de actualización para la UO
/// </summary>
[ExcludeFromCodeCoverage]
public class UnidadOrganizacionalActualizar
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
