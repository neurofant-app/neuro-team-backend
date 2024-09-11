using espaciotrabajo.services;
using Microsoft.EntityFrameworkCore;
namespace espaciotrabajo.api.helpers;

public static class StartupHelpers
{
    ///// <summary>
    ///// Realiza las migraciones en las bases de datos
    ///// </summary>
    ///// <param name="app"></param>
    //public static void DbContextEspacioTrabajoDatabase(this IApplicationBuilder app)
    //{
    //    using (var serviceScope = app.ApplicationServices
    //           .GetRequiredService<IServiceScopeFactory>()
    //           .CreateScope())
    //    {
    //        using (var context = serviceScope.ServiceProvider.GetService<MongoDbContextEspacioTrabajo>())
    //        {
    //            context!.Database.Migrate();
    //        }
    //    }
    //}
}
