using creador.contenido.model;
using Microsoft.EntityFrameworkCore;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;
using MongoDB.EntityFrameworkCore.Extensions;

namespace creador.contenido.services.dbcontext;

public class MongoDbContextCreadorContenido(DbContextOptions<MongoDbContextCreadorContenido> options) : DbContext(options)
{
    public const string NOMBRE_COLECCION_ESPACIOTRABAJO = "espaciotrabajo";

    public DbSet<EntidadEspacioTrabajo> espaciosTrabajo;

    public static MongoDbContextCreadorContenido Create(IMongoDatabase database)
    {
        var pack = new ConventionPack
        {
            new IgnoreExtraElementsConvention(true),
        };

        ConventionRegistry.Register("Conventions", pack, t => true);

        return new(new DbContextOptionsBuilder<MongoDbContextCreadorContenido>()
           .UseMongoDB(database.Client, database.DatabaseNamespace.DatabaseName)
           .Options);

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<EntidadEspacioTrabajo>().ToCollection(NOMBRE_COLECCION_ESPACIOTRABAJO);

    }
}
