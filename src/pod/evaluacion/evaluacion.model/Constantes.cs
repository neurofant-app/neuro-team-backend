namespace evaluacion.model;


/// <summary>
/// Estados de un proiceso de eveluación
/// </summary>
public enum EstadoEvaluacion
{
    /// <summary>
    /// La evaluación se ecuentra en proceso de diseño
    /// </summary>
    Diseno,
    /// <summary>
    /// Le evaluación ha sido publicada y se encuentra en espera de su aplicación
    /// </summary>
    Publicada,
    /// <summary>
    /// Le evaluación se encuentra en ejecución
    /// </summary>
    Ejecucion,
    /// <summary>
    /// Le evaluación ha sido finalizada
    /// </summary>
    Finalizada,
    /// <summary>
    /// Le evaluación se canceló
    /// </summary>
    Cancelada
}