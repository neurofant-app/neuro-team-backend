
using System.Diagnostics.CodeAnalysis;

namespace evaluacion.model.reactivos;

/// <summary>
/// DTO para la creación de reactivos múltiples
/// </summary>
[ExcludeFromCodeCoverage]
public class ReactivoMultipleEliminar
{
    public List<Guid> Ids { get; set; } = [];
}
