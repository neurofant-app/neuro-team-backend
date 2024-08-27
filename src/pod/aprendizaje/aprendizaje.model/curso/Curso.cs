using aprendizaje.model.plan;
using aprendizaje.model.temario;
using comunes.primitivas.atributos;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Diagnostics.CodeAnalysis;

namespace aprendizaje.model;


/// <summary>
/// DEfine un curso de aprendizaje
/// </summary>
[ExcludeFromCodeCoverage]
[EntidadDB]
public class Curso
{
    /// <summary>
    /// Identificador único del curso
    /// </summary>
    [BsonId]
    [BsonGuidRepresentation(GuidRepresentation.Standard)]
    public Guid Id { get; set; }

    /// <summary>
    /// Didentificador único del espacio de trabajo al que pertenece el curso,
    /// Los espacios de trabajo son creados por usuarios suscritos a NeuroPad
    /// </summary>
    [BsonElement("eid")]
    public required string EspacioTrabajoId { get; set; }

    /// <summary>
    /// Idiomas en los que se ofrece el contenido del curso
    /// </summary>
    [BsonElement("i")]
    public List<string> Idiomas { get; set; } = [];


    /// <summary>
    /// Nombre del curso
    /// </summary>
    [BsonElement("n")]
    public List<ValorI18N<string>> Nombre { get; set; } = [];

    /// <summary>
    /// Descripción del curso, HTML, MarkDown o similar
    /// </summary>
    [BsonElement("d")]
    public List<ValorI18N<string>> Descripcion { get; set; } = [];

    /// <summary>
    /// Versión del curso
    /// </summary>
    [BsonElement("v")]
    public string Version { get; set; } = "";

    /// <summary>
    /// Lista de planes de estudiso para cumplir con el curso
    /// </summary>
    [BsonElement("pe")]
    public List<Plan> PlanesEstudio { get; set; } = [];

    /// <summary>
    /// Lista de temariso disponibles para la planeación curricular a partir de esta lista se vinculan los temarios de los planes
    /// </summary>
    [BsonElement("ts")]
    public List<Temario> Temarios { get; set; } = [];


    /// <summary>
    /// Especialidades del plan de estudios
    /// </summary>
    [BsonElement("es")]
    public List<Especialidad> Especialidades { get; set; } = [];


}
