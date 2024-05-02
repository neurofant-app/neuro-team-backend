using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using sat.bot.modelo;

namespace sat.bot.services;

public class ConfCfdiUi : IEntityTypeConfiguration<CfdiUi>
{
    public void Configure(EntityTypeBuilder<CfdiUi> builder)
    {
        builder.ToTable("consulta@cfdisui");
        builder.HasKey(e => e.UUID);
        builder.Property(e => e.UUID).HasColumnName("folioFiscal").IsRequired().HasColumnType("TEXT").HasMaxLength(64);
        builder.Property(e => e.Tipo).HasColumnName("tipo").IsRequired().HasColumnType("NUMERIC");
        builder.Property(e => e.RFCEmisor).HasColumnName("rfcemisor").IsRequired().HasColumnType("TEXT").HasMaxLength(15);
        builder.Property(e => e.NombreEmisor).HasColumnName("nombreemisor").IsRequired().HasColumnType("TEXT").HasMaxLength(64);
        builder.Property(e => e.RFCReceptor).HasColumnName("rfcreceptor").IsRequired().HasColumnType("TEXT").HasMaxLength(15);
        builder.Property(e => e.NombreReceptor).HasColumnName("nombrereceptor").IsRequired().HasColumnType("TEXT").HasMaxLength(64);
        builder.Property(e => e.FechaEmisión).HasColumnName("fechaemision").IsRequired();
        builder.Property(e => e.FechaCertificacion).HasColumnName("fechacertificacion").IsRequired();
        builder.Property(e => e.PacCertifico).HasColumnName("paccertifico").IsRequired().HasColumnType("TEXT").HasMaxLength(25);
        builder.Property(e => e.Total).HasColumnName("total").IsRequired().HasColumnType("NUMERIC");
        builder.Property(e => e.EfectoComprobante).HasColumnName("efectocomprobante").IsRequired().HasColumnType("TEXT").HasMaxLength(64);
        builder.Property(e => e.EstatusCancelacion).HasColumnName("estatuscancelacion").IsRequired().HasColumnType("TEXT").HasMaxLength(64);
        builder.Property(e => e.EstadoComprobante).HasColumnName("estadocomprobante").IsRequired().HasColumnType("TEXT").HasMaxLength(64);
        builder.Property(e => e.StatusProcesoCancelacion).HasColumnName("statusprocesocancelacion").HasColumnType("TEXT").HasMaxLength(64);
        builder.Property(e => e.FechaProcesoCancelacion).HasColumnName("fechaprocesocancelacion");
        builder.Property(e => e.Motivo).HasColumnName("motivo").HasColumnType("TEXT").HasMaxLength(128);
        builder.Property(e => e.FolioSustitución).HasColumnName("FolioSustitucion").HasColumnType("TEXT").HasMaxLength(64);
        builder.Property(e => e.UrlDescarga).HasColumnName("urldescarga").HasColumnType("TEXT");
    }
}