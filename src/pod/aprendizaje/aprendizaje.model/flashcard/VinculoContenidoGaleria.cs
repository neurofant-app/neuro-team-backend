﻿using MongoDB.Bson.Serialization.Attributes;

namespace aprendizaje.model.flashcard;

/// <summary>
/// Vinculo a los medios de la galería
/// </summary>
public class VinculoContenidoGaleria
{
    /// <summary>
    /// Identificador único de la galería
    /// </summary>
    [BsonElement("gid")]
    public Guid GaleriaId { get; set; }

    /// <summary>
    /// Identificador único del contenido
    /// </summary>
    [BsonElement("cid")]
    public Guid ContenidoId { get; set; }

    /// <summary>
    /// Id único del contenido en el almacenamiento, 
    /// este id se utiliza como identificador de versiones para el contenido referenciado por el Id
    /// Cuando un COntenido camvbia se crea un nuevo archivo que tendrá un nuevo ID de almacenamientp
    /// </summary>
    [BsonElement("alid")]
    public required string AlmacenamientoId { get; set; }

    /// <summary>
    /// Tipo MIME del contenido
    /// </summary>
    [BsonElement("tm")]
    public required string TipoMime { get; set; }

    /// <summary>
    /// Tamaño del contenido en bytes
    /// </summary>
    [BsonElement("t")]
    public long Tamano { get; set; } = 0;
}
