using MongoDB.Bson.Serialization.Attributes;
using System.Diagnostics.CodeAnalysis;

namespace evaluacion.model.participantes;

/// <summary>
/// Define un participante del proceso de evaluacion
/// </summary>
[ExcludeFromCodeCoverage]
public class ParticipanteEvaluacion
{
    /// <summary>
    /// Identificador único del registro
    /// </summary>
    [BsonId]
    public Guid Id { get; set; }

    /// <summary>
    /// Identificador único del participante
    /// </summary>
    [BsonElement("i")]
    public required string ParticipanteId { get; set; }

    /// <summary>
    /// Identificador de la variante de evaluación aplicada
    /// </summary>
    [BsonElement("e")]
    public Guid EvaluacionId { get; set; }

    /// <summary>
    /// IDentificador de la variante de evaluación aplicada
    /// </summary>
    [BsonElement("v")]
    public Guid? VarianteId { get; set; }

    /// <summary>
    /// Estado de evaluación del participante
    /// </summary>
    [BsonElement("s")]
    public EstadoProcesoEvaluacion Estado { get; set; } = EstadoProcesoEvaluacion.SinEvaluar;

    /// <summary>
    /// Procentaje de aciertos de la evaluacion
    /// </summary>
    [BsonElement("p")]
    public decimal Porcentaje { get; set; } = 0;

    /// <summary>
    /// Especifica se la evaluación de OMR ha sido realizada
    /// </summary>
    [BsonElement("eo")]
    public bool EvaluadoOMR { get; set; } = false;

    /// <summary>
    /// Especifica si hay errores de OMR
    /// </summary>
    [BsonElement("ero")]
    public bool ErroresOMR { get; set; } = false;

    /// <summary>
    /// Especifica si la evaluación tiene datos de OMR
    /// </summary>
    [BsonElement("do")]
    public bool DatosRecibidosOMR { get; set; } = false;

    /// <summary>
    /// MEtadatos del proceos de OMR para el cálculo de la calificación
    /// </summary>
    [BsonElement("ee")]
    public List<ElementoEvaluacionOMR> ElementosEvaluacion { get; set; } = [];

    /// <summary>
    /// Bitácora de procesamiento
    /// </summary>
    [BsonElement("bp")]
    public List<EventoProcesoEvaluacion> BitacoraProceso { get; set; } = [new EventoProcesoEvaluacion() { Estado = EstadoProcesoEvaluacion.SinEvaluar, Fecha = DateTime.UtcNow }];

    /// <summary>
    /// Imágenes utilizadas para el proceso de evaluación
    /// </summary>
    [BsonElement("im")]
    public List<ImagenEvaluacion>? Imagenes { get; set; }

}
