
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using seguridad.modelo.relaciones;

namespace seguridad.servicios.mysql;

public class ConfiguracionRolUsuario : IEntityTypeConfiguration<RolUsuario>
{
    public void Configure(EntityTypeBuilder<RolUsuario> builder)
    {
        builder.ToTable("seguridad$rolUsuario");
        builder.HasKey(x =>new { x.UsuarioId,x.RolId });

    }
}
