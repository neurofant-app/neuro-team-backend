using apigenerica.primitivas;
using apigenerica.primitivas.aplicacion;
using apigenerica.primitivas.seguridad;
using aprendizaje.api.seguridad;
using comunes.interservicio.primitivas;
using comunes.interservicio.primitivas.seguridad;
using comunes.primitivas.configuracion.mongo;
using Microsoft.Extensions.Options;
using System.Reflection;
using servicio.almacenamiento;

namespace controlescolar.api;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // INcluye los servicios b�sicos para la API de contaboee
        builder.CreaConfiguracionStandar(Assembly.GetExecutingAssembly());
        builder.Services.Configure<ConfiguracionAPI>(builder.Configuration.GetSection(nameof(ConfiguracionAPI)));
        builder.CreaConfiguiracionEntidadGenerica();
        builder.Services.AddSingleton<IConfigureOptions<ConfiguracionMongo>, ConfigureConfiguracionMongoOptions>();
        builder.Services.AddSingleton<IServicionConfiguracionMongo, ServicioConfiguracionMongoOptions>();
        builder.Services.AddSingleton<IProveedorAplicaciones, ConfiguracionSeguridad>();
        builder.Services.AddSingleton<ICacheSeguridad, CacheSeguridad>();
        builder.Services.AddSingleton<IProxySeguridad, ProxySeguridad>();
        builder.Services.AddTransient<IServicioAutenticacionJWT, ServicioAuthInterprocesoJWT>();
        builder.Services.AddTransient<ICacheAtributos, CacheAtributos>();
        
        builder.AddFabricaAlmacenamientoAppsettings();
        
        builder.Services.AddHttpClient();

        var app = builder.Build();

        // A�adir la extensi�n para los servicios de API gen�rica
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