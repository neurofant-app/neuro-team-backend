﻿using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations.Schema;

namespace aprendizaje.model.flashcard;

public abstract class FlashcardBase
{
    /// <summary>
    /// Identificador único de la tarjeta, este Id se genera y se controla 
    /// al interior de la neuora para minimizar el espacio de almaenamiento 
    /// en Mongo
    /// </summary>
    [BsonId]
    public long Id { get; set; }

    /// <summary>
    /// Identificador único de la Neurona a la que pertenece el flashcard
    /// </summary>
    [BsonElement("nid")]
    public Guid NeuronaId { get; set; }

    /// <summary>
    /// IDentificador único del tema al que pertenece la tarjeta
    /// </summary>
    [BsonElement("tid")]
    public Guid TemaId { get; set; }

    /// <summary>
    /// Identificador del último cambio existente en la neurona, el valor inicial el cero
    /// cada actualziación trae la fecha UTC convertida a ticks de net core
    /// </summary>
    [BsonElement("ts")]
    public long TimeStampt { get; set; } = 0;

    /// <summary>
    /// Determina el tipo base para el concepto de la tarjeta
    /// Sólo son adminitidos los tipos simples
    /// </summary>
    [BsonElement("tc")]
    public TipoBaseFlashcard TipoConcepto { get; set; } = TipoBaseFlashcard.Texto;

    /// <summary>
    /// Determina el tipo base del contenido de la tarjeta
    /// </summary>
    [BsonElement("tcn")]
    public TipoBaseFlashcard TipoContenido { get; set; }


    /// <summary>
    /// Contenido personalizado asociado, 
    /// el tipo base debe establecerse como Personalizado para su uso
    /// </summary>
    [BsonElement("cnp")]
    public Guid? ContenidoPersonalizadoId { get; set; }

    /// <summary>
    /// Para las tajetas de tipo básico, a excepción de las de texto, almacena la referencia al 
    /// elemento de la galería que apunta al contenido  
    /// </summary>
    [BsonElement("cng")]
    public List<ValorI18N<VinculoContenidoGaleria>> ContenidoGaleria { get; set; } = [];

    /// <summary>
    /// Contenido personalizado asociado
    /// </summary>
    [NotMapped]
    public ContenidoPersonalizado? ContenidoPersonalizado { get; set; }

}
