
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using seguridad.modelo;

namespace seguridad.servicios.mysql;

public class ConfiguracionPermiso : IEntityTypeConfiguration<Permiso>
{
    public void Configure(EntityTypeBuilder<Permiso> builder)
    {
        builder.ToTable("seguridad$permiso");
        builder.HasKey(x => x.PermisoId);
        builder.Property(e => e.PermisoId).IsRequired(true);
        builder.Property(e => e.Nombre).HasMaxLength(200);
        builder.Property(e => e.Descripcion);
        builder.Property(e => e.Ambito).IsRequired(true);
        builder.HasMany(x => x.PermisoGrupo).WithOne(y => y.Permiso).HasForeignKey(z => z.PermisoId).OnDelete(DeleteBehavior.Cascade);
        builder.HasMany(x => x.PermisoUsuario).WithOne(y => y.Permiso).HasForeignKey(z => z.PermisoId).OnDelete(DeleteBehavior.Cascade);
    }
}
