using MongoDB.Bson.Serialization.Attributes;

namespace comunicaciones.model;
/// <summary>
/// Es un contenedor de mensajes para uno o más destinatarios participantes.
/// </summary>
public class Conversacion
{
    [BsonId]
    public string Id { get; set; }
    /// <summary>
    /// Para las conversaciones unidireccionales contiene los datos del emisor
    /// </summary>
    [BsonElement("e")]
    public Participante? Emisor { get; set; }
    [BsonElement("lp")]
    public List<Participante> Participantes{ get; set; }
    [BsonElement("c")]
    public TipoCanal Canal { get; set; }
    [BsonElement("n")]
    public string? Nombre { get; set; }
    [BsonElement("fc")]
    public DateTime FechaCreacion { get; set; }
    [BsonElement("fa")]
    public DateTime FechaActualizacion { get; set; }
    [BsonElement("cm")]
    public int CantidadMensajes { get; set; }
    [BsonElement("lm")]
    public List<Mensaje> Mensajes { get; set; }
    /// <summary>
    /// True indica que la conversacion es unidireccional,
    /// False indica que la conversacion es bidireccional
    /// </summary>
    [BsonElement("u")]
    public bool Unidireccional { get; set; } = true;
}
