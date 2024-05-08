using comunes.primitivas.atributos;

namespace controlescolar.modelo.alumnos;

/// <summary>
/// Define el DTO para el despliegue de alumnos perteneciente a un campus
/// </summary>
[CQRSConsulta]
public class ConsultaAlumno : PersonaBase
{

    /// <summary>
    /// Identificador único del alumno en el repositorio, se genera al crear un registro
    /// </summary>
    public virtual Guid Id { get; set; }

    /// <summary>
    /// IDentificadpr único del alumno dentro del campus por ejemplo número de alumno 
    /// </summary>
    public string? IdInterno { get; set; }
}
