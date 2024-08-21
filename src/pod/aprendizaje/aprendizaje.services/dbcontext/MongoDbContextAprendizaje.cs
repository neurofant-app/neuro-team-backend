using aprendizaje.model;
using aprendizaje.model.galeria;
using aprendizaje.model.neurona;
using aprendizaje.model.temario;
using Microsoft.EntityFrameworkCore;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;
using MongoDB.EntityFrameworkCore.Extensions;

namespace aprendizaje.services;

public class MongoDbContextAprendizaje(DbContextOptions<MongoDbContextAprendizaje> options) : DbContext(options)
{
    public const string NOMBRE_COLECCION_NEURONA = "neurona";
    public const string NOMBRE_COLECCION_TEMARIO = "temario";
    public const string NOMBRE_COLECCION_GALERIA = "galeria";
    public const string NOMBRE_COLECCION_CURSO = "curso";
    public const string NOMBRE_COLECCION_PLAN = "plan";

    public DbSet<Neurona> Neurona { get; set; }
    public DbSet<Temario> Temario { get; set; }
    public DbSet<Galeria> Galeria { get; set; }
    public DbSet<Curso> Curso { get; set; }
    public DbSet<Plan> Plan { get; set; }


    public static MongoDbContextAprendizaje Create(IMongoDatabase database)
    {
        var pack = new ConventionPack
        {
            new IgnoreExtraElementsConvention(true)
        };
        ConventionRegistry.Register("Conventions", pack, t => true);

        return new(new DbContextOptionsBuilder<MongoDbContextAprendizaje>()
           .UseMongoDB(database.Client, database.DatabaseNamespace.DatabaseName)
           .Options);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Neurona>().ToCollection(NOMBRE_COLECCION_NEURONA);

        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Temario>().ToCollection(NOMBRE_COLECCION_TEMARIO);

        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Galeria>().ToCollection(NOMBRE_COLECCION_GALERIA);

        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Curso>().ToCollection(NOMBRE_COLECCION_CURSO);

        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Plan>().ToCollection(NOMBRE_COLECCION_PLAN);
    }
}
