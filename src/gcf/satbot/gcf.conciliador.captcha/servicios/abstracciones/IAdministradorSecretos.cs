using System.Threading.Tasks;

namespace gcf.conciliador.captcha.servicios;

/// <summary>
/// Accede a los secretos de la plataforma y devuelve su contenido
/// </summary>
public interface IAdministradorSecretos
{
    Task<string> ObtieneContrasenaCertificadoPFX(string secretoId);
    Task<byte[]> ObtieneCertificadoPFX(string secretoId);
}
