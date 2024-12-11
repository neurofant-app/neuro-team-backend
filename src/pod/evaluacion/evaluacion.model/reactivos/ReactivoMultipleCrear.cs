
using System.Diagnostics.CodeAnalysis;

namespace evaluacion.model.reactivos;

/// <summary>
/// DTO para la creación de reactivos múltiples
/// </summary>
[ExcludeFromCodeCoverage]
public class ReactivoMultipleCrear
{
    public List<ReactivoCrear> Reactivos { get; set; } = [];
}
