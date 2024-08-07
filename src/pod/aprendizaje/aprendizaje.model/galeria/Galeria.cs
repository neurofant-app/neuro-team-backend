﻿using comunes.primitivas.atributos;
using MongoDB.Bson.Serialization.Attributes;
using System.Diagnostics.CodeAnalysis;

namespace aprendizaje.model.galeria;

/// <summary>
/// DEfine auna galería para el almacenamiento de los medios de soporte al aprendizaje
/// </summary>
[ExcludeFromCodeCoverage]
[EntidadDB]
public class Galeria
{
    /// <summary>
    /// IDentificador único de la galería
    /// </summary>
    [BsonId]
    public Guid Id { get; set; }

    /// <summary>
    /// Didentificador único del espacio de trabajo al que pertenece la Neurona,
    /// Los espacios de trabajo son creados por usuarios suscritos a NeuroPad
    /// </summary>
    [BsonElement("eid")]
    public required string EspacioTrabajoId { get; set; }

    /// <summary>
    /// Nombre de la galería
    /// </summary>
    [BsonElement("n")]
    public required string Nombre { get; set; }

    /// <summary>
    /// Fecha de creación o actualizaicón de la galería
    /// </summary>
    [BsonElement("f")]
    public DateTime Fecha { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Contenido de la galería
    /// </summary>
    [BsonElement("c")]
    public List<Contenido> Contenido { get; set; } = [];

    /// <summary>
    /// Determina si la galería permite ser añadida por cualquier espacio de trabajo
    /// </summary>
    [BsonElement("p")]
    public bool Publica { get; set; }

    /// <summary>
    /// Lista de los espacios vinculdaos que tienen acceso a la galería en modo lectura
    /// </summary>
    [BsonElement("ev")]
    public List<Guid> EspaciosVinculadosLextura { get; set; } = [];

    /// <summary>
    /// Lista de los temas galeria que tienen acceso a la galería
    /// </summary>
    [BsonElement("ltg")]
    public List<TemaGaleria> ListaTemasGaleria { get; set; } = [];

}
