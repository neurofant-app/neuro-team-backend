namespace sat.bot.modelo;

public class CfdiRecibido
{
    public string FolioFiscal { get; set; }
    public string RFCEmisor { get; set; }
    public string NombreEmisor { get; set; }
    public string RFCReceptor { get; set; }
    public string NombreReceptor { get; set; }
    public DateTime FechaEmisión { get; set; }
    public DateTime FechaCertificacion { get; set; }
    public string PacCertifico { get; set; }
    public decimal Total { get; set; }
    public string EfectoComprobante { get; set; }
    public string EstatusCancelacion { get; set; }
    public string EstadoComprobante { get; set; }
    public string? StatusProcesoCancelacion { get; set; }
    public DateTime? FechaProcesoCancelacion { get; set; }
    public string? RfcCuentaTerceros { get; set; }
    public string? UrlDescarga { get; set; }
}
