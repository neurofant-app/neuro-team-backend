
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using seguridad.modelo;

namespace seguridad.servicios.mysql;

public class ConfiguracionModulo : IEntityTypeConfiguration<Modulo>
{
    public void Configure(EntityTypeBuilder<Modulo> builder)
    {
        builder.ToTable("seguridad$modulo");
        builder.HasKey(x => x.ModuloId);
        builder.Property(e => e.Nombre).HasMaxLength(200);
        builder.Property(e => e.Descripcion);
        builder.HasMany(x => x.RolesPredefinidos).WithOne(y => y.Modulo).HasForeignKey(z => z.ModuloId).OnDelete(DeleteBehavior.Cascade);
        builder.HasMany(x => x.Permisos).WithOne(y => y.Modulo).HasForeignKey(z => z.ModuloId).OnDelete(DeleteBehavior.Cascade);
    }
}
