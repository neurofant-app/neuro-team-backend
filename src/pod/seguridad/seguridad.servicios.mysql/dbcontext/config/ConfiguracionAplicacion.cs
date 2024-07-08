
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using seguridad.modelo;
using seguridad.modelo.instancias;

namespace seguridad.servicios.mysql;

public class ConfiguracionAplicacion : IEntityTypeConfiguration<Aplicacion>
{
    public void Configure(EntityTypeBuilder<Aplicacion> builder)
    {
        builder.ToTable("seguridad$aplicacion");
        builder.HasKey(x => x.ApplicacionId);
        builder.Property(e => e.ApplicacionId).IsRequired(true);
        builder.Property(e => e.Nombre).HasMaxLength(100);
        builder.Property(e => e.Descripcion);
        builder.HasMany(x => x.Modulos).WithOne(y => y.Aplicacion).HasForeignKey(z => z.ApplicacionId).OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(x => x.InstanciaAplicacion).WithOne(y => y.Aplicacion).HasForeignKey<InstanciaAplicacion>(z => z.ApplicacionId);
    }
}