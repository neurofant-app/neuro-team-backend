using MongoDB.Bson.Serialization.Attributes;
using System.Diagnostics.CodeAnalysis;

namespace evaluacion.model.evaluacion;

/// <summary>
/// Almacena la lista de participantes de una evaluación 
/// Esta entidad se utiliza para separar los paricipantes de la entidad de evaluacion
/// por ejemplo cuando hay un gran número de ellos
/// </summary>
[ExcludeFromCodeCoverage]
public class ListaParticipantes
{
    /// <summary>
    /// Identificado rúnico del registro
    /// </summary>
    [BsonId]
    public Guid Id { get; set; }

    /// <summary>
    /// IDentificador único de la evaluación
    /// </summary>
    [BsonElement("eid")]
    public Guid EvaluacionId { get; set; }

    /// <summary>
    /// Identificadores de los evaluados a las cuales les será aplicada la evaluación si EvaluadosFijos = TRUE
    /// </summary>
    [BsonElement("ps")]
    public List<Participante> Participantes { get; set; } = [];

    /// <summary>
    /// Indice de la lista de partiicipantes cuando se dividen en lotes
    /// </summary>
    [BsonElement("i")]
    public int Indice { get; set; }

}
