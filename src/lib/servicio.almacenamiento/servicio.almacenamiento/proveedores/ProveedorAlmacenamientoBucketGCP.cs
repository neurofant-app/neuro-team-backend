using FluentStorage;
using Microsoft.Extensions.Logging;
using servicio.almacenamiento.configuraciones;

namespace servicio.almacenamiento.proveedores;

/// <summary>
/// Proveedor de almacenamiento local
/// </summary>
public class ProveedorAlmacenamientoBucketGCP : ProveedorAlmacenamientoBase
{
    private ConfiguracionBucketGCP _config;
    
    public ProveedorAlmacenamientoBucketGCP(ILogger logger, ConfiguracionBucketGCP config): 
        base(logger)
    {
        _config = config;
    }
   
}
