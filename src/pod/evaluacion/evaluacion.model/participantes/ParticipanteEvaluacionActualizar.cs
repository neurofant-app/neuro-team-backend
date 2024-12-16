using System.Diagnostics.CodeAnalysis;

namespace evaluacion.model.participantes;

/// <summary>
/// DTO para la actualización del participante en el  proceso de evaluacion
/// </summary>
[ExcludeFromCodeCoverage]
public class ParticipanteEvaluacionActualizar
{
    /// <summary>
    /// Identificador único del registro
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Identificador único del participante
    /// </summary>
    public required string ParticipanteId { get; set; }

    /// <summary>
    /// Identificador de la variante de evaluación aplicada
    /// </summary>
    public Guid EvaluacionId { get; set; }

    /// <summary>
    /// IDentificador de la variante de evaluación aplicada
    /// </summary>
    public Guid? VarianteId { get; set; }

}
