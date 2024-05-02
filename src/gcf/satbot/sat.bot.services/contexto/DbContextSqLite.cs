

using Microsoft.EntityFrameworkCore;
using modelo.repositorio.cfdi;
using sat.bot.modelo;

namespace sat.bot.services;

public class DbContextSqLite : DbContext
{
    private readonly string _connectionString;
    public DbContextSqLite(string connectionString)
    {
        _connectionString = connectionString;
    }


    public DbContextSqLite()
    {
    }


    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite(_connectionString);
    }

    public DbSet<CFDI> CFDIs { get; set; }
    public DbSet<RFC> Rfc { get; set; }
    public DbSet<CfdiUi> cfdisUi { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new ConfCFDI());
        modelBuilder.ApplyConfiguration(new ConfRFC());
        modelBuilder.ApplyConfiguration(new ConfCfdiUi());
        base.OnModelCreating(modelBuilder);



    }
}




