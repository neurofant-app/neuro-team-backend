using System.Diagnostics.CodeAnalysis;

namespace evaluacion.model.participantes;

/// <summary>
/// Define un evento del proceso para conformar el historial de la evaluación del participante
/// </summary>
[ExcludeFromCodeCoverage]
public class EventoProcesoEvaluacion
{
    /// <summary>
    /// Fecha del elemento histórico
    /// </summary>
    public DateTime Fecha { get; set; }

    /// <summary>
    /// Estado asociado al evento 
    /// </summary>
    public EstadoProcesoEvaluacion Estado { get; set; }
}
