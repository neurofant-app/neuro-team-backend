using conversaciones.model;
using Microsoft.EntityFrameworkCore;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;
using MongoDB.EntityFrameworkCore.Extensions;

namespace conversaciones.services.dbcontext;

public class MongoDbContextConversaciones(DbContextOptions<MongoDbContextConversaciones> options): DbContext(options)
{
    public const string NOMBRE_COLECCION_PLANTILLA = "plantilla";
    public DbSet<Plantilla> Plantilla { get; set; }
    public static MongoDbContextConversaciones Create(IMongoDatabase database)
    {
        // Este fragemnto sirve para evitar conflicts de mongo con cambios en el modelo
        var pack = new ConventionPack
            {
                new IgnoreExtraElementsConvention(true)
            };
        ConventionRegistry.Register("Conventions", pack, t => true);

        return new(new DbContextOptionsBuilder<MongoDbContextConversaciones>()
           .UseMongoDB(database.Client, database.DatabaseNamespace.DatabaseName)
           .Options);
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Plantilla>().ToCollection(NOMBRE_COLECCION_PLANTILLA);
    }
}
