using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using servicio.almacenamiento.configuraciones;
using servicio.almacenamiento.proveedores;
using servicio.almacenamiento.servicioconfiguracion;
using servicio.secretos;

namespace servicio.almacenamiento;

/// <summary>
/// Servicio de fábrica de proveedores de almacenamiento
/// </summary>
/// <param name="logger"></param>
/// <param name="configuracionAlmacenamiento"></param>
/// <param name="configuration"></param>
/// <param name="gestorSecretos"></param>
public class FabricaProveedorAlmacenamiento(ILogger<FabricaProveedorAlmacenamiento> logger, 
                                            IRepositorioConfiguracionAlmacenamiento configuracionAlmacenamiento,
                                            IConfiguration configuration,
                                            IGestorSecretos gestorSecretos) : IFabricaProveedorAlmacenamiento
{
    private readonly IGestorSecretos _gestorSecretos = gestorSecretos;
    private readonly ILogger _logger = logger;   
    private readonly IConfiguration _configuration = configuration;
    private readonly IRepositorioConfiguracionAlmacenamiento _configAlmacenamiento = configuracionAlmacenamiento;
    
    /// <summary>
    /// Obtiene un proveedor de almacenamiento para un servicio
    /// </summary>
    /// <param name="servicio"></param>
    /// <param name="servicioId"></param>
    /// <returns></returns>
    public async Task<IProveedorAlmacenamiento?> ObtieneProveedor(string servicio, string? servicioId)
    {
        var configuracion = await _configAlmacenamiento.ObtieneConfiguracion(servicio, servicioId);
        IProveedorAlmacenamiento? proveedor = null;
        if(configuracion != null)
        {
            switch (configuracion.Tipo) {
                case TipoProveedorAlmacenamiento.BucketGCP:
                    proveedor = ObtieneProveedorBucketGCP(configuracion);
                    break;

                case TipoProveedorAlmacenamiento.FilesystemLocal:
                    proveedor = ObtieneProveedorFilesysten(configuracion);
                    break;
            }

        }
        return proveedor;
    }

    /// <summary>
    /// Genera un proveedor de almacenamiento local
    /// </summary>
    /// <param name="config"></param>
    /// <returns></returns>
    private ProveedorAlmacenamientoFilesystemLocal? ObtieneProveedorFilesysten(ConfiguracionProveedor config)
    {
        var configuracion = System.Text.Json.JsonSerializer.Deserialize<ConfiguracionFilesystemLocal>(config.ConfiguracionJSON!);
        if(configuracion != null)
        {
            return new ProveedorAlmacenamientoFilesystemLocal(_logger, configuracion);
        }
        return null;
    }

    /// <summary>
    /// Genera un proveedor de almacenamiento de para un bucket de GCP
    /// </summary>
    /// <param name="config"></param>
    /// <returns></returns>
    private ProveedorAlmacenamientoBucketGCP? ObtieneProveedorBucketGCP(ConfiguracionProveedor config)
    {
        var configuracion = System.Text.Json.JsonSerializer.Deserialize<ConfiguracionBucketGCP>(config.ConfiguracionJSON!);
        if(configuracion != null)
        {
            return new ProveedorAlmacenamientoBucketGCP(_logger, configuracion);
        }
        return null;
    }
}
