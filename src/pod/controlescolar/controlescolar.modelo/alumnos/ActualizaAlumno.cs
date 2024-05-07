using comunes.primitivas.atributos;

namespace controlescolar.modelo.alumnos;

/// <summary>
/// Define el TO de actualización un alumno perteneciente a un campus
/// </summary>
[CQRSActualizar]
public class ActualizaAlumno : PersonaBase
{
    /// <summary>
    /// Identificador único del usuario a actualizar
    /// </summary>
    public virtual required Guid Id { get; set; }

    /// <summary>
    /// IDentificadpr único del alumno dentro del campus por ejemplo número de alumno 
    /// </summary>
    public string? IdInterno { get; set; }
}
