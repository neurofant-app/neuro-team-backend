using MongoDB.Bson.Serialization.Attributes;

namespace seguridad.modelo;

/// <summary>
/// Especifica las propiedades de seguridad de una aplicación
/// </summary>
public class Aplicacion
{

    /// <summary>
    /// Identificador único de la aplicación, este Id será propoerionado por un sistema externo
    /// </summary>
    [BsonId()]
    public required string ApplicacionId { get; set; }


    /// <summary>
    /// Modulos de una aplicacion
    /// </summary>
    [BsonElement("m")]
    public List<Modulo> Modulos { get; set; } = [];


    /// <summary>
    /// Nombre del módulo para la UI, esto será calcolado en base al idioa
    /// </summary>
    [BsonElement("n")]
    public string? Nombre { get; set; }

    /// <summary>
    /// Descripción del módulo para la UI, esto será calcolado en base al idioa
    /// </summary>
    [BsonElement("d")]
    public string? Descripcion { get; set; }
}
