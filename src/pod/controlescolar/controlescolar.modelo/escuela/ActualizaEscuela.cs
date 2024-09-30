using MongoDB.Bson.Serialization.Attributes;
using System.Diagnostics.CodeAnalysis;

namespace controlescolar.modelo.escuela;

/// <summary>
/// DTO de API para la actualización de una escuela
/// </summary>
[ExcludeFromCodeCoverage]
public class ActualizaEscuela
{
    /// <summary>
    /// Id de la escuela para actualizar
    /// </summary>
    public virtual Guid Id { get; set; }

    /// <summary>
    /// Nombre de la escuela
    /// </summary>
    public required string Nombre { get; set; }

    /// <summary>
    /// Clave de la escuela para uso local por ejemplo del sistema escolar nacional
    /// </summary>
    [BsonElement("c")]
    public string? Clave { get; set; }
}
