using MongoDB.Bson.Serialization.Attributes;
using System.Diagnostics.CodeAnalysis;

namespace evaluacion.model.ejecucionevaluacion;

/// <summary>
/// Almacena los datos de la ejecución de una variante de evaluación
/// </summary>
[ExcludeFromCodeCoverage]
public class EjecucionVarianteEvaluacion
{
    /// <summary>
    /// Identificador único de le ejecución de la variante de evaluación
    /// </summary>
    [BsonElement("id")]
    public Guid Id { get; set; }

    /// <summary>
    /// Identificador de la evaluación 
    /// </summary>
    [BsonElement("eid")]
    public Guid EvaluacionId { get; set; }

    /// <summary>
    /// Identificador de la variante de evaluación 
    /// </summary>
    [BsonElement("vid")]
    public Guid VarianteId { get; set; }

    /// <summary>
    /// Nombre de la ejecución, si no se proporicione se asigna automáticamente
    /// </summary>
    [BsonElement("n")]
    public string? Nombre { get; set; }

    /// <summary>
    /// Total de participantes en la evaluación, este vaor se incrementa con el CRUD de participantes 
    /// en la ejecución
    /// </summary>
    [BsonElement("tp")]
    public int TotalParticipantes { get; set; } = 0;

    /// <summary>
    /// Mapa de evaluación del OMR, almacena las respuestas correctas en una cadena de texto 
    /// en base al indice del reactivo, esta cadena se almacena cifrada y se genera cuando
    /// la evaluación adquiere el estado Publicada, por ejemplo si en el 1er reactivo 
    /// la respuesta correcta es la segunda opcion y para el segundo reactivo la respuesta 
    /// correcta es la 4a entonces la cadena comenzará con "24..."
    /// </summary>
    [BsonElement("om")]
    public string MapaEvaluacionOMR { get; set; } = "";



}
