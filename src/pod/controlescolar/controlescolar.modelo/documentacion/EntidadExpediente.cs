using comunes.primitivas.atributos;
using controlescolar.modelo.comunes;
using MongoDB.Bson.Serialization.Attributes;
using System.Diagnostics.CodeAnalysis;

namespace controlescolar.modelo.documentacion;

/// <summary>
/// Define una serie de documentos relevantes relacionados a una entidad
/// </summary>
[ExcludeFromCodeCoverage]
[EntidadDB]
public class EntidadExpediente
{
    /// <summary>
    /// Identificador único del expediente
    /// </summary>
    [BsonId]
    public long Id { get; set; }

    /// <summary>
    /// Nombre asigando al expediente
    /// </summary>
    [BsonElement("n")]
    public required List<ValorI18N<string>> Nombre { get; set; } = [];

    /// <summary>
    /// Descripcion del expediente
    /// </summary>
    [BsonElement("d")]
    public required List<ValorI18N<string>>? Descripcion { get; set; } = [];

    /// <summary>
    /// Determina si el expediente se encuentra activo para su uso
    /// </summary>
    [BsonElement("a")]
    public bool Activo { get; set; } = true;

    /// <summary>
    /// IDentificador del rol a que pertenece el expediente
    /// </summary>
    [BsonElement("r")]
    public long RolEscolarId { get; set; }

    /// <summary>
    /// Fecha de creación del expediente
    /// </summary>
    [BsonElement("f")]
    public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// DOcumentos asociados al rol
    /// </summary>
    [BsonElement("dc")]
    public List<EntidadDocumentoBase> Documentos { get; set; } = [];

}
