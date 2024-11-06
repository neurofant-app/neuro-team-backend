using controlescolar.modelo.documentacion;
using MongoDB.Bson.Serialization.Attributes;
using System.Diagnostics.CodeAnalysis;

namespace controlescolar.modelo.persona;

/// <summary>
/// Define una instancia para un documento del expediente
/// </summary>
[ExcludeFromCodeCoverage]
public class InstanciaDocumento
{
    /// <summary>
    /// Identificador único del documento
    /// </summary>
    [BsonElement("d")]
    public long DocumentoId { get; set; }

    /// <summary>
    /// Fecha de creación
    /// </summary>
    [BsonElement("fc")]
    public DateTime Creacion { get; set; }

    /// <summary>
    /// Fecha de actualización
    /// </summary>
    [BsonElement("fa")]
    public DateTime? Actualizacion { get; set; }

    /// <summary>
    /// Identificador único del BLOB en el almacenamiento asociado
    /// Al momento de crearse puede no exister el BLOB 
    /// </summary>
    [BsonElement("id")]
    public string? BlobId { get; set; }

    /// <summary>
    /// Tamaño del contenido en bytes
    /// </summary>
    [BsonElement("t")]
    public long Tamano { get; set; } = 0;

    /// <summary>
    /// Tipo MIME del contenido
    /// </summary>
    [BsonElement("tm")]
    public string? TipoMime { get; set; }

    /// <summary>
    /// Indica si el contenido es el activo, en aca actualizacion el mas reciente se torna activo
    /// </summary>
    [BsonElement("a")]
    public bool Activo { get; set; }

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
    /// Determina si el expeiente se encuentra completo en base al tipo de permanencia
    /// </summary>
    [BsonElement("ca")]
    public bool Completo { get; set; } = false;

    /// <summary>
    /// Determina si el expeiente se encuentra caducado en base al tipo de caducidad
    /// </summary>
    [BsonElement("ca")]
    public bool Caduco { get; set; } = false;
}
