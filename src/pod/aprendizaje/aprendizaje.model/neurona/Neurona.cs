using aprendizaje.model.precios;
﻿using comunes.primitivas.atributos;
using MongoDB.Bson.Serialization.Attributes;
using System.Diagnostics.CodeAnalysis;

namespace aprendizaje.model.neurona;

/// <summary>
/// Una neuorna representa un conjunto de medios y actividades de aprendizaje vinculados a un temario
/// </summary>
[ExcludeFromCodeCoverage]
[EntidadDB]
public class Neurona
{
    /// <summary>
    /// IDentificador único de la neurona
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
    /// Idiomas en los que se ofrece el contenido  de la Neurona
    /// </summary>
    [BsonElement("i")]
    public List<string> Idiomas { get; set; } = [];

    /// <summary>
    /// Nombre de la neurona por idioma, este nombre será utilziado por la búsqeuda 
    /// de texto en el mercado de neurofant
    /// </summary>
    [BsonElement("n")]
    public List<ValorI18N<string>> Nombre { get; set; } = [];

    /// <summary>
    /// Descripción de la neurona por idioma, esta descripción será utilziado por la búsqeuda 
    /// de texto en el mercado de neurofant
    /// </summary>
    [BsonElement("d")]
    public List<ValorI18N<string>> Descripcion { get; set; } = [];

    /// <summary>
    /// Identificador único del temario que utiliza la neurona como plan de estudios
    /// </summary>
    [BsonElement("tid")]
    public Guid TemarioId { get; set; }


    /// <summary>
    /// Estado de publicación de la neurona
    /// </summary>
    [BsonElement("ep")]
    public EstadoContenido EstadoPublicacion { get; set; }


    /// <summary>
    /// Versión de la neurona, este valor se incremente en cada publicación de manera automática
    /// </summary>
    [BsonElement("v")]
    public string Version { get; set; } = "";

    /// <summary>
    /// Identificador de la neurona a partir del cual se derivó la actual
    /// </summary>
    [BsonElement("nid")]
    public Guid? NeuronaDerivadaId { get; set; }


    /// <summary>
    /// Fecha de creación de la neurona
    /// </summary>
    [BsonElement("fc")]
    public DateTime FechaCreacion { get; set; }

    /// <summary>
    /// Fscha de la última actualización a la neurona
    /// </summary>
    [BsonElement("fa")]
    public DateTime FechaActualizacion { get; set; }

    /// <summary>
    /// Fecha de la última consulta a la neuroa por terceos
    /// </summary>
    [BsonElement("fct")]
    public DateTime? FechaConsulta { get; set; }

    /// <summary>
    /// Identificador único del almacenamiento de los datos de la neurona
    /// </summary>
    [BsonElement("ad")]
    public Guid AlmacenamientoId { get; set; }

    /// <summary>
    /// Identificador único de la galería a locala la neurona si existe una.
    /// A veces la neurona solo consume recursos de su galería local.
    /// </summary>
    [BsonElement("gid")]
    public Guid? GaleriaId { get; set; }

    /// <summary>
    /// Tipo de licenciamiento para la neurona
    /// </summary>
    [BsonElement("t")]
    public TipoLiceciaNeurona TipoLicencia { get; set; }

    /// <summary>
    /// Número de Flashcards existentes en la neurona
    /// </summary>
    [BsonElement("cf")]
    public long ConteoFlashcards { get; set; } = 0;


    /// <summary>
    /// Número de Actividades de evaluación existentes en la neurona
    /// </summary>
    [BsonElement("ca")]
    public long ConteoActividades { get; set; } = 0;

    /// <summary>
    /// Número de descargas de la neurona desde el mercado o por grupos de usuarios
    /// </summary>
    [BsonElement("cd")]
    public long ConteoDescargas { get; set; }

    /// <summary>
    /// Guarda un número que corresponde al Identificado del siguiente 
    /// objeto a insertar asociado a la neurona por ejemplo una flashcard o actividad
    /// </summary>
    [BsonElement("s")]
    public long SecuenciaObjetos { get; set; }

    /// <summary>
    /// Flashcards asociados a la neuroa
    /// </summary>
    [BsonElement("fs")]
    public List<FlashcardNeurona> Flashcards { get; set; } = [];

    /// <summary>
    /// Identificadores de las actividades de aprendizaje asociados a la neuroa
    /// </summary>
    [BsonElement("acd")]
    public List<long> ActividadesIds { get; set; } = [];

    /// <summary>
    /// Identificador del último cambio existente en la neurona, el valor inicial es cero,
    /// cada actualización trae la fecha UTC convertida a ticks de net core
    /// </summary>
    [BsonElement("ts")]
    public long TimeStampt { get; set; } = 0;

    /// <summary>
    /// Lista de eventos asociados a la neurona
    /// </summary>
    [BsonIgnore]
    public List<EventoNeurona> Eventos { get; set; } = [];

    /// <summary>
    /// Lista de precios asociada a la neurona
    /// </summary>
    [BsonIgnore]
    public List<Precio> Precios { get; set; } = [];
}
