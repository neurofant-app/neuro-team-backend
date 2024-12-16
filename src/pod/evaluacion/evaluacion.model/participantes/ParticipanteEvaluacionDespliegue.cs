using System.Diagnostics.CodeAnalysis;

namespace evaluacion.model.participantes;

/// <summary>
/// DTO para el despliegue de participantes de la evaluacion
/// </summary>
[ExcludeFromCodeCoverage]
public class ParticipanteEvaluacionDespliegue
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

    /// <summary>
    /// Estado de evaluación del participante
    /// </summary>
    public EstadoProcesoEvaluacion Estado { get; set; } = EstadoProcesoEvaluacion.SinEvaluar;

    /// <summary>
    /// Procentaje de aciertos de la evaluacion
    /// </summary>
    public decimal Porcentaje { get; set; } = 0;

    /// <summary>
    /// Especifica se la evaluación de OMR ha sido realizada
    /// </summary>
    public bool EvaluadoOMR { get; set; } = false;

    /// <summary>
    /// Especifica si hay errores de OMR
    /// </summary>
    public bool ErroresOMR { get; set; } = false;

    /// <summary>
    /// Especifica si la evaluación tiene datos de OMR
    /// </summary>
    public bool DatosRecibidosOMR { get; set; } = false;

    /// <summary>
    /// Bitácora de procesamiento
    /// </summary>
    public List<EventoProcesoEvaluacion> BitacoraProceso { get; set; } = [new EventoProcesoEvaluacion() { Estado = EstadoProcesoEvaluacion.SinEvaluar, Fecha = DateTime.UtcNow }];

}
