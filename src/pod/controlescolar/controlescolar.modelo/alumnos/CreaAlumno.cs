using comunes.primitivas.atributos;

namespace controlescolar.modelo.alumnos;

/// <summary>
/// DTO de creación para un alumno perteneciente a un campus
/// </summary>
[CQRSCrear]
public class CreaAlumno: PersonaBase
{

    /// <summary>
    /// Identificador único del campus al que pertenece el alumno
    /// </summary>
    public virtual required Guid CampusId { get; set; }

    /// <summary>
    /// IDentificadpr único del alumno dentro del campus por ejemplo número de alumno 
    /// </summary>
    public string? IdInterno { get; set; }
}
