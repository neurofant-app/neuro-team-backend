using System.Diagnostics.CodeAnalysis;

namespace evaluacion.model.evaluacion.variantes;

/// <summary>
/// DTO para la actuallización de la variante de evaluación
/// </summary>
[ExcludeFromCodeCoverage]
public class VarianteEvaluacionActualizar
{

    /// <summary>
    /// Identificador único de la variante de evaluación
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Nombre de la variante, si no se proporicione se asigna automáticamente
    /// con el nombre la evaluación y el Id
    /// </summary>
    public string? Nombre { get; set; } 

}
