using comunes.primitivas.atributos;
using MongoDB.Bson.Serialization.Attributes;
using System.Diagnostics.CodeAnalysis;

namespace espaciotrabajo.model.espaciotrabajo;

/// <summary>
/// Define un miembro del espacio de trabajo
/// </summary>
[ExcludeFromCodeCoverage]
[EntidadDB]
public class Miembro
{
    /// <summary>
    /// Identificador único del miembro
    /// </summary>
    [BsonId]
    public required string UsuarioId { get; set; }

}
