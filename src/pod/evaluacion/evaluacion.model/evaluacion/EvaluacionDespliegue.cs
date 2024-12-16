using evaluacion.model.evaluacion.temas;
using MongoDB.Bson.Serialization.Attributes;
using System.Diagnostics.CodeAnalysis;

namespace evaluacion.model.evaluacion;

/// <summary>
/// DTO para el despliegue de las evaluaciones
/// </summary>
[ExcludeFromCodeCoverage]
public class EvaluacionDespliegue
{
    /// <summary>
    /// Identificador único de la evaluación
    /// </summary>
    [BsonId]
    public Guid Id { get; set; }

    /// <summary>
    /// Nombre de la evaluación
    /// </summary>
    [BsonElement("n")]
    public required string Nombre { get; set; }


    /// <summary>
    /// Identificador único del creador de la evaluación
    /// </summary>
    [BsonElement("cid")]
    public Guid CreadorId { get; set; }


    /// <summary>
    /// Fecha de creación de la evaluación
    /// </summary>
    [BsonElement("fc")]
    public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;


    /// <summary>
    /// Determina si los evaluados son una lista fija para aplicar la evaluación
    /// En caso FALSE significa que las variantes serán asignadas dinámicamente a los evaluados
    /// </summary>
    [BsonElement("pf")]
    public bool ParticipantesFijos { get; set; }

    /// <summary>
    /// Lista de temas incluidos en una evaluación
    /// </summary>
    [BsonElement("ti")]
    public List<TemaEvaluacion> Temas { get; set; } = [];

    /// <summary>
    /// reactivos totales en la evaluación
    /// </summary>
    [BsonElement("tr")]
    public int TotalReactivos { get; set; } = 0;

    /// <summary>
    /// Estado de la evaluación
    /// </summary>
    [BsonElement("es")]
    public EstadoEvaluacion Estado { get; set; }
}
