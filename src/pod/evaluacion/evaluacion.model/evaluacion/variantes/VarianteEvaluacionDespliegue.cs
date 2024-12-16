using System.Diagnostics.CodeAnalysis;

namespace evaluacion.model.evaluacion.variantes;

/// <summary>
/// DTO para el despliegue de la variante
/// </summary>
[ExcludeFromCodeCoverage]
public class VarianteEvaluacionDespliegue
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

    /// <summary>
    /// Identificador único del creador de la evaluación
    /// </summary>
    public Guid CreadorId { get; set; }

    /// <summary>
    /// Fecha de creación de la evaluación
    /// </summary>
    public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Total de reactivos en la variante
    /// </summary>
    public int TotalReactivos { get; set; } = 0;

    /// <summary>
    /// Total de puntos de reactivos en la variante
    /// </summary>
    public int TotalPuntos { get; set; } = 0;

    /// <summary>
    /// Total de ejecuciones de la variante
    /// </summary>
    public int TotalEjecuciones { get; set; } = 0;

}
