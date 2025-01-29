using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace servicio.secretos;

public static class Configuracion
{
    /// <summary>
    /// Adiciona el servicio de  gestor de boveda local
    /// </summary>
    /// <param name="builder"></param>
    public static void AddGestorSecretosLocal(this WebApplicationBuilder? builder)
    {
        if (builder == null)
        {
            return;
        }
        builder.Services.AddTransient<IGestorSecretos, GestorSecretosBovedaLocal>();
    }
}
