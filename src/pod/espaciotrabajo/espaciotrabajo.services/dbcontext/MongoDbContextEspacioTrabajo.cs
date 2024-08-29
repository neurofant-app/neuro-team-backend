using espaciotrabajo.model.espaciotrabajo;
using Microsoft.EntityFrameworkCore;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;
using MongoDB.EntityFrameworkCore.Extensions;

namespace espaciotrabajo.services;

public class MongoDbContextEspacioTrabajo(DbContextOptions<MongoDbContextEspacioTrabajo> options) : DbContext(options)
{
    public const string NOMBRE_COLECCION_ESPACIOTRABAJO = "espaciotrabajo";
    public DbSet<EspacioTrabajo> EspaciosTrabajo { get; set; }

    public static MongoDbContextEspacioTrabajo Create(IMongoDatabase database)
    {
        var pack = new ConventionPack
        {
            new IgnoreExtraElementsConvention(true),
        };

        ConventionRegistry.Register("Conventions", pack, t => true);
        return new(new DbContextOptionsBuilder<MongoDbContextEspacioTrabajo>()
            .UseMongoDB(database.Client, database.DatabaseNamespace.DatabaseName)
            .Options);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<EspacioTrabajo>().ToCollection(NOMBRE_COLECCION_ESPACIOTRABAJO);
    }
}
