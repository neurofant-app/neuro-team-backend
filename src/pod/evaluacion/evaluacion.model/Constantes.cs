namespace evaluacion.model;

/// <summary>
/// Estado general en el proceso de evaluacion
/// </summary>
public enum EstadoProcesoEvaluacion { 
    /// <summary>
    /// La evaluación no ha sido realizada
    /// </summary>
    SinEvaluar,
    /// <summary>
    /// Los datos para la evaluación han sido recibidos
    /// </summary>
    DatosRecibidos,
    /// <summary>
    /// La evaluación se encuentra en proceso
    /// </summary>
    EnProceso,
    /// <summary>
    /// El proceso de evaluación ha concluido
    /// </summary>
    Procesado,
    /// <summary>
    /// El proceso de evaluación concluyó con error
    /// </summary>
    ErrorProceso
}

/// <summary>
/// Resultados tipificados del proceso de evaluación OMR
/// </summary>
public enum ResultadoEvaluacionOMR { 
    /// <summary>
    /// El elemento no ha sido procesado
    /// </summary>
    SinProcesar,
    /// <summary>
    /// Le evaluación del OMR resulto correcta
    /// </summary>
    Correcto,
    /// <summary>
    /// El participante no marcó ningún alveolo en las respuestas
    /// </summary>
    Vacio,
    /// <summary>
    /// El participante marcó más de un alveolo en las respuestas
    /// </summary>
    Multiple
}


/// <summary>
/// Grado de dificultad de l reactivo
/// </summary>
public enum DificultadReactivo { 
    /// <summary>
    /// Dificultad desconocida
    /// </summary>
    Desconocida,
    /// <summary>
    /// Baja dificultad
    /// </summary>
    Baja,
    /// <summary>
    /// Dificultad media
    /// </summary>
    Media,
    /// <summary>
    /// Dificultad alta
    /// </summary>
    Alta
}


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
    Publicada
}


/// <summary>
/// Estados de un proiceso de eveluación
/// </summary>
public enum EstadoEjecucionEvaluacion
{
    /// <summary>
    /// La ejecución de la evaluación se ecuentra en proceso de diseño
    /// </summary>
    Diseno,
    /// <summary>
    /// Le ejecución de la  evaluación ha sido publicada y se encuentra en espera de su aplicación
    /// </summary>
    Publicada,
    /// <summary>
    /// La ejecución de la evaluación se encuentra en ejecución
    /// </summary>
    Ejecucion,
    /// <summary>
    /// La ejecución de la evaluación ha sido finalizada
    /// </summary>
    Finalizada,
    /// <summary>
    /// Le ejecución de la evaluación se canceló
    /// </summary>
    Cancelada
}


/// <summary>
/// Define como se immprimirán los alveolos para la evaluación
/// </summary>
public enum TipoNumeracionAlveolos
{
    /// <summary>
    /// Imprime números por ejemplo 1,2,3,5
    /// </summary>
    Decimal,
    /// <summary>
    /// Imprime letras por ejemplo a,b,c,d
    /// </summary>
    Alfabetica
}

/// <summary>
/// Medio de ejecución de la evaluacion
/// </summary>
public enum MedioEjecucionEvaluacion { 
    /// <summary>
    /// La evaluación debe imprimirse
    /// </summary>
    Impresa,
    /// <summary>
    /// La evaluación puede presentarse de manera electrónica
    /// </summary>
    Electronica
}
