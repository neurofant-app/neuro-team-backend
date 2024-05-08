
using aplicaciones.model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace aplicaciones.services.configuraciones;

public class ConfiguracionInvitacion : IEntityTypeConfiguration<EntidadInvitacion>
{
    public void Configure(EntityTypeBuilder<EntidadInvitacion> builder)
    {
        builder.ToTable("aplicacion$invitaciones");
        builder.HasKey(x => x.Id);
        builder.Property(e => e.Id).IsRequired(true);
        builder.Property(e => e.AplicacionId).IsRequired(true);
        builder.Property(e => e.Fecha).IsRequired(true);
        builder.Property(e => e.Estado).IsRequired(true);
        builder.Property(e => e.Email).HasMaxLength(250).IsRequired(true);
        builder.Property(e => e.RolId).IsRequired(true);
        builder.Property(e => e.Nombre).IsRequired(true);
        builder.Property(e => e.Tipo).IsRequired(true);
        builder.Property(e => e.Token).IsRequired(false);
        builder.HasOne(x => x.Aplicacion).WithMany(y => y.Invitaciones).HasForeignKey(z => z.AplicacionId).OnDelete(DeleteBehavior.Cascade);
    }
}
