
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using seguridad.modelo.instancias;


namespace seguridad.servicios.mysql;

public class ConfiguracionInstanciaAplicacion : IEntityTypeConfiguration<InstanciaAplicacion>
{
    public void Configure(EntityTypeBuilder<InstanciaAplicacion> builder)
    {
        builder.ToTable("seguridad$instanciaaplicacion");
        builder.HasKey(x => x.Id);
        builder.Property(e => e.Id).IsRequired(true);
        builder.Property(e => e.DominioId).HasMaxLength(200).IsRequired(true);
        builder.HasMany(x => x.RolesPersonalizados).WithOne(y => y.InstanciaAplicacion).HasForeignKey(z => z.InstanciaAplicacionId).OnDelete(DeleteBehavior.Cascade);
        builder.HasMany(x => x.RolGrupo).WithOne(y => y.InstanciaAplicacion).HasForeignKey(z => z.Id).OnDelete(DeleteBehavior.Cascade);
        builder.HasMany(x => x.PermisoGrupo).WithOne(y => y.InstanciaAplicacion).HasForeignKey(z => z.Id).OnDelete(DeleteBehavior.Cascade);
        builder.HasMany(x => x.RolUsuarios).WithOne(y => y.InstanciaAplicacion).HasForeignKey(z => z.Id).OnDelete(DeleteBehavior.Cascade);
        builder.HasMany(x => x.PermisoUsuarios).WithOne(y => y.InstanciaAplicacion).HasForeignKey(z => z.Id).OnDelete(DeleteBehavior.Cascade);

        builder.Ignore(x => x.RolesPersonalizados);
        builder.Ignore(x => x.MiembrosRol);
        builder.Ignore(x => x.MiembrosPermiso);
    }
}
