using MongoDB.Bson.Serialization.Attributes;

namespace comunicaciones.model;

public class Prepago
{
    [BsonId]
    public string PrepagoId { get; set; }
    /// <summary>
    /// Indica si el prepago tiene vigencia, sin vigencia , mensual o trimestral.
    /// </summary>
    [BsonElement("v")]
    public TipoVigencia Vigencia { get; set; }
    /// <summary>
    /// Fecha de vigencia del prepago
    /// </summary>
    [BsonElement("fv")]
    public DateTime? FechaVigencia { get; set; }
    [BsonElement("eid")]
    public string EmisorId { get; set; }
    [BsonElement("c")]
    public TipoCanal Canal { get; set; }
    [BsonElement("lm")]
    public long LimiteMensajes { get; set; }
    [BsonElement("lb")]
    public long LimiteBytes { get; set; }
    /// <summary>
    /// Especifica si los mensajes y bytes no utilizados pueden ser adicionados
    /// al siguiente prepago
    /// </summary>
    [BsonElement("a")]
    public bool Acumulable { get; set; }

}
