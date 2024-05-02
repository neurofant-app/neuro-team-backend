namespace modelo.sat.cfdi.v40

{
    /// <summary>
    /// Nodo requerido para expresar la información del contribuyente emisor del comprobante
    /// </summary>
    public class Emisor
    {
        /// <summary>
        /// Propiedad requerido para registrar la Clave del RFC correspondiente al contribuyente
        /// emisor del comprobante.
        /// </summary>
        public string Rfc { get; set; }

        /// <summary>
        /// Propiedad requerida para registrar el nombre, denominacion o razón social del contribuyente
        /// inscrito en el RFC, del emisor del comprobante.
        /// </summary>
        public string Nombre { get; set; }

        /// <summary>
        /// Propiedad requerido para incorporar la clave del régimen del contribuyente emisor al que aplicará
        /// el efecto fiscal de este comprobante    
        /// </summary>
        public string RegimenFiscal { get; set; }

        /// <summary>
        /// Propiedad condicional para expresar el no. de operación proporcionado por el SAT.
        /// </summary>
        public string? FacAtrAdquirente { get; set; }
    }
}