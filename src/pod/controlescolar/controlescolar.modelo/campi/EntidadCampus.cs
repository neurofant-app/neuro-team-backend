using comunes.primitivas.atributos;
using comunes.primitivas.entidades;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;

namespace controlescolar.modelo.campi;

/// <summary>
/// Define un sitio de enseñanza real o virtual
/// </summary>
[EntidadDB]
public class EntidadCampus : CampusBase, IEntidadCuenta
{
    /// <summary>
    /// Identificador único del campus en el repositorio, se genera al crear un registro
    /// </summary>
    [BsonId]

    public virtual Guid Id { get; set; }

    /// <summary>
    /// Identificador único de la cuenta que administra el campus
    /// Se obtiene del contexto por ejemplo un claim de JWT
    /// </summary>
    [BsonElement("cid")]
    public Guid CuentaId { get; set; }

    /// <summary>
    /// Identificador único del campo padre
    /// </summary>
    [BsonElement("caid")]
    public Guid? CampusPadreId { get; set; }

}
