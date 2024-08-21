using aplicaciones.model;
using Microsoft.EntityFrameworkCore;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;
using MongoDB.EntityFrameworkCore.Extensions;

namespace aplicaciones.services.dbcontext;

public class MongoDbContextAplicaciones(DbContextOptions<MongoDbContextAplicaciones> options) : DbContext(options) 
{
    public const string NOMBRE_COLECCION_APLICACION = "aplicacion";
    public const string NOMBRE_COLECCION_INVITACION = "invitacion";
    public const string NOMBRE_COLECCION_CONSENTIMIENTO = "consentimiento";
    public const string NOMBRE_COLECCION_LOGOAPLICACION = "logoAplicacion";
    public const string NOMBRE_COLECCION_PLANTILLaAPLICACION = "plantillaAplicacion";
    public DbSet<EntidadAplicacion> Aplicaciones { get; set; }
    public DbSet<EntidadInvitacion> Invitaciones { get; set; }
    public DbSet<EntidadConsentimiento> Consentimientos { get; set; }
    public DbSet<EntidadLogoAplicacion> LogoAplicaciones { get; set; }
    public DbSet<EntidadPlantillaInvitacion> PlantillaInvitaciones { get; set; }


    public static MongoDbContextAplicaciones Create(IMongoDatabase database)
    {
        // Este fragemnto sirve para evitar conflicts de mongo con cambios en el modelo
        var pack = new ConventionPack
            {
                new IgnoreExtraElementsConvention(true)
            };
        ConventionRegistry.Register("Conventions", pack, t => true);

        return new(new DbContextOptionsBuilder<MongoDbContextAplicaciones>()
           .UseMongoDB(database.Client, database.DatabaseNamespace.DatabaseName)
           .Options);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<EntidadAplicacion>().ToCollection(NOMBRE_COLECCION_APLICACION);
        modelBuilder.Entity<EntidadInvitacion>().ToCollection(NOMBRE_COLECCION_INVITACION);
        modelBuilder.Entity<EntidadConsentimiento>().ToCollection(NOMBRE_COLECCION_CONSENTIMIENTO);
        modelBuilder.Entity<EntidadLogoAplicacion>().ToCollection(NOMBRE_COLECCION_LOGOAPLICACION);
        modelBuilder.Entity<EntidadPlantillaInvitacion>().ToCollection(NOMBRE_COLECCION_PLANTILLaAPLICACION);
    }

}
