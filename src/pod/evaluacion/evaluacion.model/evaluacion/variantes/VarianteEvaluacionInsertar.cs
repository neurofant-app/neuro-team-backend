using System.Diagnostics.CodeAnalysis;

namespace evaluacion.model.evaluacion.variantes;

/// <summary>
/// DTO para la creación de la variante de evaluación
/// </summary>
[ExcludeFromCodeCoverage]
public class VarianteEvaluacionInsertar
{

    /// <summary>
    /// Nombre de la variante, si no se proporicione se asigna automáticamente
    /// con el nombre la evaluación y el Id
    /// </summary>
    public string? Nombre { get; set; } 

}
