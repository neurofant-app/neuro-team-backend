using controlescolar.modelo.comunes;
using MongoDB.Bson.Serialization.Attributes;
using System.Diagnostics.CodeAnalysis;

namespace controlescolar.modelo.documentacion;

/// <summary>
/// Define un documento perteneciente a un expediente
/// </summary>
[ExcludeFromCodeCoverage]
public class EntidadDocumentoBase
{

    [BsonId]
    public long Id { get; set; }

    /// <summary>
    /// Nombre asigando al documento
    /// </summary>
    [BsonElement("n")]
    public required List<ValorI18N<string>> Nombre { get; set; } = [];

    /// <summary>
    /// Descripcion del documento
    /// </summary>
    [BsonElement("n")]
    public required List<ValorI18N<string>>? Descripcion { get; set; } = [];


    /// <summary>
    /// Determina si el documento se encuentra activo para su uso
    /// </summary>
    [BsonElement("a")]
    public bool Activo { get; set; } = true;

    /// <summary>
    /// Fecha de creación del documento
    /// </summary>
    [BsonElement("f")]
    public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;


    /// <summary>
    /// Determina si el documento es permanente o tiene una caducidad
    /// </summary>
    [BsonElement("c")]
    public TipoCaducidadDocumento Caducidad { get; set; }

    /// <summary>
    /// Ámbito de aplicación del documento
    /// </summary>
    [BsonElement("b")]
    public TipoAmbitoDocumento Ambito { get; set; }

    /// <summary>
    /// En el caso de que el ámbito sea de plantel especifica el plantel al que aplica el documento
    /// </summary>
    [BsonElement("p")]
    public long? PlantelId { get; set; }

    /// <summary>
    /// Determina si un documento es opcional
    /// </summary>
    [BsonElement("o")]
    public bool Opcional { get; set; }

}
