using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using modelo.repositorio.cfdi;

namespace repositorio.cfdi.sqlite.Conf
{
    public class ConfRetencion : IEntityTypeConfiguration<Retencion>
    {
        public void Configure(EntityTypeBuilder<Retencion> builder)
        {
            builder.ToTable("retencion");
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id).HasColumnName("id").IsRequired().HasColumnType("NUMERIC"); 
            builder.Property(e => e.Base).HasColumnName("base").IsRequired().HasColumnType("NUMERIC");   
            builder.Property(e => e.Impuesto).HasColumnName("impuesto").HasMaxLength(4).IsRequired().HasColumnType("TEXT");  
            builder.Property(e => e.TipoFactor).HasColumnName("tipofactor").HasMaxLength(6).IsRequired().HasColumnType("TEXT");  
            builder.Property(e => e.Importe).HasColumnName("importe").IsRequired().HasColumnType("NUMERIC");  
        }
    }
}
