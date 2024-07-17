using aprendizaje.model.neurona;
using aprendizaje.model.temario;
using Microsoft.EntityFrameworkCore;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;
using MongoDB.EntityFrameworkCore.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aprendizaje.services;

public class MongoDbContextAprendizaje(DbContextOptions<MongoDbContextAprendizaje> options) : DbContext(options)
{
    public const string NOMBRE_COLECCION_NEURONA = "neurona";
    public const string NOMBRE_COLECCION_TEMARIO = "temario";

    public DbSet<Neurona> Neurona { get; set; }
    public DbSet<Temario> Temario { get; set; }

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
    }
}
