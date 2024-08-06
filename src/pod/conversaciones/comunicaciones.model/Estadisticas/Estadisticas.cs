using MongoDB.Bson.Serialization.Attributes;

namespace comunicaciones.model;

public class Estadisticas
{
    [BsonId]
    public string EmisorId { get; set; }
    [BsonElement("a")]
    public int Ano { get; set; }
    [BsonElement("m")]
    public int Mes { get; set; }
    [BsonElement("d")]
    public int Dia { get; set; }
    [BsonElement("c")]
    public TipoCanal Canal { get; set; }
    [BsonElement("co")]
    public long Conteo { get; set; }
    [BsonElement("coe")]
    public long ConteoErroneo { get; set; }
}
