namespace modelo.sat.cfdi.v40
{
    /// <summary>
    /// Nodo requerido para precisar la info. del contribuyente receptor del comprobante.
    /// </summary>
    public class Receptor
    {
        /// <summary>
        /// Propiedad requerida para regitrar el RFC, correspondiente al contribuyente
        /// receptor del comprobante.
        /// </summary>
        public string Rfc { get; set; }

        /// <summary>
        /// Propiedad requerida para registar el nombre del contribuyente, inscritoen el RFC,
        /// del receptor del comprobante.
        /// </summary>
        public string Nombre { get; set; }

        /// <summary>
        /// Propiedad requerida para registrar el código postal del domicilio fiscal del receptor
        /// del comprobante.
        /// </summary>
        public string DomicilioFiscalReceptor { get; set; }

        /// <summary>
        /// Propiedad condicional para registrar la clave del pais de residencia para efectos fiscales
        /// del receptor del comprobante.
        /// </summary>
        public string? ResidenciaFiscal { get; set; }

        /// <summary>
        /// Propiedad condicional para expresar el número de registro de identidad fiscal del receptor
        /// cuando sea residente en el entranjero.
        /// </summary>
        public string? NumRegIdTrib { get; set; }

        /// <summary>
        /// Propiedad requerida para incorporar la clave del régimen fiscal del contribuyente receptor
        /// al que aplicará el efecto fiscal de este comprobante.
        /// </summary>
        public string RegimenFiscalReceptor { get; set; }

        /// <summary>
        /// Propiedad requerida para expresar la clave del uso que dará a esta factura el receptor del
        /// CFDI.
        /// </summary>
        public string UsoCFDI { get; set; }
    }
}
