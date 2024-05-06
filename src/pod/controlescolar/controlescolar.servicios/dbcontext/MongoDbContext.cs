using controlescolar.modelo.campi;
using Microsoft.EntityFrameworkCore;
using MongoDB.EntityFrameworkCore.Extensions;

namespace controlescolar.servicios.dbcontext;

public class MongoDbContext : DbContext
{
    public const string CollectionCampi = "controlescolar$campi";
    public DbSet<EntidadCampus> EntidadCampi { get; set; }

    public MongoDbContext(DbContextOptions<MongoDbContext> options) : base(options)
    { 
    
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<EntidadCampus>().ToCollection("campus");
    }

}
