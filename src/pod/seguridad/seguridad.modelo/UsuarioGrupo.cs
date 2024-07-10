using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace seguridad.modelo;

/// <summary>
/// Define aun usuario perteneciente aun grupo
/// </summary>
[ExcludeFromCodeCoverage]
public class UsuarioGrupo
{
    [BsonIgnore]
    /// <summary>
    /// Identificador único
    /// </summary>
    public required string Id { get; set; }

    /// <summary>
    /// Identificador único del usuario
    /// </summary>
    public required string UsuarioId { get; set; }
    
    [BsonIgnore]
    public Guid GrupoId { get; set; }

    [BsonIgnore]
    [JsonIgnore]
    public GrupoUsuarios GrupoUsuarios { get; set; }
}
