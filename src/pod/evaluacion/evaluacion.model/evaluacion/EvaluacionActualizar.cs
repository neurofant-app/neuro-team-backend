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
    public required string Nombre { get; set; }
}
