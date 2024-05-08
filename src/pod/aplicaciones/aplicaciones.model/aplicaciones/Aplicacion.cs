using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;

namespace aplicaciones.model;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
/// <summary>
/// Define una aplicación orbetivo para el sistema de invitaciones
/// por ejemeplo contabee o eccordion
/// </summary>
public class Aplicacion
{
    /// <summary>
    /// Identificador único de la aplicación
    /// </summary>
    [BsonId]
    public Guid Id { get; set; }
    // Requerida 
    // [A] [D]

    /// <summary>
    /// Nombre de la aplicación que emite la invitación
    /// </summary>
    public required  string Nombre { get; set; }
    // Requerida 200
    // [I] [A] [D]


    /// <summary>
    /// Especifica si la aplicación se encuentra activa, solo es posible emitir notificaciones so lo está
    /// </summary>
    public bool Activa { get; set; }
    // Requerida
    // [I] [A] [D]


    /// <summary>
    /// Lista de invitaciones asociadass a una aplicación
    /// </summary>
    [JsonIgnore]
    public IEnumerable<Invitacion>? Invitaciones { get; set; }

    [JsonIgnore]
    public IEnumerable<PlantillaInvitacion> Plantillas { get; set; }

    [JsonIgnore]
    public IEnumerable<LogoAplicacion> Logotipos { get; set; }


    [JsonIgnore]
    public IEnumerable<Consentimiento> Consentimientos { get; set; }
}
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
