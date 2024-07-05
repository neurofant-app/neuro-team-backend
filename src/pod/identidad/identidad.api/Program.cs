using comunes.interservicio.primitivas;
using contabee.identity.api.helpers;
using contabee.identity.api.models;
using identidad.api.models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver.Core.Configuration;
using MongoDB.Driver;
using OpenIddict.Validation.AspNetCore;
using Quartz;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using static OpenIddict.Abstractions.OpenIddictConstants;


namespace contabee.identity.api;

public class Program
{
    /// <summary>
    /// Inyecta el servicio de Identidad de OpenIdDict
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configuration">Configuracion para ConfiguracionAPI utiliza clave ConfiguracionAPI </param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    private static void InyectaOpenIdDict(IServiceCollection services, ConfigurationManager configuration)
    {

        ConfiguracionAPI configuracionAPI = new();
        configuration.GetSection(ConfiguracionAPI.ClaveConfiguracionBase).Bind(configuracionAPI);

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


    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        IWebHostEnvironment environment = builder.Environment;

        builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
                .AddUserSecrets(Assembly.GetExecutingAssembly(), true)
                .AddEnvironmentVariables();
        var configuration = builder.Configuration;


        ConfiguracionAPI configuracionAPI = new();
        configuration.GetSection("ConfiguracionAPI").Bind(configuracionAPI);

        bool continuar = IdentityHelpers.PreProceso(args, configuracionAPI);
        if (!continuar)
        {
            Console.WriteLine("PreProceso finalizado");
            return;
        }


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


        // Add services to the container.
        //builder.Services.AddControllersWithViews();

        var dbProdiver = configuration["dbtype"];
        switch (dbProdiver)
        {
            case "mysql":
                builder.Services.AddDbContext<ApplicationDbContext>(options =>
                {
                    // Configure the context to use mysql.
                    var connectionString = builder.Configuration.GetConnectionString("identityMySql");
                    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));

                    // Register the entity sets needed by OpenIddict.
                    // Note: use the generic overload if you need
                    // to replace the default OpenIddict entities.
                    options.UseOpenIddict();
                });

                // Register the Identity services.
                builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
                    .AddEntityFrameworkStores<ApplicationDbContext>()
                    .AddDefaultTokenProviders();
                break;
            case "mongo":
                var connectionString = builder.Configuration.GetConnectionString("identityMongo");

                builder.Services.AddIdentity<ApplicationUserMongo, ApplicationRole>()
                    .AddMongoDbStores<ApplicationUserMongo, ApplicationRole, Guid>
                    (
                        connectionString, "identityMongo"
                    );
                break;
            case "default":
                break;
        }

        // OpenIddict offers native integration with Quartz.NET to perform scheduled tasks
        // (like pruning orphaned authorizations/tokens from the database) at regular intervals.
        builder.Services.AddQuartz(options =>
        {
            options.UseMicrosoftDependencyInjectionJobFactory();
            options.UseSimpleTypeLoader();
            options.UseInMemoryStore();
        });

        // Register the Quartz.NET service and configure it to block shutdown until jobs are complete.
        builder.Services.AddQuartzHostedService(options => options.WaitForJobsToComplete = true);

        builder.Services.AddOpenIddict()

            // Register the OpenIddict core components.
            .AddCore(options =>
            {
                switch (dbProdiver)
                {
                    case "mysql":
                        // Configure OpenIddict to use the Entity Framework Core stores and models.
                        // Note: call ReplaceDefaultEntities() to replace the default OpenIddict entities.
                        options.UseEntityFrameworkCore()
                               .UseDbContext<ApplicationDbContext>();
                        break;
                    case "mongo":
                        var connectionString = builder.Configuration.GetConnectionString("identityMongo");

                        options.UseMongoDb()
                        .UseDatabase(new MongoClient(connectionString).GetDatabase("identityMongo"));
                        break;
                    case "default":
                        break;
                }

                // Enable Quartz.NET integration.
                options.UseQuartz();
            })

            // Register the OpenIddict server components.
            .AddServer(options =>
            {
                // Enable the token endpoint.
                options.SetAuthorizationEndpointUris("connect/authorize")
                     .SetLogoutEndpointUris("connect/logout")
                     .SetIntrospectionEndpointUris("connect/introspect")
                     .SetTokenEndpointUris("connect/token")
                     .SetUserinfoEndpointUris("connect/userinfo")
                     .SetVerificationEndpointUris("connect/verify");

                options.RegisterScopes(Scopes.Email, Scopes.Profile, Scopes.Roles);

                // Enable the password and the refresh token flows.
                options.AllowPasswordFlow()
                       .AllowRefreshTokenFlow()
                       .AllowClientCredentialsFlow();

                // Accept anonymous clients (i.e clients that don't send a client_id).
                options.AcceptAnonymousClients();

                // REgistra credenciales  de  cifrado para el JWT.
                if (!string.IsNullOrEmpty(configuracionAPI.EncryptionCertificate) && File.Exists(configuracionAPI.EncryptionCertificate))
                {
                    Console.WriteLine($"Utilizando certificado de cifrado {configuracionAPI.EncryptionCertificate}");
                    X509Certificate2 ec = new(configuracionAPI.EncryptionCertificate);
                    options.AddEncryptionCertificate(ec);
                }
                else
                {
                    Console.WriteLine("Utilizando certificado de desarrollo para cifrado");
                    options.AddDevelopmentEncryptionCertificate();
                }

                // REgistra credenciales de firma para el JWT
                if (!string.IsNullOrEmpty(configuracionAPI.SigningCertificate) && File.Exists(configuracionAPI.SigningCertificate))
                {
                    Console.WriteLine($"Utilizando certificado para firma {configuracionAPI.SigningCertificate}");
                    X509Certificate2 sc = new(configuracionAPI.SigningCertificate);
                    options.AddSigningCertificate(sc);
                }
                else
                {
                    Console.WriteLine("Utilizando certificado de desarrollo para firma");
                    options.AddDevelopmentSigningCertificate();
                }


                // Evita que se encripte el payload del token
                if (!configuracionAPI.JWTCifrado)
                {
                    options.DisableAccessTokenEncryption();
                }


                //#endif

                // Register the ASP.NET Core host and configure the ASP.NET Core-specific options.
                options.UseAspNetCore()
                       .EnableAuthorizationEndpointPassthrough()
                       .EnableLogoutEndpointPassthrough()
                       .EnableTokenEndpointPassthrough()
                       .EnableUserinfoEndpointPassthrough()
                       .EnableStatusCodePagesIntegration();
            });


        InyectaOpenIdDict(builder.Services, builder.Configuration);

        // Register the worker responsible for seeding the database.
        // Note: in a real world application, this step should be part of a setup script.
        builder.Services.AddHostedService<Worker>();

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();

        app.UseRouting();
        app.UseCors("default");
        app.UseHttpsRedirection();
        app.UseAuthentication();
        app.UseAuthorization();


        app.MapControllers();
        app.MapDefaultControllerRoute();


        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }



        app.Run();
    }
}