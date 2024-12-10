using evaluacion.model.evaluacion;
using Microsoft.EntityFrameworkCore;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;
using MongoDB.EntityFrameworkCore.Extensions;

namespace evaluacion.services.dbcontext;

public class MongoDbContextEvaluacion(DbContextOptions<MongoDbContextEvaluacion> options) : DbContext(options) 
{
    public const string NOMBRE_COLECCION_EVALUACION = "evaluaciones";
    public DbSet<Evaluacion> Evaluaciones { get; set; }
    public static MongoDbContextEvaluacion Create(IMongoDatabase database)
    {
        // Este fragemnto sirve para evitar conflicts de mongo con cambios en el modelo
        var pack = new ConventionPack
            {
                new IgnoreExtraElementsConvention(true)
            };
        ConventionRegistry.Register("Conventions", pack, t => true);

        return new(new DbContextOptionsBuilder<MongoDbContextEvaluacion>()
           .UseMongoDB(database.Client, database.DatabaseNamespace.DatabaseName)
           .Options);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Evaluacion>().ToCollection(NOMBRE_COLECCION_EVALUACION);

    }

}
