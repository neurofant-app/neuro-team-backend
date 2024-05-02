using aplicaciones.model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace aplicaciones.services.configuraciones;

public class ConfiguracionLogoAplicacion : IEntityTypeConfiguration<LogoAplicacion>
{
    public void Configure(EntityTypeBuilder<LogoAplicacion> builder)
    {
        builder.ToTable("aplicacion$logosaplicaciones");
        builder.HasKey(x => x.Id);
        builder.Property(e => e.Id).IsRequired(true);
        builder.Property(e=> e.AplicacionId).IsRequired(true);
        builder.Property(e => e.Tipo).IsRequired(true);
        builder.Property(e => e.Idioma).HasMaxLength(10).IsRequired(true);
        builder.Property(e => e.IdiomaDefault).IsRequired(true);
        builder.Property(e => e.LogoURLBase64).IsRequired(true);
        builder.Property(e => e.EsSVG).IsRequired(true);
        builder.Property(e => e.EsUrl).IsRequired(true);
        builder.HasOne(x => x.Aplicacion).WithMany(y => y.Logotipos).HasForeignKey(z => z.AplicacionId).OnDelete(DeleteBehavior.Cascade);
    }
}
