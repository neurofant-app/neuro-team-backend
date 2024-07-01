using comunes.primitivas.atributos;
using MongoDB.Bson.Serialization.Attributes;

namespace creador.contenido.model;
/// <summary>
/// Define un EspacioTrabajo
/// </summary>
[EntidadDB]
public class EntidadEspacioTrabajo
{
    /// <summary>
    /// Id único de la entidad.
    /// </summary>
    [BsonId]
    public Guid Id { get; set; }
    // Requerida // [A] [D]

    /// <summary>
    /// Id del usuario creador, se toma del JWT.
    /// </summary>
    [BsonElement("uid")]
    public Guid UsuarioId { get; set; }
    // Requerida // [I]

    /// <summary>
    /// Nombre del espacio de trabajo.
    /// </summary>
    [BsonElement("n")]
    public string Nombre { get; set; }
    // Requerida // [I] [A] [D]

    /// <summary>
    /// Fecha de creación.
    /// </summary>
    [BsonElement("fc")]
    public DateTime FechaCreacion { get; set; }
    // Requerida // [D]

    /// <summary>
    /// Enumeración del estado de la entidad.
    /// </summary>
    [BsonElement("e")]
    public EstadoEntidad Estado { get; set; }
    // Requerida // [A] [D]

    /// <summary>
    /// Fecha en que marcó para eliminación.
    /// </summary>
    [BsonElement("fe")]
    public DateTime? FechaEliminacion { get; set; }
    // [D]
}
