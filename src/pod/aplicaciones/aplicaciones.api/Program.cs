using apigenerica.primitivas;
using aplicaciones.services.aplicacion;
using aplicaciones.services.dbcontext;
using aplicaciones.services.invitacion;
using aplicaciones.services.proxy;
using aplicaciones.services.proxy.abstractions;
using aplicaciones.services.proxy.implementations;
using comunes.interservicio.primitivas;
using comunes.primitivas.configuracion.mongo;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Reflection;

namespace aplicaciones.api;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
       
        // INcluye los servicios básicos para la API de contaboee
        builder.CreaConfiguracionStandar(Assembly.GetExecutingAssembly());

        builder.CreaConfiguiracionEntidadGenerica();
        builder.Services.AddTransient<IProxyIdentityServices, ProxyIdentityServices>();
        builder.Services.AddTransient<IProxyComunicacionesServices, ProxyComunicacionesServices>();
        builder.Services.AddSingleton<IConfigureOptions<ConfiguracionMongo>, ConfigureConfiguracionMongoOptions>();
        builder.Services.AddSingleton<IServicionConfiguracionMongo, ServicioConfiguracionMongoOptions>();
        builder.Services.AddTransient<IServicioAplicacion, ServicioAplicacion>();
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
