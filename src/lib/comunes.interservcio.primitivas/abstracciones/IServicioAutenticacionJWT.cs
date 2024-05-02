namespace comunes.interservicio.primitivas;

/// <summary>
/// Servico de autneticación
/// </summary>
public interface IServicioAutenticacionJWT
{
    /// <summary>
    /// Obtiene un token interproceso a aprtir de la configuración utilizando la clave de Autenticacion JWT
    /// </summary>
    /// <param name="claveConfiguracion"></param>
    /// <returns></returns>
    Task<TokenJWT?> TokenInterproceso(string claveConfiguracion);

    Task<TokenJWT?> GetToken( string user, string password, string claveConfiguracion = "auth_default");
}
