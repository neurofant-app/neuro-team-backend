namespace sat.bot.modelo;

/// <summary>
/// Resultado de la tarea de conciliacion
/// </summary>
public class ResultadoTareaConciliacion
{

    public ResultadoTareaConciliacion(TareaConciliacion tarea)
    {
        Tarea = tarea;
    }

    /// <summary>
    /// Identificacdo de la tareas
    /// </summary>
    public Guid Id { get { return Tarea.Id; } }

    /// <summary>
    /// Datos de la tarea  de conciliacion
    /// </summary>
    public TareaConciliacion Tarea { get; set; }

    /// <summary>
    /// FEcha de inicio de procesamiento de la tarea
    /// </summary>
    public DateTime FechaInicioTarea { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// FEcha de conclusion de procesamiento de la tarea
    /// </summary>
    public DateTime FechaConclusionTarea { get; set; }

    /// <summary>
    /// DEtermina si la tarea finalizó sin errores
    /// </summary>
    public bool OK { get; set; }

    /// <summary>
    /// Lista de errores ocurridos durante el proceso
    /// </summary>
    public List<string> Errores { get; set; } = new List<string>();

}

