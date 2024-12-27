using evaluacion.model.restricciones;
using MongoDB.Bson.Serialization.Attributes;

namespace evaluacion.model.ejecucionevaluacion;

/// <summary>
/// Datos de ejecución de una evaluación
/// </summary>
public class EjecucionEvaluacion
{    
    /// <summary>
     /// Identificador único de le ejecución de la variante de evaluación
     /// </summary>
    [BsonId]
    public Guid Id { get; set; }

    /// <summary>
    /// Identificador de la evaluación 
    /// </summary>
    [BsonElement("eid")]
    public Guid EvaluacionId { get; set; }

    /// <summary>
    /// Identificador único del creador de la ejecución
    /// </summary>
    [BsonElement("cid")]
    public Guid CreadorId { get; set; }

    /// <summary>
    /// Fecha de creación de la ejecución evaluación
    /// </summary>
    [BsonElement("fc")]
    public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Medios de ejecusión de la ejecución evaluación
    /// </summary>
    [BsonElement("me")]
    public List<MedioEjecucionEvaluacion> MediosEjecucion { get; set; } = [];

    /// <summary>
    /// Identificador único de la plantilla de impresión
    /// </summary>
    [BsonElement("pid")]
    public string? PlantillaImpresionId { get; set; }

    /// <summary>
    /// Especifica cómo se imprimiran los alveolos de respuestas
    /// </summary>
    [BsonElement("na")]
    public TipoNumeracionAlveolos NumeracionAlveolos { get; set; } = TipoNumeracionAlveolos.Alfabetica;

    /// <summary>
    /// Estado de la variante de evaluación
    /// </summary>
    [BsonElement("e")]
    public EstadoEjecucionEvaluacion Estado { get; set; } = EstadoEjecucionEvaluacion.Diseno;

    /// <summary>
    /// Especifica si los participantes de le evalaución se asignan dinámicamnete, incripción durante la ejecución
    /// o si son fijos definidos por el eveluador
    /// </summary>
    [BsonElement("pd")]
    public bool ParticipantesDinamicos { get; set; } = false;

    /// <summary>
    /// Determina si la ejecución se encuentra restringida en fecha
    /// </summary>
    [BsonElement("rf")]
    public bool RestringidaFecha { get; set; }

    /// <summary>
    /// Restricción de ejecución en un periodo
    /// </summary>
    [BsonElement("resf")]
    public RestriccionEjecucionFecha? RestriccionFecha { get; set; }


    /// <summary>
    /// Si es necesaria una contrasena para el acceso almacena True
    /// </summary>
    [BsonElement("apass")]
    public bool AccesoContrasena { get; set; }

    /// <summary>
    /// Especifica una contraseña para acceder a la evaluación, la contraseña se almacena como un hash
    /// </summary>
    [BsonElement("pass")]
    public RestriccionContrasena? RestriccionContrasena { get; set; }

    /// <summary>
    /// Lista de variantes contenidas en la ejecución
    /// </summary>
    [BsonElement("vs")]
    public List<EjecucionVarianteEvaluacion> Variantes { get; set; } = [];

}
