using comunes.primitivas.atributos;
using MongoDB.Bson.Serialization.Attributes;
using System.Diagnostics.CodeAnalysis;

namespace espaciotrabajo.model.espaciotrabajo;

/// <summary>
/// Define un espacio de trabajo para los creadores de aprendizaje
/// </summary>
[ExcludeFromCodeCoverage]
[EntidadDB]
public class EspacioTrabajo
{
    /// <summary>
    /// Identificador único del espacio de trabajo
    /// </summary>
    [BsonId]
    public Guid? Id { get; set; }

    /// <summary>
    /// Nombre del espacio de trabajo
    /// </summary>
    [BsonElement("n")]
    public required string Nombre { get; set; }

    /// <summary>
    /// Identificador del dueño al que pertenece el espacio de trabajo
    /// Regularmente va a ser el Id del usuario creador
    /// </summary>
    [BsonElement("tid")]
    public Guid TenantId { get; set; }

    /// <summary>
    /// Identificador único de la aplicacion asociada al espaco de trabajo, por ejemplo NeuroTeam  o NeuroPad
    /// </summary>
    [BsonElement("aid")]
    public required string AppId { get; set; }

    /// <summary>
    /// Miembros del espacios
    /// </summary>
    [BsonElement("m")]
    List<Miembro> Miembros { get; set; } = [];
}
