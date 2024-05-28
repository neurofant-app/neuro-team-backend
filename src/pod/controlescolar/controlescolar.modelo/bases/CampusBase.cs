using MongoDB.Bson.Serialization.Attributes;

namespace controlescolar.modelo;

/// <summary>
/// DEfine un sitio de enseñanza real o virtual
/// </summary>
public abstract class CampusBase
{
    /// <summary>
    /// Nombre del campus
    /// </summary>
    [BsonElement("n")]
    public virtual required string Nombre { get; set; }

    /// <summary>
    /// DEtermina si un campus se encuentr activo o inactivo
    /// </summary>
    [BsonElement("act")]
    public virtual required bool Activo { get; set; } = true;

    /// <summary>
    /// Especifica si el campus es virtual
    /// </summary>
    [BsonElement("v")]
    public virtual required bool Virtual { get; set; }

}
