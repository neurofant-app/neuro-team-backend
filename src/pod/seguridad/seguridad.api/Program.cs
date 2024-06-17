using apigenerica.primitivas;
using apigenerica.primitivas.aplicacion;
using apigenerica.primitivas.seguridad;
using comunes.interservicio.primitivas;
using comunes.interservicio.primitivas.seguridad;
using comunes.primitivas.configuracion.mongo;
using Microsoft.Extensions.Options;
using seguridad.servicios;

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
            builder.Services.Configure<ConfiguracionAPI>(builder.Configuration.GetSection(nameof(ConfiguracionAPI)));
            builder.Services.AddSingleton<IConfigureOptions<ConfiguracionMongo>, ConfigureConfiguracionMongoOptions>();
            builder.Services.AddSingleton<IServicionConfiguracionMongo, ServicioConfiguracionMongoOptions>();
            builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            builder.Services.AddSingleton<IServicioInstanciaAplicacion, ServicioInstanciaAplicacion>();
            builder.Services.AddSingleton<IServicioAplicacion, ServicioAplicacion>();
            builder.Services.AddSingleton<IProveedorAplicaciones, ConfiguracionSeguridad>();
            builder.Services.AddSingleton<ICacheSeguridad, CacheSeguridad>();
            builder.Services.AddSingleton<IProxySeguridad, ProxySeguridad>();
            builder.Services.AddTransient<IServicioAutenticacionJWT, ServicioAuthInterprocesoJWT>();
            builder.Services.AddTransient<ICacheAtributos, CacheAtributos>();
            builder.Services.AddHttpClient();
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
