using System.Diagnostics.CodeAnalysis;

namespace organizacion.model.unidadorganizacional;

/// <summary>
/// DTO de creación para la UO
/// </summary>
[ExcludeFromCodeCoverage]
public class UnidadOrganizacionalInsertar
{  
    /// <summary>
    /// Nombre de la unidad organizacional
    /// </summary>
    public string Nombre { get; set; }
}
