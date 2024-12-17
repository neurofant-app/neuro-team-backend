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
    public Guid Id { get; set; }
    public required string Nombre { get; set; }
    public Guid CreadorId { get; set; }
    public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;
    public bool ParticipantesFijos { get; set; }
    public List<TemaEvaluacion> Temas { get; set; } = [];
    public int TotalReactivos { get; set; } = 0;

    /// <summary>
    /// Estado de la evaluación
    /// </summary>
    [BsonElement("es")]
    public EstadoEvaluacion Estado { get; set; }
}
