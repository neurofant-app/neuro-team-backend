
using MongoDB.Bson.Serialization.Attributes;

namespace comunicaciones.model;

public class PrepagoDisponible
{
    [BsonId]
    public string PrepagoId { get; set; }
    [BsonElement("eid")]
    public string EmisorId { get; set; }
    [BsonElement("c")]
    public TipoCanal Canal { get; set; }
    [BsonElement("mr")]
    public long MensajesRestantes { get; set; }
    [BsonElement("br")]
    public long BytesRestantes { get; set; }
}
