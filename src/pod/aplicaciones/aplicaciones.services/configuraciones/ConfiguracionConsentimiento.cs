using aplicaciones.model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace aplicaciones.services.configuraciones;

public class ConfiguracionConsentimiento : IEntityTypeConfiguration<Consentimiento>
{
    public void Configure(EntityTypeBuilder<Consentimiento> builder)
    {
        builder.ToTable("aplicacion$consentimientos");
        builder.HasKey(x => x.Id);
        builder.Property(e => e.Id).IsRequired(true);
        builder.Property(e => e.AplicacionId).IsRequired(true);
        builder.Property(e => e.Tipo).IsRequired(true);
        builder.Property(e => e.Idioma).HasMaxLength(10).IsRequired(true);
        builder.Property(e => e.IdiomaDefault).IsRequired(true);
        builder.Property(e => e.Texto).IsRequired(true);
        builder.HasOne(x => x.Aplicacion).WithMany(y => y.Consentimientos).HasForeignKey(z => z.AplicacionId).OnDelete(DeleteBehavior.Cascade);

    }
}
