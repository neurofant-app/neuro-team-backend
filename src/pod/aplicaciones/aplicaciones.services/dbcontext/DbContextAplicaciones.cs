using aplicaciones.model;
using aplicaciones.services.configuraciones;
using Microsoft.EntityFrameworkCore;

namespace aplicaciones.services.dbContext;

public class DbContextAplicaciones : DbContext
{
    public const string TablaAplicaciones = "aplicacion$aplicaciones";
    public const string TablaInvitaciones = "aplicacion$invitaciones";
    public const string TablaPlantillasInvitaciones = "aplicacion$plantillasinvitaciones";
    public const string TablaLogosAplicaciones = "aplicacion$logosaplicaciones";
    public const string TablaConsentimientos = "aplicacion$consentimientos";
    public DbContextAplicaciones(DbContextOptions<DbContextAplicaciones> options) : base(options)
    {

    }

    public DbSet<Aplicacion> Aplicaciones { get; set; }
    public DbSet<EntidadInvitacion> Invitaciones { get; set; }
    public DbSet<EntidadPlantillaInvitacion> PlantillasAplicaciones { get; set; }
    public DbSet<EntidadLogoAplicacion> LogosAplicaciones { get; set; }
    public DbSet<EntidadConsentimiento> Consentimientos { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new ConfiguracionAplicacion());
        modelBuilder.ApplyConfiguration(new ConfiguracionInvitacion());
        modelBuilder.ApplyConfiguration(new ConfiguracionPlatillaInvitacion());
        modelBuilder.ApplyConfiguration(new ConfiguracionLogoAplicacion());
        modelBuilder.ApplyConfiguration(new ConfiguracionConsentimiento());
        base.OnModelCreating(modelBuilder);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
    }
}
