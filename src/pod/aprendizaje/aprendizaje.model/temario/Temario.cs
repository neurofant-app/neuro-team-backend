using comunes.primitivas.atributos;
using MongoDB.Bson.Serialization.Attributes;
using System.Diagnostics.CodeAnalysis;

namespace aprendizaje.model.temario;


/// <summary>
/// Describe un termario que sirve como plan de estudios para las actividades de aprendizaje
/// </summary>
[ExcludeFromCodeCoverage]
[EntidadDB]
public class Temario
{
    /// <summary>
    /// Identificador único del temario
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
    /// Determina si el temario es de acceso públic, cualquiera puede tener acceeo al mismo
    /// </summary>
    [BsonElement("p")]
    public bool Publico { get; set; }

    /// <summary>
    /// Lista de temas asociados al temario
    /// </summary>
    [BsonElement("t")]
    public List<Tema> Temas { get; set; } = [];

    /// <summary>
    /// Estado de publicación del temario
    /// </summary>
    [BsonElement("e")]
    public EstadoTemario EstadoPublicacion { get; set; }

    /// <summary>
    /// Versión del temario
    /// </summary>
    [BsonElement("v")]
    public string Version { get; set; } = "";

    /// <summary>
    /// Identificador del temario a partir del cual se derivó el actual
    /// </summary>
    [BsonElement("tid")]
    public Guid? TemarioDerivadoId { get; set; }

}
