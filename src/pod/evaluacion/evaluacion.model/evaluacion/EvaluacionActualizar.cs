using MongoDB.Bson.Serialization.Attributes;
using System.Diagnostics.CodeAnalysis;

namespace evaluacion.model.evaluacion;

/// <summary>
/// Una evaluación es un conjunto de reactivos aplicados a un grupo de estudiantes
/// para obtener las calificaciones individuales y los estadísticos grupales asociados
/// </summary>
[ExcludeFromCodeCoverage]
public class EvaluacionActualizar
{
    /// <summary>
    /// Nombre de la evaluación
    /// </summary>
    [BsonElement("n")]
    public required string Nombre { get; set; }

    /// <summary>
    /// Determina si los evaluados son una lista fija para aplicar la evaluación
    /// En caso FALSE significa que las variantes serán asignadas dinámicamente a los evaluados
    /// </summary>
    [BsonElement("f")]
    public bool ParticipantesFijos { get; set; }


}
