 using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using modelo.repositorio.cfdi;

namespace sat.bot.services;

public class ConfCFDI : IEntityTypeConfiguration<CFDI>
{
    public void Configure(EntityTypeBuilder<CFDI> builder)
    {
        builder.ToTable("cfdis");
        builder.HasKey(e => e.rowid);
        builder.Property(e => e.rowid).ValueGeneratedOnAdd();

        builder.Property(e => e.UUID).HasColumnName("uuid").HasMaxLength(64).IsRequired().HasColumnType("TEXT");   //:):)

        builder.Property(e => e.Version).HasColumnName("version").HasMaxLength(5).IsRequired().HasColumnType("TEXT");  //:):)

        builder.Property(e => e.Emitido).HasColumnName("emitido").IsRequired(); //:):)    

        builder.Property(e => e.Cancelado).HasColumnName("cancelado").IsRequired(); //:):)

        builder.Property(e => e.FechaCFDI).HasColumnName("fechacfdi").IsRequired().HasColumnType("INTEGER"); //:):)

        builder.Property(e => e.iano).HasColumnName("iano").IsRequired().HasColumnType("INTEGER");  //:):)

        builder.Property(e => e.imes).HasColumnName("imes").IsRequired().HasColumnType("INTEGER"); //:):)

        builder.Property(e => e.idia).HasColumnName("idia").IsRequired().HasColumnType("INTEGER");  //:):)

        builder.Property(e => e.RFCId).HasColumnName("rfcid").IsRequired().HasColumnType("INTEGER");  //:):)

        builder.Property(e => e.SubTotal).HasColumnName("subtotal").IsRequired().HasColumnType("NUMERIC"); //:):)

        builder.Property(e => e.Total).HasColumnName("total").IsRequired().HasColumnType("NUMERIC");  //:):)

        builder.Property(e => e.TotalIRetenidos).HasColumnName("totaliretenidos").IsRequired().HasColumnType("NUMERIC");  //:):)

        builder.Property(e => e.TotalITrasladados).HasColumnName("totalitrasladados").IsRequired().HasColumnType("NUMERIC");  //:):)

        builder.Property(e => e.Uso).HasColumnName("uso").HasMaxLength(5).IsRequired(false).HasColumnType("TEXT");  //:):)

        builder.Property(e => e.TieneIRetenidos).HasColumnName("tieneiretenidos").IsRequired();  //:)/:)

        builder.Property(e => e.TieneITrasladados).HasColumnName("tieneitrasladados").IsRequired(); //:)/:)

        builder.Property(e => e.TieneRelacionados).HasColumnName("tienerelacionados").IsRequired();  //:)/:)

        builder.Property(e => e.TieneI3os).HasColumnName("tienei3os").HasMaxLength(5).IsRequired();  //:)/:)

        builder.Property(e => e.TieneInfoAduanera).HasColumnName("tieneinfoaduanera").IsRequired(); //:)/:)

        builder.Property(e => e.TieneCPredial).HasColumnName("tienecpredial").IsRequired(); //:)/:)

        builder.Property(e => e.TieneComplementos).HasColumnName("tienecomplementos").IsRequired(); //:)/:)

        builder.Property(e => e.TieneAddenda).HasColumnName("tieneaddenda").IsRequired(); // :)/:)

        builder.Property(e => e.Serie).HasColumnName("serie").HasMaxLength(100).IsRequired(false).HasColumnType("TEXT");  //:)/:)

        builder.Property(e => e.Folio).HasColumnName("folio").HasMaxLength(100).IsRequired(false).HasColumnType("TEXT");  //:)/:)

        builder.Property(e => e.FormaPago).HasColumnName("formapago").HasMaxLength(100).IsRequired(false).HasColumnType("TEXT");  // :)/:)

        builder.Property(e => e.Moneda).HasColumnName("moneda").HasMaxLength(5).IsRequired(false).HasColumnType("TEXT");  //:)/:)

        builder.Property(e => e.TipoDeComprobante).HasColumnName("tipodecomprobante").HasMaxLength(5).IsRequired().HasColumnType("TEXT");  //:)

        builder.Property(e => e.MetodoPago).HasColumnName("metodopago").HasMaxLength(5).HasColumnType("TEXT");  //:):)

        builder.Property(e => e.LugarExpedicion).HasColumnName("lugarexpedicion").HasMaxLength(5).IsRequired().HasColumnType("TEXT");  //:):)

        builder.HasOne(x => x.RFC).WithMany(y => y.Cfdis).HasForeignKey(z => z.RFCId);
        //builder.HasMany(x => x.Traslados).WithOne(y => y.Cfdi).HasForeignKey(z => z.IdPadre)
        //.OnDelete(DeleteBehavior.Restrict);
        //builder.HasMany(x => x.Retenciones).WithOne(y => y.Cfdi).HasForeignKey(z => z.IdPadre)
        //.OnDelete(DeleteBehavior.Restrict);

    }
}
