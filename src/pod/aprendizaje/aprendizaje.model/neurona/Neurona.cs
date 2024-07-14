using System.Diagnostics.CodeAnalysis;

namespace aprendizaje.model.neurona;

//  - - - DETALLES
//  
//  Esta entidad se almacena en mongo
//
//  - - - 

/// <summary>
/// Una neuorna representa un conjunto de medios y actividades de aprendizaje vinculados a un temario
/// </summary>
[ExcludeFromCodeCoverage]
public class Neurona
{
    /// <summary>
    /// IDentificador único de la neurona
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Didentificador único del espacio de trabajo al que pertenece la Neurona,
    /// Los espacios de trabajo son creados por usuarios suscritos a NeuroPad
    /// </summary>
    public required string EspacioTrabajoId { get; set; }

    /// <summary>
    /// Idiomas en los que se ofrece el contenido  de la Neurona
    /// </summary>
    public List<string> Idiomas { get; set; } = [];

    /// <summary>
    /// Nombre de la neurona por idioma, este nombre será utilziado por la búsqeuda 
    /// de texto en el mercado de neurofant
    /// </summary>
    public List<ValorI18N> Nombre { get; set; } = [];

    /// <summary>
    /// Descripción de la neurona por idioma, esta descripción será utilziado por la búsqeuda 
    /// de texto en el mercado de neurofant
    /// </summary>
    public List<ValorI18N> Descripcion { get; set; } = [];

    /// <summary>
    /// Identificador único del temario que utiliza la neurona como plan de estudios
    /// </summary>
    public Guid TemarioId { get; set; }


    /// <summary>
    /// Estado de publicación de la neurona
    /// </summary>
    public EstadoNeurona EstadoPublicacion { get; set; }


    /// <summary>
    /// Versión de la neurona
    /// </summary>
    public string Version { get; set; } = "";

    /// <summary>
    /// Identificador de la neurona a partir del cual se derivó la actual
    /// </summary>
    public Guid? NeuronaDerivadaId { get; set; }


    /// <summary>
    /// Fecha de creación de la neurona
    /// </summary>
    public DateTime FechaCreacion { get; set; }

    /// <summary>
    /// Fscha de la última actualización a la neurona
    /// </summary>
    public DateTime FechaActualizacion { get; set; }

    /// <summary>
    /// Fecha de la última consulta a la neuroa por terceos
    /// </summary>
    public DateTime? FechaConsulta { get; set; }


    /// <summary>
    /// Identificador único del almacenamiento de los datos de la neurona
    /// </summary>
    public Guid AlmacenamientoId { get; set; }

    /// <summary>
    /// Tipo de licenciamiento para la neurona
    /// </summary>
    public TipoLiceciaNeurona TipoLicencia { get; set; }

    /// <summary>
    /// Número de Flashcards existentes en la neurona
    /// </summary>
    public long ConteoFlashcards { get; set; } = 0;


    /// <summary>
    /// Número de Actividades de evaluación existentes en la neurona
    /// </summary>
    public long ConteoActividdades { get; set; } = 0;

    /// <summary>
    /// Número de descargas de la neurona desde el mercado o por grupos de usuarios
    /// </summary>
    public long ConteoDescargas { get; set; }

    /// <summary>
    /// Guarda un número que corresponde al Identificado del siguiente 
    /// obneto a insertar asociado a la neurona por ejemplo una flashcard o actividad
    /// </summary>
    public long SecuenciaObjetos { get; set; }

    /// <summary>
    /// Identificadores de los flashcards asociados a la neuroa
    /// </summary>
    public List<long> FlashCardIds { get; set; } = [];

    /// <summary>
    /// Identificadores de las actividades de aprendizaje asociados a la neuroa
    /// </summary>
    public List<long> ActividadesIds { get; set; } = [];

}
