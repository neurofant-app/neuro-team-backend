using Microsoft.EntityFrameworkCore;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;
using productos.model.categoria;

namespace organizacion.services.dbcontext;

public class MongoDbContextProductos(DbContextOptions<MongoDbContextProductos> options) : DbContext(options) 
{
    public const string NOMBRE_COLECCION_CATEGORIAS = "categorias";
    public DbSet<Categoria> Categorias { get; set; }

    public static MongoDbContextProductos Create(IMongoDatabase database)
    {
        // Este fragemnto sirve para evitar conflicts de mongo con cambios en el modelo
        var pack = new ConventionPack
            {
                new IgnoreExtraElementsConvention(true)
            };
        ConventionRegistry.Register("Conventions", pack, t => true);

        return new(new DbContextOptionsBuilder<MongoDbContextProductos>()
           .UseMongoDB(database.Client, database.DatabaseNamespace.DatabaseName)
           .Options);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }

}
