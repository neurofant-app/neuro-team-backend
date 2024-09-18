using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using comunes.primitivas.atributos;

namespace controlescolar.modelo.alumnos;

/// <summary>
/// Define un alumno perteneciente a un campus
/// </summary>
[EntidadDB]
public class EntidadAlumno: PersonaBase
{
    /// <summary>
    /// Identificador único del alumno en el repositorio, se genera al crear un registro
    /// </summary>
    [BsonId]
    public virtual Guid Id { get; set; }
    /// <summary>
    /// IDentificadpr único del alumno dentro del campus por ejemplo número de alumno 
    /// </summary>
    [BsonElement("idi")]
    public string? IdInterno { get; set; }

    [BsonElement("eid")]
    public Guid EscuelaId { get; set; }


}
