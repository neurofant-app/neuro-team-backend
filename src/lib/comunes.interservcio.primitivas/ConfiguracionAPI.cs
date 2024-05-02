namespace comunes.interservicio.primitivas;

/// <summary>
/// Define la configuración de API para las llamadas interservicio
/// </summary>
public class ConfiguracionAPI
{

    /// <summary>
    /// Define la clave bajo la cual se guarda la configuración de la api en ENV o Appsettings
    /// </summary>
    public const string ClaveConfiguracionBase = "ConfiguracionAPI";


    /// <summary>
    /// DEfine la clave por defecto para el endpoint de autenticacion 
    /// </summary>
    public const string ClaveEndpointAuthDefault = "default";


    /// <summary>
    /// Ruta al certificado para las operaciones de crifrado de JWT, SOLO SE UTILZIAN POR EL SERVER DE IDENTITY
    /// </summary>
    public string? EncryptionCertificate { get; set; }

    /// <summary>
    /// Ruta al certificado para las operaciones de firma de JWT, SOLO SE UTILZIAN POR EL SERVER DE IDENTITY
    /// </summary>
    public string? SigningCertificate { get; set; }

    /// <summary>
    /// Especifica si el servidor de indentidad cifra el payload del JWT
    /// </summary>
    public bool JWTCifrado { get; set; }



    /// <summary>
    /// Lista de configuraciones de autenticacion JWT
    /// </summary>
    public List<AutenticacionJWT> AuthConfigJWT { get; set; } = [];
    
    
    /// <summary>
    /// Lista de hosts interservicio
    /// </summary>
    public List<HostInterServicio> Hosts { get; set; } = [];
}
