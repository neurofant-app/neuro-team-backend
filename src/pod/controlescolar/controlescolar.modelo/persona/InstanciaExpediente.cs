using MongoDB.Bson.Serialization.Attributes;
using System.Diagnostics.CodeAnalysis;

namespace controlescolar.modelo.persona;

/// <summary>
/// Define una instancia de expediente para una persona de la escuela
/// </summary>

[ExcludeFromCodeCoverage]
public class InstanciaExpediente
{
    /// <summary>
    /// Identificador único del expediente
    /// </summary>
    [BsonElement("e")]
    public long ExpedienteId { get; set; }

    /// <summary>
    /// IDentificador único del almacenamiento de la instancia
    /// </summary>
    [BsonElement("a")]
    public Guid AlmacenamientoId { get; set; }

    /// <summary>
    /// Fecha de creación
    /// </summary>
    [BsonElement("fc")]
    public DateTime Creacion { get; set; }

    /// <summary>
    /// Fecha de actualización
    /// </summary>
    [BsonElement("fa")]
    public DateTime? Actualizacion { get; set; }

    /// <summary>
    /// INstancias de los documentos asociados al expediente
    /// </summary>
    [BsonElement("d")]
    public List<InstanciaDocumento> Documentos { get; set; } = [];

    /// <summary>
    /// Determina si el expeiente se encuentra completo en base a los documentos forzosos
    /// </summary>
    [BsonElement("co")]
    public bool Completo { get; set; }

    /// <summary>
    /// Determina si el expeiente se encuentra caducado en base a los documentos no permanentes
    /// </summary>
    [BsonElement("ca")]
    public bool Caduco { get; set; }
}
