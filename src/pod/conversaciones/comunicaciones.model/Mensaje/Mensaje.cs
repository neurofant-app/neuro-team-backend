using MongoDB.Bson.Serialization.Attributes;

namespace comunicaciones.model;
/// <summary>
/// Contiene la información a intercambiar y será enviado por un canal en base
/// a la lista de canales disponibles
/// </summary>
public class Mensaje
{
    [BsonId]
    public string Id { get; set; }
    [BsonElement("c")]
    public string Cuerpo { get; set; }
    [BsonElement("e")]
    public string? Encabezado { get; set; }
    [BsonElement("pid")]
    public string? PlantillaId { get; set; }
    [BsonElement("cu")]
    public string? CargaUtil { get; set; }
    [BsonElement("fc")]
    public DateTime FechaCreacion { get; set; }
    [BsonElement("fe")]
    public DateTime? FechaEnvio { get; set; }
    /// <summary>
    /// Es true cuando ocurre un error de envio
    /// </summary>
    [BsonElement("er")]
    public bool ErrorEnvio { get; set; }
    /// <summary>
    /// Es una pequeña referencia al error detectado
    /// </summary>
    [BsonElement("cer")]
    public string? CodigoError { get; set; }
    /// <summary>
    /// En conversaciones bidireccionales almacena el CortoId del emisor.
    /// Es null en conversaciones unidireccionales
    /// </summary>
    [BsonElement("eid")]
    public string? EmisorId { get; set; }
    /// <summary>
    /// Longitud del mensaje
    /// </summary>
    [BsonElement("b")]
    public long Bytes { get; set; }
    /// <summary>
    /// Id del prepago utilizado para el envío
    /// </summary>
    [BsonElement("ppd")]
    public string? PrepagoId { get; set; }
}
