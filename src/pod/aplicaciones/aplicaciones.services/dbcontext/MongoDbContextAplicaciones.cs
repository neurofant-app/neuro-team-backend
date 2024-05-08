using aplicaciones.model;
using Microsoft.EntityFrameworkCore;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;
using MongoDB.EntityFrameworkCore.Extensions;

namespace aplicaciones.services.dbcontext;

public class MongoDbContextAplicaciones(DbContextOptions<MongoDbContextAplicaciones> options) : DbContext(options) 
{
    public const string NOMBRE_COLECCION_APLICACION = "aplicacion";
    public const string NOMBRE_COLECCION_CONSENTIMIENTO = "consentimiento";
    public const string NOMBRE_COLECCION_LOGOAPLICACION = "logoAplicacion";
    public const string NOMBRE_COLECCION_PLANTILLaAPLICACION = "plantillaAplicacion";
    public DbSet<Aplicacion> Aplicaciones { get; set; }
    public DbSet<Consentimiento> Consentimientos { get; set; }
    public DbSet<LogoAplicacion> LogoAplicaciones { get; set; }
    public DbSet<PlantillaInvitacion> PlantillaInvitaciones { get; set; }


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
        modelBuilder.Entity<Aplicacion>().ToCollection(NOMBRE_COLECCION_APLICACION);
        modelBuilder.Entity<Consentimiento>().ToCollection(NOMBRE_COLECCION_CONSENTIMIENTO);
        modelBuilder.Entity<LogoAplicacion>().ToCollection(NOMBRE_COLECCION_LOGOAPLICACION);
        modelBuilder.Entity<PlantillaInvitacion>().ToCollection(NOMBRE_COLECCION_PLANTILLaAPLICACION);

    }

}
