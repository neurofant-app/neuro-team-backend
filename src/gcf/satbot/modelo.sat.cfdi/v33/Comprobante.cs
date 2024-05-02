
namespace modelo.sat.cfdi.v33
{/// <summary>
/// Estándar de Comprobante Fiscal Digital por Internet V3.3    
/// </summary>
    public class Comprobante
    {
        /// <summary>
        /// Indica la versión del estándar bajo el que se encuentra expresado el comprobante.   
        /// </summary>
        public string Version { get; set; } = "3.3";

        /// <summary>
        /// Precisar la serie para control interno del contribuyente.
        /// </summary>
        public string? Serie { get; set; }

        /// <summary>
        /// Expresa el folio del comprobante
        /// </summary>
        public string? Folio { get; set; }

        /// <summary>
        /// fecha y hora de expedición del Comprobante Fiscal Digital por Internet.
        /// </summary>
        public DateTime Fecha { get; set; }//t_FechaH

        /// <summary>
        /// sello digital del comprobante fiscal.
        /// </summary>
        public string Sello { get; set; }

        /// <summary>
        /// clave de la forma de pago de los bienes o servicios
        /// </summary>
        public string? FormaPago { get; set; }//catCFDI:c_FormaPago

        /// <summary>
        /// número de serie del certificado de sello digital
        /// </summary>
        public string NoCertificado { get; set; }

        /// <summary>
        /// certificado de sello digital que ampara al comprobante
        /// </summary>
        public string Certificado { get; set; }

        /// <summary>
        /// condiciones comerciales aplicables para el pago del comprobante fiscal digital por Internet.
        /// </summary>
        public string? CondicionesDePago { get; set; }

        //7suma de los importes de los conceptos antes de descuentos de impuesto.
        public decimal SubTotal { get; set; }//t_Importe

        /// <summary>
        /// importe total de los descuentos aplicables antes de impuestos.
        /// </summary>
        public decimal? Descuento { get; set; }//t_Importe

        /// <summary>
        /// clave de la moneda utilizada para expresar los montos
        /// </summary>
        public string Moneda { get; set; }//catCFDI:c_Moneda

        /// <summary>
        /// tipo de cambio conforme con la moneda usada.
        /// </summary>
        public decimal? TipoCambio { get; set; }

        /// <summary>
        /// Suma de los subtotales anteriores
        /// </summary>
        public decimal Total { get; set; }//t_Importe

        /// <summary>
        /// clave del efecto del comprobante fiscal para el contribuyente emisor.
        /// </summary>
        public string TipoDeComprobante { get; set; }//catCFDI:c_TipoDeComprobante

        /// <summary>
        /// clave del método de pago que aplica para este comprobante
        /// </summary>
        public string? MetodoPago { get; set; }//catCFDI:c_MetodoPago

        /// <summary>
        /// el código postal del lugar de expedición del comprobante
        /// </summary>
        public string LugarExpedicion { get; set; }//catCFDI:c_CodigoPostal

        /// <summary>
        /// clave de confirmación que entregue el PAC
        /// </summary>
        public string? Confirmacion { get; set; }

        public CfdiRelacionados? CfdiRelacionados { get; set; }
        public Emisor Emisor { get; set; }
        public Receptor Receptor { get; set; }
        public Conceptos Conceptos { get; set; }
        public Impuestos? Impuestos { get; set; }
        public Complemento? Complemento { get; set; }
        public Addenda? Addenda { get; set; }

       public Comprobante ()
        {
            Emisor = new Emisor();
            Receptor = new Receptor();
            Conceptos= new Conceptos();
        }
    }
}
