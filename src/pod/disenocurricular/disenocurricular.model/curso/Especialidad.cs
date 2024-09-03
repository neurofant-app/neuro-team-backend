using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using comunes.primitivas.atributos;
using System.Diagnostics.CodeAnalysis;

namespace disenocurricular.model;

/// <summary>
/// Define una especialziación para un plan de estudios
/// </summary>
[ExcludeFromCodeCoverage]
[EntidadDB]
public class Especialidad
{
    /// <summary>
    /// Identificador único del plan de estudios
    /// </summary>
    [BsonId]
    public Guid Id { get; set; }

    /// <summary>
    /// Identificador del curso al que corresponde el temario
    /// </summary>
    [BsonElement("cid")]
    public Guid CursoId { get; set; }


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
    /// Lista de temarios obligatiors que deben cubrirse en el period
    /// </summary>
    [BsonElement("tr")]
    public List<Guid> TemariosObligatorios { get; set; } = [];

    /// <summary>
    /// Lista de temarios opcionales que deben cubrirse en el period
    /// </summary>
    [BsonElement("to")]
    public List<Guid> TemariosOpcionales { get; set; } = [];


    [BsonElement("cmin")]
    /// <summary>
    /// Número mínimo de creditos para cubrir la especialidad
    /// </summary>
    public int MinimoCreditos { get; set; } = 0;


    [BsonElement("cmax")]
    /// <summary>
    /// Número mínimo de creditos para cubrir la especialidad, 0 sin límite
    /// </summary>
    public int MaximoCreditos { get; set; } = 0;
}

