using comunes.primitivas.atributos;
using MongoDB.Bson.Serialization.Attributes;


namespace controlescolar.modelo.Planteles;

[EntidadDB]
public class EntidadPlanteles
{
    /// <summary>
    /// Identificador único del Plantel en el repositorio, se genera al crear un registro
    /// </summary>
    [BsonId]
    public virtual Guid Id { get; set; }
    /// <summary>
    /// IDentificadpr único del escuela dentro del plantel por ejemplo clave de la escuela 
    /// </summary>

    [BsonElement("eid")]
    public Guid EscuelaId { get; set; }
    /// <summary>
    /// IDentificadpr único de la dirección dentro del plantel 
    /// </summary>

    [BsonElement("d")]
    public Guid? DireccionId { get; set; }
    /// <summary>
    /// Nombre del plantel
    /// </summary>
    public required string Nombre { get; set; }
    /// <summary>
    /// clave del plantel
    /// </summary>
    public string? Clave { get; set; }


}
