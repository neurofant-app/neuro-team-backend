namespace modelo.sat.cfdi.v33
{/// <summary>
/// Nodo requerido para precisar la información del contribuyente receptor del comprobante.
/// </summary>
    public class Receptor
    {/// <summary>
     /// Clave del Registro Federal de Contribuyentes correspondiente al contribuyente receptor del comprobante.
     /// </summary>
        public string Rfc { get; set; }//tdCFDI:t_RFC
        /// <summary>
        /// l nombre, denominación  o razón social del contribuyente receptor del   comprobante
        /// </summary>
        public string? Nombre { get; set; }
        /// <summary>
        /// clave del país de  residencia para efectos fiscales del receptor del  comprobante
        /// </summary>
        public string? ResidenciaFiscal { get; set; }//catCFDI:c_Pais
        /// <summary>
        /// número de registro  de identidad fiscal del receptor cuando sea residente en  el extranjero.
        /// </summary>
        public string? NumRegIdTrib { get; set; }
        /// <summary>
        /// clave del uso que  dará a esta factura el receptor del CFDI.
        /// </summary>
        public string UsoCFDI { get; set; }//catCFDI:c_UsoCFDI
    }
}
