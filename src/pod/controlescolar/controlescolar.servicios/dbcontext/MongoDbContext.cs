using controlescolar.modelo.campi;
using Microsoft.EntityFrameworkCore;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;
using MongoDB.EntityFrameworkCore.Extensions;

namespace controlescolar.servicios.dbcontext;

public class MongoDbContext(DbContextOptions<MongoDbContext> options) : DbContext(options)
{
    public const string NOMBRE_COLECCION_CAMPUS = "campus";

    public DbSet<EntidadCampus> EntidadCampi { get; set; }

    public static MongoDbContext Create(IMongoDatabase database)
    {
        // Este fragemnto sirve para evitar conflicts de mongo con cambios en el modelo
        var pack = new ConventionPack
            {
                new IgnoreExtraElementsConvention(true)
            };
        ConventionRegistry.Register("Conventions", pack, t => true);

        return new(new DbContextOptionsBuilder<MongoDbContext>()
           .UseMongoDB(database.Client, database.DatabaseNamespace.DatabaseName)
           .Options);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<EntidadCampus>().ToCollection(NOMBRE_COLECCION_CAMPUS);
    }

}
