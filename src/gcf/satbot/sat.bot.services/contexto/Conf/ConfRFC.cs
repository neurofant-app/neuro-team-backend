using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using modelo.repositorio.cfdi;

namespace sat.bot.services;

public class ConfRFC : IEntityTypeConfiguration<RFC>
{
    public void Configure(EntityTypeBuilder<RFC> builder)
    {
        builder.ToTable("rfc");
        builder.HasKey(e => e.rowid);
        builder.Property(e => e.rowid).ValueGeneratedOnAdd();
        builder.Property(e => e.Rfc).HasColumnName("rfc").HasMaxLength(15).IsRequired().HasColumnType("TEXT");
        builder.Property(e => e.Nombre).HasColumnName("nombre").HasMaxLength(250).IsRequired().HasColumnType("TEXT");
        //builder.HasMany(x => x.Resumenes).WithOne(y => y.RFC).HasForeignKey(z => z.RFCId)
        //.OnDelete(DeleteBehavior.Restrict);
        //builder.HasMany(x => x.Estadisticas).WithOne(y => y.RFC).HasForeignKey(z => z.RFCId)
        //.OnDelete(DeleteBehavior.Restrict);
        //builder.HasMany(x => x.Bitacoras).WithOne(y => y.RFC).HasForeignKey(z => z.RFCId)
        //.OnDelete(DeleteBehavior.Restrict);
    }
}
