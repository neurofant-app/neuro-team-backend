using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace comunes.primitivas.configuracion.mongo;

/// <summary>
/// OBtiene la configuración para mongo en base al nombre de la entidad
/// </summary>
/// <param name="logger"></param>
/// <param name="options"></param>
public class ServicioConfiguracionMongoOptions(ILogger<ServicioConfiguracionMongoOptions> logger, IOptions<ConfiguracionMongo> options) : IServicionConfiguracionMongo
{
    private readonly ConfiguracionMongo configuracionMongo = options.Value;
    private readonly ILogger logger = logger;

    /// <summary>
    /// OBtine la cadena de conexión por defecto
    /// </summary>
    /// <returns></returns>
    public string? ConexionDefault()
    {
        if(configuracionMongo != null)
        {
            return configuracionMongo.ConexionDefault;
        }
        return null;
    }

    /// <summary>
    /// BUsca una configuración en base al nonbre de una entidad
    /// </summary>
    /// <param name="entidad"></param>
    /// <returns></returns>
    public ConexionEntidad? ConexionEntidad(string entidad)
    {
        if (configuracionMongo != null && configuracionMongo.ConexionesEntidad != null)
        {
            return configuracionMongo.ConexionesEntidad.FirstOrDefault(_ => _.Entidad.Equals(entidad, StringComparison.InvariantCultureIgnoreCase));
        }
        return null;
    }
}
