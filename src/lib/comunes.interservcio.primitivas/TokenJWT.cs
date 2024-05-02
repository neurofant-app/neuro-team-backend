namespace comunes.interservicio.primitivas;

/// <summary>
/// Resuesta del servido de autenticación al obtener un token de JWT
/// </summary>
public class TokenJWT
{
    /// <summary>
    /// Token de acceso generado
    /// </summary>
    public string access_token { get; set; }
    /// <summary>
    /// Tipo de auteticacion
    /// </summary>
    public string token_type { get; set; }
    /// <summary>
    /// Tiempo en que expira el token
    /// </summary>
    public int expires_in { get; set; }
    
}
