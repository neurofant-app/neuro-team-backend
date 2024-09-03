using disenocurricular.model;
using Microsoft.EntityFrameworkCore;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using MongoDB.EntityFrameworkCore.Extensions;

namespace disenocurricular.services.dbcontext;
public class MongoDbContextDisenoCurricular(DbContextOptions<MongoDbContextDisenoCurricular> options) : DbContext(options)
{
    public const string NOMBRE_COLECCION_CURSOS = "cursos";
    public const string NOMBRE_COLECCION_ESPECIALIDADES = "especialidades";
    public const string NOMBRE_COLECCION_PLANES = "planes";
    public const string NOMBRE_COLECCION_TEMARIOS = "temarios";

    public DbSet<Curso> Cursos { get; set; }
    public DbSet<Plan> Planes { get; set; }
    public DbSet<Temario> Temarios { get; set; }
    public DbSet<Especialidad> Especialidades { get; set; }

    public static MongoDbContextDisenoCurricular Create(IMongoDatabase database)
    {
        var pack = new ConventionPack
        {
            new IgnoreExtraElementsConvention(true),
        };

        ConventionRegistry.Register("Conventions", pack, t => true);

        return new(new DbContextOptionsBuilder<MongoDbContextDisenoCurricular>()
           .UseMongoDB(database.Client, database.DatabaseNamespace.DatabaseName)
           .Options);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Curso>().ToCollection(NOMBRE_COLECCION_CURSOS);

        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Especialidad>().ToCollection(NOMBRE_COLECCION_ESPECIALIDADES);

        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Plan>().ToCollection(NOMBRE_COLECCION_PLANES);

        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Temario>().ToCollection(NOMBRE_COLECCION_TEMARIOS);
    }
}