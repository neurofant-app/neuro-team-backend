using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace aprendizaje.model.plan;

/// <summary>
/// Define un periodo escolar para la conclusió de un plan de estudios este periodo es solo de sieño curricular, 
/// no funciona para control escolar por ejemplo para saber que materias ha cursado un alumno o si se encuentra recursando
/// </summary>
public class Periodo
{
    /// <summary>
    /// Identificador único del periodo
    /// </summary>
    [BsonId]
    [BsonGuidRepresentation(GuidRepresentation.Standard)]
    public Guid Id { get; set; }

    /// <summary>
    /// Nombre del curso
    /// </summary>
    [BsonElement("n")]
    public List<ValorI18N<string>> Nombre { get; set; } = [];


    /// <summary>
    /// Periodo antecesor del actual
    /// </summary>
    [BsonElement("ai")]
    public Guid? AntecesorId { get; set; }

    /// <summary>
    /// Periodo antecesor del actual
    /// </summary>
    [BsonElement("si")]
    public Guid? SucesorId { get; set; }

    /// <summary>
    /// Dtermina si en el periodo pueden inlcuirse temarios de especialidad 
    /// </summary>
    [BsonElement("aes")]
    public bool AceptaEspecialidad { get; set; }

    /// <summary>
    /// DEtermina si los temarios del periodo pueden elegirse libremente, si es True la lista de obligatorios y opcionales 
    /// no juegan ningún papel y deben encontrarse vacía y el tipo de seleccion debe ser Ninguna
    /// </summary>
    [BsonElement("tl")]
    public bool TemariosLibres { get; set; };

    /// <summary>
    /// Lista de temarios obligatiors que deben cubrirse en el period
    /// </summary>
    [BsonElement("tr")]
    public List<Guid> TemariosObligatorios { get; set; } = [];

    /// <summary>
    /// Especifica el tipo de selección para los ptemario del periodo
    /// </summary>
    [BsonElement("trs")]
    public TipoSeleccionTemario TipoSeleccionObligatorios { get; set; }

        /// <summary>
    /// Cantidad mínima para los temarios obligatorios, 0 =no hay minimo
    /// </summary>
    [BsonElement("trm")]
    public int MinimoTemariosObligatorios { get; set; } = 0;


    /// <summary>
    /// Lista de temarios opcionales que deben cubrirse en el period
    /// </summary>
    [BsonElement("to")]
    public List<Guid> TemariosOpcionales { get; set; } = [];

    /// <summary>
    /// Especifica el tipo de selección para los temarios opcionales del periodo
    /// </summary>
    [BsonElement("tos")]
    public TipoSeleccionTemario TipoSeleccionOpcionales { get; set; }

    /// <summary>
    /// Cantidad mínima para los temarios opcionales, 0 =no hay minimo
    /// </summary>
    [BsonElement("tom")]
    public int MinimoTemariosOpcionales { get; set; } = 0;


    [BsonElement("cmin")]
    /// <summary>
    /// Número mínimo de creditos para cubrir en el periodo, 0 sin límite
    /// </summary>
    public int MinimoCreditos { get; set; } = 0;


    [BsonElement("cmax")]
    /// <summary>
    /// Número máximo de creditos para cubrir en el periodo, 0 sin límite
    /// </summary>
    public int MaximoCreditos { get; set; } = 0;


    [BsonElement("tmin")]
    /// <summary>
    /// Número mínimo de temarios para cubrir en el periodo, 0 sin límite
    /// </summary>
    public int MinimoTemarios { get; set; } = 0;


    [BsonElement("cmax")]
    /// <summary>
    /// Número máximo de temarios para cubrir en el periodo, 0 sin límite
    /// </summary>
    public int MaximoTemarios { get; set; } = 0;
}

