
namespace modelo.sat.cfdi.v33
{/// <summary>
 /// Nodo requerido para expresar la información del contribuyente emisor del comprobante.
 /// </summary>
    public class Emisor
    {   /// <summary>
        /// Clave del Registro Federal de Contribuyentes correspondiente al contribuyente emisor del comprobante.
        /// </summary>
        public string Rfc { get; set; }//tdCFDI:t_RFC
        /// <summary>
        /// nombre, denominación o razón social del contribuyente emisor del comprobante.
        /// </summary>
        public string? Nombre { get; set; }
        /// <summary>
        /// clave del régimen del contribuyente emisor al que aplicará el efecto fiscal de este comprobante.
        /// </summary>
        public string RegimenFiscal { get; set; }//catCFDI:c_RegimenFiscal


    }
}
