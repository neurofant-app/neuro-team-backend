
using Microsoft.EntityFrameworkCore;
using seguridad.servicios.mysql;

namespace aplicaciones.api;

public static class StartupHelpers
{
    /// <summary>
    /// Realiza las migraciones en las bases de datos
    /// </summary>
    /// <param name="app"></param>
    public static void DBContextMySqlUpdateDatabase(this IApplicationBuilder app)
    {
        using (var serviceScope = app.ApplicationServices
            .GetRequiredService<IServiceScopeFactory>()
            .CreateScope())
        {
            using (var context = serviceScope.ServiceProvider.GetService<DBContextMySql>())
            {
                context!.Database.Migrate();
            }
        }
    }
}
