using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using servicio.almacenamiento.configuraciones;

namespace servicio.almacenamiento.servicioconfiguracion;

/// <summary>
/// Configuracion de repositorio basada en appsettings
/// </summary>
public class RepositorioConfiguracionAlmacenamientoAppSettings : IRepositorioConfiguracionAlmacenamiento
{

    private readonly IConfiguration _configuration;
    private readonly List<ConfiguracionProveedor> proveedores = [];
    public RepositorioConfiguracionAlmacenamientoAppSettings(IOptions<ConfiguracionRepositorioProveedorAlmacenamiento> options, IConfiguration configuration)
    {
        _configuration = configuration;

        IConfigurationSection section = _configuration.GetSection(options.Value.Coleccion!);
        IEnumerable<IConfigurationSection> usersArray = section.GetChildren();

        foreach(var item in usersArray)
        {

            var c = new ConfiguracionProveedor()
            {
                PayloadCifrado = bool.Parse(item["PayloadCifrado"]!),
                Activa = bool.Parse(item["Activa"]!),
                ConfiguracionJSON = item["ConfiguracionJSON"]!,
                ServicioId = item["ServicioId"],
                Servicio = item["Servicio"]!,
                Default = bool.Parse(item["Default"]!),
                Tipo = (TipoProveedorAlmacenamiento)Enum.Parse(typeof(TipoProveedorAlmacenamiento), item["Tipo"]!)

            };

            proveedores.Add(c);
        }
    }

#pragma warning disable CS8602 // Dereference of a possibly null reference.
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
    public Task<ConfiguracionProveedor?> ObtieneConfiguracion(string servicio, string? servicioId)
    {
        ConfiguracionProveedor proveedor;
        if(!string.IsNullOrEmpty(servicioId))
        {

            proveedor = proveedores.FirstOrDefault(x => x.Activa && x.Servicio.Equals(servicio, StringComparison.InvariantCultureIgnoreCase)
                && x.ServicioId.Equals(servicioId, StringComparison.InvariantCultureIgnoreCase));
        } else
        {

            proveedor = proveedores.FirstOrDefault(x => x.Activa && x.Servicio.Equals(servicio, StringComparison.InvariantCultureIgnoreCase));

        }
        return  Task.FromResult( proveedor);
    }
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
#pragma warning restore CS8602 // Dereference of a possibly null reference.
}
