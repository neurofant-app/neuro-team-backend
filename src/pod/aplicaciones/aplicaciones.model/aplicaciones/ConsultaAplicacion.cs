using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;

namespace aplicaciones.model;

public class ConsultaAplicacion
{
    /// <summary>
    /// Identificador único de la aplicación
    /// </summary>
    public Guid Id { get; set; }
    // Requerida 
    // [A] [D]

    /// <summary>
    /// Nombre de la aplicación que emite la invitación
    /// </summary>
    public required string Nombre { get; set; }
    // Requerida 200
    // [I] [A] [D]

    /// <summary>
    /// Especifica si la aplicación se encuentra activa, solo es posible emitir notificaciones so lo está
    /// </summary>
    public bool Activa { get; set; }

    /// <summary>
    /// Clave de la aplicación
    /// </summary>
    [BsonElement("k")]
    public required string Clave { get; set; }

    /// <summary>
    /// Lista de Hosts asoviados a la aplciación
    /// </summary>
    [BsonElement("u")]
    public List<string>? Hosts { get; set; }

    /// <summary>
    /// Determina si la configuración es la de default
    /// </summary>
    [BsonElement("d")]
    public bool Default { get; set; } = false;


    public IEnumerable<EntidadPlantillaInvitacion> Plantillas { get; set; } = [];

    public IEnumerable<EntidadLogoAplicacion> Logotipos { get; set; } = [];

    public IEnumerable<EntidadConsentimiento> Consentimientos { get; set; } = [];
}
