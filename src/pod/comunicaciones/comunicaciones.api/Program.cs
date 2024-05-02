
using comunes.interservicio.primitivas;
using comunicaciones.modelo;
using comunicaciones.servicios.email;
using comunicaciones.servicios.whatsapp;
using System.Reflection;

namespace comunicaciones.api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // INcluye los servicios básicos para la API de contaboee
            builder.CreaConfiguracionStandar(Assembly.GetExecutingAssembly());


            builder.Services.Configure<SMTPConfig>(builder.Configuration.GetSection("SMTPConfig"));
            builder.Services.Configure<ConfiguracionAPI>(builder.Configuration.GetSection(nameof(ConfiguracionAPI)));

            builder.Services.AddTransient<IServicioEmail, ServicioEmailSendGrid>();
            builder.Services.AddTransient<IMessageBuilder, JSONMessageBuilder>();
            builder.Services.AddTransient<IServicioWhatsapp, ServicioWhatsapp>();

            var app = builder.Build();

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
}
