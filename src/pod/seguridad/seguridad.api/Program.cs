using apigenerica.primitivas;
using comunes.primitivas.configuracion.mongo;
using Microsoft.Extensions.Options;

namespace seguridad.api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var configuration = builder.Configuration;

            // Add services to the container.
            builder.Services.AddCors(c =>
            {
                c.AddPolicy("default", p =>
                {
                    p.AllowAnyMethod();
                    p.AllowAnyOrigin();
                    p.AllowAnyHeader();
                });
            });

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddSingleton<IConfigureOptions<ConfiguracionMongo>, ConfigureConfiguracionMongoOptions>();
            builder.Services.AddSingleton<IServicionConfiguracionMongo, ServicioConfiguracionMongoOptions>();

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
            app.UseCors("default");
            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
