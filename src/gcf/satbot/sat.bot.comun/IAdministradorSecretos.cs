namespace sat.bot.comun;

/// <summary>
/// Accede a los secretos de la plataforma y devuelve su contenido
/// </summary>
public interface IAdministradorSecretos
{
    Task<string> ObtieneContrasenaSAT(string secretoId);
    Task<string> ObtieneContrasenaCertificadoPFX(string secretoId);
    Task<byte[]> ObtieneCertificadoPFX(string secretoId);
}
