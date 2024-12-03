using Microsoft.EntityFrameworkCore;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;
using MongoDB.EntityFrameworkCore.Extensions;
using organizacion.model.dominio;
using organizacion.model.usuariodominio;

namespace organizacion.services.dbcontext;

public class MongoDbContextOrganizacion(DbContextOptions<MongoDbContextOrganizacion> options) : DbContext(options) 
{
    public const string NOMBRE_COLECCION_DOMINIOS = "dominios";
    public const string NOMBRE_COLECCION_USUARIODOMINIOS = "usuariodominios";
    public DbSet<Dominio> Dominios { get; set; }
    public DbSet<UsuarioDominio> UsuarioDominios { get; set; }

    public static MongoDbContextOrganizacion Create(IMongoDatabase database)
    {
        // Este fragemnto sirve para evitar conflicts de mongo con cambios en el modelo
        var pack = new ConventionPack
            {
                new IgnoreExtraElementsConvention(true)
            };
        ConventionRegistry.Register("Conventions", pack, t => true);

        return new(new DbContextOptionsBuilder<MongoDbContextOrganizacion>()
           .UseMongoDB(database.Client, database.DatabaseNamespace.DatabaseName)
           .Options);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Dominio>().ToCollection(NOMBRE_COLECCION_DOMINIOS);
        modelBuilder.Entity<UsuarioDominio>().ToCollection(NOMBRE_COLECCION_USUARIODOMINIOS);

    }

}
