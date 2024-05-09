using comunes.primitivas.atributos;
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;

namespace aplicaciones.model;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
/// <summary>
/// Define una aplicación orbetivo para el sistema de invitaciones
/// por ejemeplo contabee o eccordion
/// </summary>
[EntidadDB]
public class EntidadAplicacion
{
    /// <summary>
    /// Identificador único de la aplicación
    /// </summary>
    [BsonId]
    public virtual Guid Id { get; set; }
    // Requerida 
    // [A] [D]

    /// <summary>
    /// Nombre de la aplicación que emite la invitación
    /// </summary>
    [BsonElement("n")]
    public required  string Nombre { get; set; }
    // Requerida 200
    // [I] [A] [D]


    /// <summary>
    /// Especifica si la aplicación se encuentra activa, solo es posible emitir notificaciones so lo está
    /// </summary>
    [BsonElement("ac")]
    public bool Activa { get; set; }
    // Requerida
    // [I] [A] [D]


    /// <summary>
    /// Lista de invitaciones asociadass a una aplicación
    /// </summary>
    [BsonIgnore]
    [JsonIgnore]
    public IEnumerable<EntidadInvitacion>? Invitaciones { get; set; }
    
    [BsonIgnore]
    [JsonIgnore]
    public IEnumerable<EntidadPlantillaInvitacion> Plantillas { get; set; }
    
    [BsonIgnore]
    [JsonIgnore]
    public IEnumerable<EntidadLogoAplicacion> Logotipos { get; set; }

    [BsonIgnore]
    [JsonIgnore]
    public IEnumerable<EntidadConsentimiento> Consentimientos { get; set; }
}
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
