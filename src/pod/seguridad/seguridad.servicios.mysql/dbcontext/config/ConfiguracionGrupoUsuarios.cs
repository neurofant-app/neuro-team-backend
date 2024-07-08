
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using seguridad.modelo;

namespace seguridad.servicios.mysql;

public class ConfiguracionGrupoUsuarios: IEntityTypeConfiguration<GrupoUsuarios>
{
    public void Configure(EntityTypeBuilder<GrupoUsuarios> builder)
    {
        builder.ToTable("seguridad$grupousuarios");
        builder.HasKey(x => x.Id);
        builder.Property(e => e.DominioId).HasMaxLength(200).IsRequired(true);
        builder.Property(e => e.ApplicacionId).HasMaxLength(200).IsRequired(true);
        builder.Property(e => e.Nombre).HasMaxLength(200);
        builder.Property(e => e.Descripcion);
        builder.HasMany(x => x.UsuariosId).WithOne(y => y.GrupoUsuarios).HasForeignKey(z => z.GrupoId).OnDelete(DeleteBehavior.Cascade);
        builder.HasMany(x => x.RolGrupo).WithOne(y => y.Grupo).HasForeignKey(z => z.GrupoId).OnDelete(DeleteBehavior.Cascade);
        builder.HasMany(x => x.PermisoGrupo).WithOne(y => y.Grupo).HasForeignKey(z => z.GrupoId).OnDelete(DeleteBehavior.Cascade);

    }
}
