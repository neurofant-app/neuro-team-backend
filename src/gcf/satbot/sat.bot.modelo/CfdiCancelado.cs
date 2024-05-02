
namespace sat.bot.modelo;

public class CfdiCancelado
{
    public string FolioFiscal { get; set; }
    public string RFCEmisor { get; set; }
    public string Nombre { get; set; }
    public string RFCReceptor { get; set; }
    public DateTime  FechaEmisión { get; set; }
    public DateTime FechaCertificación { get; set; }
    public DateTime FechaSolicitud { get; set; }
    public string PacCertifico { get; set; }
    public decimal Total { get; set; }
    public string EfectoComprobante { get; set; }
    public string EstatusSolicitud { get; set; }
    public string Motivo { get; set; }
    public string FolioSustitución { get; set; }
}
