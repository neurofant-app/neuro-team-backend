namespace modelo.sat.cfdi.v40
{
    /// <summary>
    /// Estándar de CFDI.
    /// </summary>
    public class Comprobante
    {
        //Propiedades.

        /// <summary>
        /// Propiedad requerida con valor prefijado a 4.0, indicando la versión del 
        /// comprobante.
        /// </summary>
        public string Version { get; set; } = "4.0";

        /// <summary>
        /// Propiedad opcional que precisa la serie para el control interno del contribuyente.
        /// </summary>
        public string? Serie { get; set; }

        /// <summary>
        /// Propiedad opcional que expresa el folio del comprobante.
        /// </summary>
        public string? Folio { get; set; }

        /// <summary>
        /// Propiedad requerida para la expresión de la fecha y hora de expedición 
        /// del CDFI.
        /// </summary>
        public DateTime Fecha { get; set; }

        /// <summary>
        /// Propiedad que contendrá el sello digital del comprobante.
        /// </summary>
        public string Sello { get; set; }

        /// <summary>
        /// Propiedad Condicional para expresar la clave de la forma de pago.
        /// </summary>
        public string? FormaPago { get; set; }

        /// <summary>
        /// Propiedad requerida para expresar el No. de serie del certifcado.
        /// </summary>
        public string NoCertificado { get; set; }

        /// <summary>
        /// Propiedad requerida que sirve para incorporar el certificado.
        /// </summary>
        public string Certificado { get; set; }

        /// <summary>
        /// Propiedad condicional para expresar las condiciones comerciales.
        /// </summary>
        public string? CondicionesDePago { get; set; }

        /// <summary>
        /// Propiedad requerida que representa  la suma de los conceptos antes de 
        /// descuentos e impuesto.
        /// </summary>
        public decimal SubTotal { get; set; }

        /// <summary>
        /// Propiedad condicional que representa el importe total de los descuentos
        /// aplicables antes de impuestos.
        /// </summary>
        public decimal? Descuento { get; set; }

        /// <summary>
        /// Propiedad requerida para identificar la clave de la moneda utilizada para
        /// expresar montos.
        /// </summary>
        public string Moneda { get; set; }

        /// <summary>
        /// Propiedad condicional para representar el tipo de cambio FIX confome con la
        /// modena usada.
        /// </summary>
        public decimal? TipoCambio { get; set; }

        /// <summary>
        /// Propiedad requerida para expresar la clave del efecto del comprobante fiscal
        /// para el contribuyente emisor.
        /// </summary>
        public string? TipoDeComprobante { get; set; }

        /// <summary>
        /// Propiedad requerida para representar la suma subtotal.
        /// </summary>
        public decimal Total { get; set; }

        /// <summary>
        /// Propiedad requerida para expresar si el comprobante ampara una operación de
        /// exportación.
        /// </summary>
        public string Exportacion { get; set; }

        /// <summary>
        /// Propiedad condicional para precisar la clave del método de pago
        /// </summary>
        public string? MetodoPago { get; set; }

        /// <summary>
        /// Propiedad requerida para incorporar el código postal del lugar de expedición
        /// del comprobante (domicilio de la matriz o de la sucursal).
        /// </summary>
        public string LugarExpedicion { get; set; }

        /// <summary>
        /// Propiedad Condicional para registar la clave de confirmación que entre el PAC
        /// para expedir el comprobante de importes grandes.
        /// </summary>
        public string? Confirmacion { get; set; }

        //NODOS

        /// <summary>
        /// Nodo condicional para precisar la información relacionada con el comprobante legal.
        /// </summary>
        public InformacionGlobal? InformacionGlobal { get; set; }
        /// <summary>
        /// Atributo requerido para indicar la clave de la relación que existe entre
        /// éste que se está genereando y el o los CFDI previos.
        /// </summary>
        public List<CfdiRelacionados>? CfdiRelacionados { get; set; }
        /// <summary>
        /// Nodo requerido para expresar la información del contribuyente emisor del comprobante.
        /// </summary>
        public Emisor Emisor { get; set; }
        /// <summary>
        /// Nodo requerido para precisar la información del contribuyente receptor del comprobante.
        /// </summary>
        public Receptor Receptor { get; set; }
        /// <summary>
        /// Nodo requerido para listar los conceptos cubiertos por el comprobante.
        /// </summary>
        public Conceptos Conceptos { get; set; }
        /// <summary>
        /// Nodo condicional para expresar el resumen de los impuestos aplicables.
        /// </summary>
        public Impuestos? Impuestos { get; set; }
        /// <summary>
        /// Nodo opcional donde se incluye el complemento Timbre Fiscal Digital de manera obligatoria y los nodos complementarios determinados por el SAT
        /// </summary>
        public Complemento? Complemento { get; set; }
        /// <summary>
        /// Nodo opcional para recibir las extensiones al presente formato que sean de utilidad al contribuyente.
        /// </summary>
        public Addenda? Addenda { get; set; }

        public Comprobante()
        {
            InformacionGlobal = new InformacionGlobal();
            CfdiRelacionados = new List<CfdiRelacionados>();
            Emisor = new Emisor();
            Receptor= new Receptor();
            Conceptos = new Conceptos();
            Impuestos = new Impuestos();
            Complemento = new Complemento();
            Addenda = new Addenda();    
            
        }


    }
}
