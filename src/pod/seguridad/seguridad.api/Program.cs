using apigenerica.primitivas;
using apigenerica.primitivas.aplicacion;
using apigenerica.primitivas.seguridad;
using aplicaciones.api;
using comunes.interservicio.primitivas;
using comunes.interservicio.primitivas.seguridad;
using comunes.primitivas.configuracion.mongo;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MongoDB.Driver.Core.Configuration;
using seguridad.servicios;
using seguridad.servicios.mysql;

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

            var connectionStringMySql = builder.Configuration.GetConnectionString("neurofant-cloud");
            builder.Services.AddDbContext<DBContextMySql>(options =>
            {
                options.UseMySql(connectionStringMySql, ServerVersion.AutoDetect(connectionStringMySql));

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
            builder.Services.AddSingleton<ICacheSeguridad, CacheSeguridad>();
            builder.Services.AddSingleton<IProxySeguridad, ProxySeguridad>();
            builder.Services.AddTransient<IServicioAplicacion, ServicioAplicacion>();
            builder.Services.AddTransient<IServicioAplicacionMysql, ServicioAplicacionMysql>();
            builder.Services.AddTransient<IServicioInstanciaAplicacion, ServicioInstanciaAplicacion>();
            builder.Services.AddTransient<IServicioInstanciaAplicacionMysql, ServicioInstanciaAplicacionMysql>();
            builder.Services.AddTransient<IProveedorAplicaciones, ConfiguracionSeguridad>();
            builder.Services.AddTransient<ICacheSeguridad, CacheSeguridad>();
            builder.Services.AddTransient<IProxySeguridad, ProxySeguridad>();
            builder.Services.AddTransient<IServicioAutenticacionJWT, ServicioAuthInterprocesoJWT>();
            builder.Services.AddTransient<ICacheAtributos, CacheAtributos>();
            builder.Services.AddHttpClient();
            builder.CreaConfiguiracionEntidadGenerica();

            var app = builder.Build();
            app.DBContextMySqlUpdateDatabase();
            // A�adir la extensi�n para los servicios de API gen�rica
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
