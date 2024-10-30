using controlescolar.modelo.escuela;
using controlescolar.modelo.prueba;
using Microsoft.EntityFrameworkCore;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;
using MongoDB.EntityFrameworkCore.Extensions;

namespace controlescolar.servicios.dbcontext;

public class MongoDbContext(DbContextOptions<MongoDbContext> options) : DbContext(options)
{
    public const string NOMBRE_COLECCION_CAMPUS = "campus";
    public const string NOMBRE_COLECCION_ALUMNOS = "alumnos";
    public const string NOMBRE_COLECCION_PRUEBA = "prueba";
    public const string NOMBRE_COLECCION_INSTRUCTORES = "instructores";
    public const string NOMBRE_COLECCION_ESCUELAS = "escuelas";
    public DbSet<EntidadPrueba> EntidadPrueba { get; set; }
    public DbSet<EntidadEscuela> Escuelas { get; set; }

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
        modelBuilder.Entity<EntidadPrueba>().ToCollection(NOMBRE_COLECCION_PRUEBA);
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<EntidadEscuela>().ToCollection(NOMBRE_COLECCION_ESCUELAS);
    }

}
