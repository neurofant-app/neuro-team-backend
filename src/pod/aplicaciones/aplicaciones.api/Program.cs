using apigenerica.primitivas;
using aplicaciones.services.aplicacion;
using aplicaciones.services.invitacion;
using aplicaciones.services.proxy;
using aplicaciones.services.proxy.abstractions;
using aplicaciones.services.proxy.implementations;
using comunes.interservicio.primitivas;
using comunes.primitivas.configuracion.mongo;
using Microsoft.Extensions.Options;
using Quartz;
using System.Reflection;

namespace aplicaciones.api;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.Services.AddQuartz(options =>
        {
            options.UseMicrosoftDependencyInjectionJobFactory();
            options.UseSimpleTypeLoader();
            options.UseInMemoryStore();
        });

        // Register the Quartz.NET service and configure it to block shutdown until jobs are complete.
        builder.Services.AddQuartzHostedService(options => options.WaitForJobsToComplete = true);

        // INcluye los servicios básicos para la API de contaboee
        builder.CreaConfiguracionStandar(Assembly.GetExecutingAssembly());
        builder.Services.Configure<ConfiguracionAPI>(builder.Configuration.GetSection(nameof(ConfiguracionAPI)));
        builder.CreaConfiguiracionEntidadGenerica();
        builder.Services.AddTransient<IProxyIdentityServices, ProxyIdentityServices>();
        builder.Services.AddTransient<IProxyComunicacionesServices, ProxyComunicacionesServices>();
        builder.Services.AddSingleton<IConfigureOptions<ConfiguracionMongo>, ConfigureConfiguracionMongoOptions>();
        builder.Services.AddSingleton<IServicionConfiguracionMongo, ServicioConfiguracionMongoOptions>();
        builder.Services.AddTransient<IServicioAplicacion, ServicioAplicacion>();
        builder.Services.AddTransient<IServicioInvitacion, ServicioEntidadInvitacion>();
        builder.Services.AddTransient<IProxySeguridad, ProxySeguridad>();


        builder.Services.AddHostedService<Worker>();

        var app = builder.Build();
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
