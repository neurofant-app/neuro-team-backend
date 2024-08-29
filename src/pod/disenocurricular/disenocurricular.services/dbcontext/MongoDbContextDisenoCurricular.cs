using disenocurricular.model;
using Microsoft.EntityFrameworkCore;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;
using MongoDB.EntityFrameworkCore.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace disenocurricular.services.dbcontext;

public class MongoDbContextDisenoCurricular(DbContextOptions<MongoDbContextDisenoCurricular> options) : DbContext(options)
{
    public const string NOMBRE_COLECCION_CURSO = "curso";
    public const string NOMBRE_COLECCION_PLAN = "plan";
    public const string NOMBRE_COLECCION_TEMARIO = "temario";

    public DbSet<Curso> Curso { get; set; }
    public DbSet<Plan> Plan { get; set; }
    public DbSet<Temario> Temario { get; set; }

    public static MongoDbContextDisenoCurricular Create(IMongoDatabase database)
    {
        var pack = new ConventionPack
        {
            new IgnoreExtraElementsConvention(true)
        };
        ConventionRegistry.Register("Conventions", pack, t => true);

        return new(new DbContextOptionsBuilder<MongoDbContextDisenoCurricular>()
           .UseMongoDB(database.Client, database.DatabaseNamespace.DatabaseName)
           .Options);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Curso>().ToCollection(NOMBRE_COLECCION_CURSO);

        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Plan>().ToCollection(NOMBRE_COLECCION_PLAN);

        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Temario>().ToCollection(NOMBRE_COLECCION_TEMARIO);
    }
}
