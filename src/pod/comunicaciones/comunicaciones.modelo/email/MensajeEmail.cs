namespace comunicaciones.modelo;

public class MensajeEmail
{
    /// <summary>
    /// Nombre del remitente, si se deja en blanco utiliza el nombre por defecto desde la configuración
    /// </summary>
    public string? NombreDe { get; set; }

    /// <summary>
    /// Dirección de correo del remitente, si se deja en blanco utiliza la dirección de coreo por defecto desde la configuración
    /// </summary>
    public string? DireccionDe { get; set; }

    /// <summary>
    /// Nombre del destinatario
    /// </summary>
    public string DireccionPara { get; set; }

    /// <summary>
    /// Dirección de correo del destinatario
    /// </summary>
    public string NombrePara { get; set; }

    /// <summary>
    /// Plantilla para el tema (subject) del email
    /// </summary>
    public string PlantillaTema { get; set; }


    /// <summary>
    /// Plantilla para el cuerpo (body) del email
    /// </summary>    
    public string PlantillaCuerpo { get; set; }
    
    /// <summary>
    /// Datos para llenar la plantilla serializados como un texto JSON
    /// </summary>
    public string JSONData { get; set; }
}
