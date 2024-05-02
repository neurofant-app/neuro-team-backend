
namespace sat.bot.modelo;

public class CfdiUi
{
    /// <summary>
    /// Identificador único del CFDI
    /// </summary>
    public string UUID { get; set; }
    public TipoComprobante Tipo { get; set; }
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
    public string? Motivo { get; set; }
    public string? FolioSustitución { get; set; }
    public string? UrlDescarga { get; set; }

}

public enum TipoComprobante
{
    Emitido = 0,
    Recibido = 1,
    EmitidoCancelado = 2,
    RecibidoCancelado = 3
}