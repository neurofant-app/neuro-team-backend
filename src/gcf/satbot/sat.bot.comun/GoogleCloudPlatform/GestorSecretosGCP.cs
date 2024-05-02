
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace sat.bot.comun.GoogleCloudPlatform;

/// <summary>
/// Gestiona los secretos para el acceso al portal de SAT
/// </summary>
public class GestorSecretosGCP(ILogger logger, IOptions<ConfiguracionGCP> configuracion) : IAdministradorSecretos
{
    public Task<byte[]> ObtieneCertificadoPFX(string secretoId)
    {
        // Leer la ultima versión del secreto apuntado por Id en el proyecto definido por proyectoId de la configuracion
        throw new NotImplementedException();
    }

    public Task<string> ObtieneContrasenaCertificadoPFX(string secretoId)
    {
        // Leer la ultima versión del secreto apuntado por Id en el proyecto definido por proyectoId de la configuracion
        throw new NotImplementedException();
    }

    public Task<string> ObtieneContrasenaSAT(string secretoId)
    {
        // Leer la ultima versión del secreto apuntado por Id en el proyecto definido por proyectoId de la configuracion
        throw new NotImplementedException();
    }
}
