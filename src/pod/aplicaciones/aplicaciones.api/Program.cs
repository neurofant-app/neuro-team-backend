using apigenerica.primitivas;
using aplicaciones.services.dbContext;
using aplicaciones.services.invitacion;
using aplicaciones.services.proxy;
using aplicaciones.services.proxy.abstractions;
using aplicaciones.services.proxy.implementations;
using comunes.interservicio.primitivas;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace aplicaciones.api;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
       
        // INcluye los servicios básicos para la API de contaboee
        builder.CreaConfiguracionStandar(Assembly.GetExecutingAssembly());

        var connectionString = builder.Configuration.GetConnectionString("contabee-cloud");

        // Add services to the container.
        builder.Services.AddDbContext<DbContextAplicaciones>(options =>
        {
            options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
        });

        builder.CreaConfiguiracionEntidadGenerica();
        builder.Services.AddTransient<IProxyIdentityServices, ProxyIdentityServices>();
        builder.Services.AddTransient<IProxyComunicacionesServices, ProxyComunicacionesServices>();
        builder.Services.AddTransient<IServicioInvitacion, ServicioInvitacion>();

        var app = builder.Build();

        // Realiza la migracion del dbcontext de aplicacions
        app.DbContextAplicacionesUpdateDatabase();

        // Añadir la extensión para los servicios de API genérica
        app.UseEntidadAPI();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseCors(ExtensionesConfiguracionInterservicio.CORS_ANY_ORIGIN_ALL);
        app.UseHttpsRedirection();
        app.UseAuthentication();
        app.UseAuthorization();
        app.MapControllers();
        app.Run();
    }
}
