using comunes.primitivas.atributos;

namespace controlescolar.modelo.instructores;

/// <summary>
/// Define el TO de actualización un alumno perteneciente a un campus
/// </summary>
[CQRSActualizar]
public class ActualizaInstructor : PersonaBase
{
    /// <summary>
    /// IDentificadpr único del alumno dentro del campus por ejemplo número de alumno 
    /// </summary>
    public string? IdInterno { get; set; }
}
