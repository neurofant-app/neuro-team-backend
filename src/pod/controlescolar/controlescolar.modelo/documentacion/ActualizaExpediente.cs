using controlescolar.modelo.comunes;
using MongoDB.Bson.Serialization.Attributes;
using System.Diagnostics.CodeAnalysis;

namespace controlescolar.modelo.documentacion;

/// <summary>
/// DTO para la creación de un expediente
/// </summary>
[ExcludeFromCodeCoverage]
public class ActualizaExpediente
{
    /// <summary>
    /// Identificador único del documento
    /// </summary>
    public required long Id { get; set; }

    /// <summary>
    /// Nombre internacionalizable del expediente
    /// <summary>
    /// </summary>
    [BsonElement("n")]
    public required List<ValorI18N<string>> Nombre { get; set; }

    /// <summary>
    /// Descripcion del expediente
    /// </summary>
    public required List<ValorI18N<string>>? Descripcion { get; set; } = [];


    /// <summary>
    /// Identificador del rol a que pertenece el expediente
    /// </summary>
    [BsonElement("r")]
    public required  long RolEscolarId { get; set; }

}
