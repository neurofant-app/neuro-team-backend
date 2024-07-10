
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using seguridad.modelo;

namespace seguridad.servicios.mysql;

public class ConfiguracionUsuarioGrupo: IEntityTypeConfiguration<UsuarioGrupo>
{
    public void Configure(EntityTypeBuilder<UsuarioGrupo> builder)
    {
        builder.ToTable("seguridad$usuariogrupo");
        builder.HasKey(x => x.Id);
        builder.Property(e => e.Id).IsRequired(true);
    }
}
