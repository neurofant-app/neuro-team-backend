namespace comunes.interservicio.primitivas;

/// <summary>
/// Defina una configuración de autenticacion para los Host de interservicio
/// </summary>
public class AutenticacionJWT
{
    /// <summary>
    /// Clave única de la configuración
    /// </summary>
    public required string Clave { get; set; }

    /// <summary>
    /// URl del servicio de genración de tokens
    /// </summary>
    public required string UrlToken { get; set; }

    /// <summary>
    /// Identificador del cliente de OpenId utilziado para autenticar
    /// </summary>
    public required string ClientId { get; set; }

    /// <summary>
    /// Scope del cliente de OpenId utilziado para autenticar
    /// </summary>
    public required string Scope { get; set; }

    /// <summary>
    /// Secreto del cliente de OpenId utilziado para autenticar
    /// </summary>
    public required string Secret { get; set; }

    /// <summary>
    /// Ruta al certificado de cifrado del servidor de Identidad
    /// </summary>
    public string EncryptionCertificate { get; set; }

}
