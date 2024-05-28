

using Microsoft.EntityFrameworkCore;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;
using MongoDB.EntityFrameworkCore.Extensions;
using seguridad.modelo;
using seguridad.modelo.instancias;

namespace seguridad.servicios.dbcontext;

public class MongoDbContext(DbContextOptions<MongoDbContext> options) : DbContext(options)
{
    public const string NOMBRE_COLECCION_APLICACION = "aplicacion";
    public const string NOMBRE_COLECCION_GRUPOUSUARIOS = "grupoUsuarios";
    public const string NOMBRE_COLECCION_INSTANCIAAPLICAION = "instanciaAplicacion";

    public DbSet<Aplicacion> Aplicacion { get; set; }
    public DbSet<GrupoUsuarios> GrupoUsuarios { get; set; }
    public DbSet<InstanciaAplicacion> instanciaAplicacion { get; set; }

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
        modelBuilder.Entity<Aplicacion>().ToCollection(NOMBRE_COLECCION_APLICACION);

        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<GrupoUsuarios>().ToCollection(NOMBRE_COLECCION_GRUPOUSUARIOS);

        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<InstanciaAplicacion>().ToCollection(NOMBRE_COLECCION_INSTANCIAAPLICAION);
    }

}