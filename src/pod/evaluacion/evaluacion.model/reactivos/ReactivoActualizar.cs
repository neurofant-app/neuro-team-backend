using System.Diagnostics.CodeAnalysis;

namespace evaluacion.model.reactivos;

/// <summary>
/// DTO para la actualización de reactivos en la evaluación
/// </summary>
[ExcludeFromCodeCoverage]
public class ReactivoActualizar
{
    /// <summary>
    /// Identificador único del temario asociado a la evaluación
    /// </summary>
    public required Guid TemarioId { get; set; }

    /// <summary>
    /// Identificador único del tema contenido del la evaluación
    /// </summary>
    public required Guid TemaId { get; set; }

    /// <summary>
    /// Identificador único del reactivo en el temario
    /// </summary>
    public required  string ReactivoId { get; set; }

    /// <summary>
    /// Determina si el reactivo el requerido en todas las veriantes de la evaluación
    /// </summary>
    public bool? Obligatorio { get; set; } = false;

    /// <summary>
    /// Puntos asociados al reactivo en la evaluación
    /// </summary>
    public int? Puntaje { get; set; } = 1;

    /// <summary>
    /// Dificultad del reactivo
    /// </summary>
    public DificultadReactivo? Dificultad { get; set; } = DificultadReactivo.Desconocida;

}
