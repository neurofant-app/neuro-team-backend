using evaluacion.model.evaluacion.temas;
using comunes.primitivas.atributos;
using MongoDB.Bson.Serialization.Attributes;
using System.Diagnostics.CodeAnalysis;
using evaluacion.model.evaluacion.variantes;

namespace evaluacion.model.evaluacion;

/// <summary>
/// Una evaluación es un conjunto de reactivos aplicados a un grupo de estudiantes
/// para obtener las calificaciones individuales y los estadísticos grupales asociados
/// </summary>
[ExcludeFromCodeCoverage]
[EntidadDB]
public class Evaluacion
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
    /// Identificador único del dominio en el que se genera la evaluación
    /// </summary>
    [BsonElement("did")]
    public Guid DominioId { get; set; }

    /// <summary>
    /// Identificador único de la unidad organizacional en el que se genera la evaluación
    /// </summary>
    [BsonElement("oid")]
    public Guid OUId { get; set; }

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
    /// Cuando los participantes se almacenan externamente mantiene el indice de último elemento de la lista
    /// </summary>
    [BsonElement("il")]
    public int IndiceListaParticipantes { get; set; } = 0;


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
    /// Vriantes de la evaluación
    /// </summary>
    [BsonElement("vs")]
    public List<VarianteEvaluacion> Variantes { get; set; } = [];

    /// <summary>
    /// Total de variantes en la evaluación
    /// </summary>
    [BsonElement("tvs")]
    public int TotalVariantes { get; set; } = 0;

    /// <summary>
    /// Estado de la evaluación
    /// </summary>
    [BsonElement("es")]
    public EstadoEvaluacion Estado { get; set; } = EstadoEvaluacion.Diseno;

}
