using MongoDB.Bson.Serialization.Attributes;
using System.Diagnostics.CodeAnalysis;

namespace evaluacion.model.restricciones;

/// <summary>
/// Define una restricción en fechas para la ejecución de la evaluación
/// </summary>
[ExcludeFromCodeCoverage]
public class RestriccionEjecucionFecha
{
    /// <summary>
    /// Fecha mínima para generar la ejecución de la evaluación
    /// </summary>
    [BsonElement("fi")]
    public DateTime? Inicio { get; set; }

    /// <summary>
    /// Fecha máxima para generar la ejecución de la evaluación
    /// </summary>
    [BsonElement("ff")]
    public DateTime? Fin { get; set; }
}
