
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using seguridad.modelo;

namespace seguridad.servicios.mysql;

public class ConfiguracionRol : IEntityTypeConfiguration<Rol>
{
    public void Configure(EntityTypeBuilder<Rol> builder)
    {
        builder.ToTable("seguridad$rol");
        builder.HasKey(x => x.RolId);
        builder.Property(e => e.RolId).IsRequired(true);
        builder.Property(e => e.Nombre).HasMaxLength(200);
        builder.Property(e => e.Descripcion);
        builder.Property(e => e.Personalizado).IsRequired(true);
        builder.HasMany(x => x.RolPermisos).WithOne(y => y.Rol).HasForeignKey(z => z.RolId).OnDelete(DeleteBehavior.Cascade);
        builder.HasMany(x => x.RolGrupo).WithOne(y => y.Rol).HasForeignKey(z => z.RolId).OnDelete(DeleteBehavior.Cascade);
        builder.HasMany(x => x.RolUsuario).WithOne(y => y.Rol).HasForeignKey(z => z.RolId).OnDelete(DeleteBehavior.Cascade);
        builder.Ignore(x => x.Permisos);
    }
}
