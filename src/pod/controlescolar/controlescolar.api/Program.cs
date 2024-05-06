using apigenerica.primitivas;
using comunes.primitivas.configuracion.mongo;
using controlescolar.servicios.dbcontext;
using extensibilidad.metadatos;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace controlescolar.api;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        var configuration = builder.Configuration;

        // Add services to the container.

        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddSingleton<IConfigureOptions<ConfiguracionMongo>, ConfigureConfiguracionMongoOptions>();
        builder.Services.AddSingleton<IServicionConfiguracionMongo, ServicioConfiguracionMongoOptions>();

        // Configure MongoDB DbContext
        builder.Services.AddDbContext<MongoDbContext>(options =>
        {
            var mongoConfig = builder.Services.BuildServiceProvider().GetRequiredService<IOptions<ConfiguracionMongo>>().Value;

            options.UseMongoDB(mongoConfig.ConexionDefault, mongoConfig.ConexionesEntidad.FirstOrDefault(_ => _.Entidad == "campus").Entidad);
        });
        builder.CreaConfiguiracionEntidadGenerica();

        var app = builder.Build();
        // Añadir la extensión para los servicios de API genérica
        app.UseEntidadAPI();
        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();


        app.MapControllers();

        app.Run();
    }

}