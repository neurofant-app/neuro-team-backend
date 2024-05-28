using comunes.primitivas.atributos;
using MongoDB.Bson.Serialization.Attributes;

namespace seguridad.modelo;

/// <summary>
/// Representa un grupo de usuario 
/// </summary>
[EntidadDB]
public class GrupoUsuarios
{
    /// <summary>
    /// Identificador unico del grupo
    /// </summary>
    [BsonId]
    public virtual Guid Id { get; set; }

    /// <summary>
    /// Identificador único de la dominio en el que aplica la configuracion, este Id será propoerionado por un sistema externo
    /// </summary>
    [BsonElement("did")]
    public required string DominioId { get; set; }

    /// <summary>
    /// Identificador único de la aplicación, este Id será propoerionado por un sistema externo
    /// </summary>
    [BsonElement("app")]
    public required string ApplicacionId { get; set; }

    /// <summary>
    /// Lista de los usuarios pertenecentes al grupo
    /// </summary>
    [BsonElement("uid")]
    public List<string> UsuarioId { get; set; } = [];

    /// <summary>
    /// Nombre del grupo para la UI
    /// </summary>
    [BsonElement("n")]
    public required string Nombre { get; set; }

    /// <summary>
    /// Descripción del grupo
    /// </summary>
    [BsonElement("d")]
    public string? Descripcion { get; set; }
}
