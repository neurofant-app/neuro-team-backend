using MongoDB.Bson.Serialization.Attributes;
using System.Diagnostics.CodeAnalysis;

namespace controlescolar.modelo.persona;

/// <summary>
/// Define la entidad a almacenar en el repositorio de datos para la Persona
/// Una persona es la representación de un ser humano que tiene un vínculo con la escuela
/// </summary>
[ExcludeFromCodeCoverage]
public class EntidadPersona
{
    // <summary>
    /// Identificador único de la persona
    /// </summary>
    [BsonId]
    public Guid Id { get; set; }

    /// <summary>
    /// Identificador único la escuela a la que pertenece el resistro de la persona
    /// </summary>
    [BsonElement("t")]
    public Guid EscuelaId { get; set; }
    /// Debe crearse un índice en la base de datos para facilitar las búsquedas

    /// <summary>
    /// Nombre de la persona
    /// </summary>
    [BsonElement("n")]
    public required string Nombre { get; set; }

    /// <summary>
    /// Primer apellido de la persona
    /// </summary>
    [BsonElement("a1")]
    public required string Apellido1 { get; set; }

    /// <summary>
    /// Segundo apellido de la persona
    /// </summary>
    [BsonElement("a2")]
    public string? Apellido2 { get; set; }

    /// <summary>
    /// Fecha de nacimiento de la persona, la parte de hora es ignorada y debe llenarse con 00:00:00
    /// </summary>
    [BsonElement("fn")]
    public DateTime? Nacimiento { get; set; }

    /// <summary>
    /// Genero asociado a la persona
    /// </summary>
    [BsonElement("g")]
    public GeneroPersona? Genero { get; set; }

    /// <summary>
    /// Clave de única de identidad de la persona a nivel nacional, pro ejemplo la CURP en México
    /// </summary>
    [BsonElement("in")]
    public string? IdNacional { get; set; }
    // Indexar este campo en el repositorio


    /// <summary>
    /// Define como una persona se vincula con la escuela, los vínculos tienen una temporalidad
    /// y una persona puede tener múlples vínculos con una escuela y sus recursos por ejemplo en un plantel
    /// puede ser alumno de dos cursos y laborar como docente en un turno y administrativo en otro por ejemplo
    /// </summary>
    [BsonElement("vi")]
    public List<VinculoPersonaEscuela> Vinculos { get; set; } = [];

    /// <summary>
    /// Instancias de los expedientes asocidos a la persona
    /// </summary>
    [BsonElement("xp")]
    public List<InstanciaExpediente> Expedientes { get; set; } = [];

}
