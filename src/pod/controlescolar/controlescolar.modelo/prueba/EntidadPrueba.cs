

using MongoDB.Bson.Serialization.Attributes;

namespace controlescolar.modelo.prueba;

public class EntidadPrueba
{
    [BsonId]
    public virtual Guid Id { get; set; }

    [BsonElement("n")]
    public string? Nombre { get; set; }

    [BsonElement("fn")]
    public DateTime FechaNacimiento { get; set; }

    [BsonElement("e")]
    public int Edad { get; set; }

    [BsonElement("p")]
    public decimal Precio { get; set; }

    [BsonElement("s")]
    public long Serie { get; set; }

    [BsonElement("a")]
    public bool Activo{ get; set; }

}
