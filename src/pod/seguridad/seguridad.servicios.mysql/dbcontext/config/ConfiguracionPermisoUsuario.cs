
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using seguridad.modelo.relaciones;

namespace seguridad.servicios.mysql;

public class ConfiguracionPermisoUsuario : IEntityTypeConfiguration<PermisoUsuario>
{
    public void Configure(EntityTypeBuilder<PermisoUsuario> builder)
    {
        builder.ToTable("seguridad$permisoUsuario");
        builder.HasKey(x => new { x.UsuarioId, x.PermisoId});
    }
}
