using MongoDB.Bson.Serialization.Attributes;

namespace comunicaciones.model;

public class Emisor
{
    /// <summary>
    /// Identificador único del emisor
    /// </summary>
    [BsonId]
    public string Id { get; set; }
    /// <summary>
    /// Nombre distintivo del emisor
    /// </summary>
    [BsonElement("n")]
    public string Nombre { get; set; }
    /// <summary>
    /// Identificador del usuario asociado al emisor en el servicio de
    /// identidad un mismo usuario puede poseer varios emisores
    /// </summary>
    [BsonElement("uid")]
    public string UsuarioId { get; set; }
    /// <summary>
    /// Lista de prepagos adquiridos
    /// </summary>
    [BsonElement("lp")]
    public List<Prepago> Prepagos { get; set; }
    /// <summary>
    /// Mantiene una lista de prepago disponible
    /// </summary>
    [BsonElement("pd")]
    public List<PrepagoDisponible> Disponible { get; set; }
}
