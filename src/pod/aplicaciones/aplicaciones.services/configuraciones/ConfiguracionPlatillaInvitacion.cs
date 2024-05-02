using aplicaciones.model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace aplicaciones.services.configuraciones;

public class ConfiguracionPlatillaInvitacion : IEntityTypeConfiguration<PlantillaInvitacion>
{
    public void Configure(EntityTypeBuilder<PlantillaInvitacion> builder)
    {
        builder.ToTable("aplicacion$plantillasinvitaciones");
        builder.HasKey(x => x.Id);
        builder.Property(e => e.Id).IsRequired(true);
        builder.Property(e => e.TipoContenido).IsRequired(true);
        builder.Property(e => e.AplicacionId).IsRequired(true);
        builder.Property(e => e.Plantilla).IsRequired(true);
        builder.HasOne(x => x.Aplicacion).WithMany(y => y.Plantillas).HasForeignKey(z => z.AplicacionId).OnDelete(DeleteBehavior.Cascade);


    }
}
