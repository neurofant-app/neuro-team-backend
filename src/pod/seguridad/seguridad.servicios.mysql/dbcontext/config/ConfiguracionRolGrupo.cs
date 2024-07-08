
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using seguridad.modelo;
using seguridad.modelo.relaciones;

namespace seguridad.servicios.mysql;

public class ConfiguracionRolGrupo : IEntityTypeConfiguration<RolGrupo>
{
    public void Configure(EntityTypeBuilder<RolGrupo> builder)
    {
        builder.ToTable("seguridad$rolgrupo");
        builder.HasKey(x => x.Id);
        builder.Property(e => e.Id).IsRequired(true);
    }
}
