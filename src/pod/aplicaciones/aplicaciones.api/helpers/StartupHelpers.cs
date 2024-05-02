using aplicaciones.services.dbContext;
using Microsoft.EntityFrameworkCore;

namespace aplicaciones.api;

public static class StartupHelpers
{
    /// <summary>
    /// Realiza las migraciones en las bases de datos
    /// </summary>
    /// <param name="app"></param>
    public static void DbContextAplicacionesUpdateDatabase(this IApplicationBuilder app)
    {
        using (var serviceScope = app.ApplicationServices
            .GetRequiredService<IServiceScopeFactory>()
            .CreateScope())
        {
            using (var context = serviceScope.ServiceProvider.GetService<DbContextAplicaciones>())
            {
                context!.Database.Migrate();
            }
        }
    }
}
