using MongoDB.Bson.Serialization.Attributes;

namespace conversaciones.model;

public class Contenido
{
    [BsonId]
    public string Id { get; set; }
    [BsonElement("t")]
    public TipoCanal Canal { get; set; }
    [BsonElement("c")]
    public string Cuerpo { get; set; }
    [BsonElement("e")]
    public string? Encabezado { get; set; }
    [BsonElement("i")]
    public string? Idioma { get; set; }
}
