using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.Text.Json.Serialization;
using comunes.primitivas.atributos;

namespace aplicaciones.model;
[EntidadDB]
/// <summary>
/// Define laplantilla de contenido para las comunicaciones con los usuarios del sistema
/// </summary>
public class EntidadPlantillaInvitacion
{
    /// <summary>
    /// Identificador único de la plantila
    /// </summary>
    [BsonId]
    public virtual Guid Id { get; set; }
    // Requerida
    // [A] [D]

    /// <summary>
    /// Tipo de contenido de la plantilla   
    /// </summary>
    [BsonElement("tc")]
    public TipoContenido TipoContenido { get; set; }
    // Requerida
    // [I] [A] [D]


    /// <summary>
    /// Identificador único de la aplicación a la qu pertenece la plantilla
    /// </summary>
    [BsonElement("aid")]
    public required Guid AplicacionId { get; set; }
    // Requerida
    // [I] [A] [D]

    /// <summary>
    /// Contenido para la plantilla
    /// </summary>
    [BsonElement("p")]
    public required string Plantilla { get; set; }
    // Requerida TAMAÑO MAXIMO EN LA BASE DE DATOS
    // [I [A] [D]
    //


    /// <summary>
    /// Aplicación asociada a la invitación
    /// </summary>
    [JsonIgnore]
    [BsonIgnore]
    public EntidadAplicacion Aplicacion { get; set; }
}
