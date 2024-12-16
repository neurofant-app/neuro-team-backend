using System.Diagnostics.CodeAnalysis;

namespace evaluacion.model.participantes;

/// <summary>
/// DTO para la ádición de un participante a la evaluación
/// </summary>
[ExcludeFromCodeCoverage]
public class ParticipanteEvaluacionCrear
{

    /// <summary>
    /// Identificador único del participante
    /// </summary>
    public required string ParticipanteId { get; set; }

    /// <summary>
    /// IDentificador de la variante de evaluación aplicada
    /// </summary>
    public Guid? EvaluacionId { get; set; }

    /// <summary>
    /// IDentificador de la variante de evaluación aplicada
    /// </summary>
    public Guid? VarianteId { get; set; }

}
