using aplicaciones.model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace aplicaciones.services.configuraciones;

public class ConfiguracionAplicacion : IEntityTypeConfiguration<Aplicacion>
{
    public void Configure(EntityTypeBuilder<Aplicacion> builder)
    {
        builder.ToTable("aplicacion$aplicaciones");
        builder.HasKey(x => x.Id);
        builder.Property(e => e.Id).IsRequired(true);
        builder.Property(e => e.Nombre).HasMaxLength(200).IsRequired(true);   
        builder.Property(e => e.Activa).IsRequired(true);
        builder.HasMany(x => x.Invitaciones).WithOne(y => y.Aplicacion).HasForeignKey(z => z.AplicacionId).OnDelete(DeleteBehavior.Cascade);
        builder.HasMany(x => x.Plantillas).WithOne(y => y.Aplicacion).HasForeignKey(z => z.AplicacionId).OnDelete(DeleteBehavior.Cascade);
        builder.HasMany(x => x.Logotipos).WithOne(y => y.Aplicacion).HasForeignKey(z => z.AplicacionId).OnDelete(DeleteBehavior.Cascade);
        builder.HasMany(x => x.Consentimientos).WithOne(y => y.Aplicacion).HasForeignKey(z => z.AplicacionId).OnDelete(DeleteBehavior.Cascade);

    }
}
