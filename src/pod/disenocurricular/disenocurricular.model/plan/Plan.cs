using comunes.primitivas.atributos;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.Diagnostics.CodeAnalysis;

namespace disenocurricular.model;

/// <summary>
/// Define un plan de estudios
/// </summary>
[ExcludeFromCodeCoverage]
[EntidadDB]
public class Plan
{
    /// <summary>
    /// Identificador único del plan de estudios
    /// </summary>
    [BsonId]
    [BsonGuidRepresentation(GuidRepresentation.Standard)]
    public Guid Id { get; set; }
    
    /// <summary>
    /// Identificador del curso al que corresponde el temario
    /// </summary>
    [BsonElement("cid")]
    [BsonGuidRepresentation(GuidRepresentation.Standard)]
    public Guid CursoId { get; set; }

    /// <summary>
    /// Didentificador único del espacio de trabajo al que pertenece el plan de estudios
    /// </summary>
    [BsonElement("eid")]
    public required string EspacioTrabajoId { get; set; }

    /// <summary>
    /// Nombre del curso
    /// </summary>
    [BsonElement("n")]
    public List<ValorI18N<string>> Nombre { get; set; } = [];


    /// <summary>
    /// Descripción del plan de estudios, HTML, MarkDown o similar
    /// </summary>
    [BsonElement("d")]
    public List<ValorI18N<string>> Descripcion { get; set; } = [];


    /// <summary>
    /// Versión del plan de estudios
    /// </summary>
    [BsonElement("v")]
    public string Version { get; set; } = "";

    /// <summary>
    /// DEtermina si el plan de estudios se basa en periodos
    /// </summary>
    [BsonElement("pe")]
    public TipoPeriodoPlan Periodicidad { get; set; }

    /// <summary>
    /// Periodos de los que consta un plan
    /// </summary>
    [BsonElement("ps")]
    public List<Periodo> Periodos { get; set; } = [];

}

