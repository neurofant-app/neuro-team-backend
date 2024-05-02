using apigenerica.model.reflectores;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace apigenerica.primitivas;

public static class Extensiones
{
    public static void CreaConfiguiracionEntidadGenerica(this WebApplicationBuilder builder)
    {
        // Añadir la extensión para los servicios de API genérica
        builder.Services.AddServiciosEntidadAPI();
        builder.Services.AddTransient<IConfiguracionAPIEntidades, ConfiguracionAPIEntidades>();
        builder.Services.AddTransient<IReflectorEntidadesAPI, ReflectorEntidadAPI>();

    }
}
