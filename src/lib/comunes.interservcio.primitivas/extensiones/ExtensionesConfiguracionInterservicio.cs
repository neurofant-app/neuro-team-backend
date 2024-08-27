using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OpenIddict.Validation.AspNetCore;
using Serilog;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;

namespace comunes.interservicio.primitivas;

public static class ExtensionesConfiguracionInterservicio
{
    /// <summary>
    /// Nombre de la configuracion CORS para any origin any method any header
    /// </summary>
    public const string CORS_ANY_ORIGIN_ALL = "anyall";

    /// <summary>
    /// Obtiene una caonfiguracion de OpenId para JWT 
    /// </summary>
    /// <param name="configuracion"></param>
    /// <param name="clave"></param>
    /// <returns></returns>
    public static AutenticacionJWT? ObtieneConfiguracionJWT(this ConfiguracionAPI configuracion, string clave)
    {
        return configuracion.AuthConfigJWT
            .FirstOrDefault(_ => _.Clave.Equals(clave, StringComparison.InvariantCultureIgnoreCase));
    }

    /// <summary>
    /// Obtiene los datos de configuracion de un host de interservicio
    /// </summary>
    /// <param name="configuracion"></param>
    /// <param name="clave"></param>
    /// <returns></returns>
    public static HostInterServicio? ObtieneHost(this ConfiguracionAPI configuracion, string clave)
    {
        return configuracion.Hosts
            .FirstOrDefault(_ => _.Clave.Equals(clave, StringComparison.InvariantCultureIgnoreCase));
    }

    /// <summary>
    /// Inyecta el servicio de Identidad de OpenIdDict
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configuration">Configuracion para ConfiguracionAPI utiliza clave ConfiguracionAPI </param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public static void InyectaOpenIdDict(this IServiceCollection services, ConfigurationManager configuration)
    {

        ConfiguracionAPI configuracionAPI = new();
        configuration.GetSection(ConfiguracionAPI.ClaveConfiguracionBase).Bind(configuracionAPI);

        var demo = File.Exists(configuracionAPI.SigningCertificate);

        services.AddOpenIddict()
            .AddValidation(options =>
            {
                var auth = configuracionAPI.AuthConfigJWT.FirstOrDefault(x => x.Clave == ConfiguracionAPI.ClaveEndpointAuthDefault);
                if (auth != null)
                {
                    options.SetIssuer(auth.UrlToken);
                    // Añade la clave de cifrado si es necesaria
                    if (configuracionAPI.JWTCifrado)
                    {
                        X509Certificate2 ec = new(auth.EncryptionCertificate);
                        options.AddEncryptionCertificate(ec);

                    }
                    options.UseSystemNetHttp();
                    options.UseAspNetCore();

                }
                else
                {
                    throw new Exception("Configuración de auntenticación no válida");
                }
            });

        services.AddAuthentication(OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme);
        services.AddAuthorization();
    }


    /// <summary>
    /// INicializa la configuracion estándar de Seriog
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="configuration"></param>
    public static void InicializaSerilog(this WebApplicationBuilder builder, ConfigurationManager configuration)
    {
        var logger = new LoggerConfiguration()
            .ReadFrom.Configuration(configuration)
            .Enrich.FromLogContext()
            .CreateLogger();

        builder.Logging.ClearProviders();
        builder.Logging.AddConsole();
        builder.Logging.AddSerilog(logger);
    }


    /// <summary>
    /// DEfine una configuracion por defecto que permito el acceso sin erstricciones
    /// </summary>
    /// <param name="builder"></param>
    public static void CORSAnyAll(this WebApplicationBuilder builder) {
        builder.Services.AddCors(options =>
        {
            options.AddPolicy(CORS_ANY_ORIGIN_ALL,
                builder =>
                {
                    builder.AllowAnyOrigin()
                           .AllowAnyHeader()
                           .AllowAnyMethod();
                });
        });
    }

    /// <summary>
    /// Adiciona la configuracion estandar
    ///     Configuracion desde appsetting por entorni
    ///     Inyeccion IOptions de ConfiguracionAPI
    ///     CORS ANY ALL
    ///     Serilog v[ia configuracion
    ///     Validacion standar JWT OpenIdDict
    ///     CAche en memoria
    ///     Servicio de autenticacioón interproceso JWT
    ///     Servicio HTTPClient
    ///     AddControllers
    ///     AddEndpointsApiExplorer
    ///     AddSwaggerGen
    /// </summary>
    /// <param name="builder"></param>
    public static void CreaConfiguracionStandar(this WebApplicationBuilder builder, Assembly rootAssembly)
    {
        IWebHostEnvironment environment = builder.Environment;

        builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                            .AddJsonFile($"appsettings.{environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
                            .AddUserSecrets(rootAssembly, true)
                            .AddEnvironmentVariables();

        builder.Services.Configure<ConfiguracionAPI>(builder.Configuration.GetSection(ConfiguracionAPI.ClaveConfiguracionBase));
        builder.CORSAnyAll();
        builder.InicializaSerilog(builder.Configuration);
        builder.Services.InyectaOpenIdDict(builder.Configuration);
        builder.Services.AddDistributedMemoryCache();
        builder.Services.AddTransient<IServicioAutenticacionJWT, ServicioAuthInterprocesoJWT>();
        builder.Services.AddHttpClient();
        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
    }
}


