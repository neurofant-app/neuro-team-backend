
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using seguridad.modelo;
using seguridad.modelo.relaciones;

namespace seguridad.servicios.mysql;

public class ConfiguracionPermisoGrupo : IEntityTypeConfiguration<PermisoGrupo>
{
    public void Configure(EntityTypeBuilder<PermisoGrupo> builder)
    {
        builder.ToTable("seguridad$permisoGrupo");
        builder.HasKey(x => x.Id);
        builder.Property(e => e.Id).IsRequired(true);
    }
}
