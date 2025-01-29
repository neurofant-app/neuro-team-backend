using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using servicio.almacenamiento.configuraciones;
using servicio.almacenamiento.servicioconfiguracion;


namespace servicio.almacenamiento;

public static class Configuracion
{
    /// <summary>
    /// Adiciona la fabrica de almacenamiento basada en mongo
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="key">Clave de la cadena de conexión en el appsettings</param>
    public static void AddFabricaAlmacenamientoMongo(this WebApplicationBuilder? builder, string key = "ConfigFabricaAlmacenamiento")
    {
        if(builder != null)
        {
            builder.Services.Configure<ConfiguracionRepositorioProveedorAlmacenamiento>(builder.Configuration.GetSection(key));
            builder.Services.AddTransient<IRepositorioConfiguracionAlmacenamiento, RepositorioConfiguracionAlmacenamientoMongo>();
            builder.Services.AddTransient<IFabricaProveedorAlmacenamiento, FabricaProveedorAlmacenamiento>();

        }
    }


    /// <summary>
    /// Adiciona la fabrica de almacenamiento basada en mongo
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="key">Clave de la cadena de conexión en el appsettings</param>
    public static void AddFabricaAlmacenamientoAppsettings(this WebApplicationBuilder? builder, string key = "ConfigFabricaAlmacenamiento")
    {
        if (builder != null)
        {
            builder.Services.Configure<ConfiguracionRepositorioProveedorAlmacenamiento>(builder.Configuration.GetSection(key));
            builder.Services.AddTransient<IRepositorioConfiguracionAlmacenamiento, RepositorioConfiguracionAlmacenamientoAppSettings>();
            builder.Services.AddTransient<IFabricaProveedorAlmacenamiento, FabricaProveedorAlmacenamiento>();
        }
    }

}
