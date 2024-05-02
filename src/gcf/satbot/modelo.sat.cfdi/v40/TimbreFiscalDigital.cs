using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace modelo.sat.cfdi.v40
{
    public  class TimbreFiscalDigital
    {
        /// <summary>
        /// Propiedad requerida para TFD que da validez al CDFIl
        /// </summary>
        public string Version { get; set; } = "1.1";
        /// <summary>
        /// Atributo requerido para expresar los 36 caracteres del 
        /// folio fiscal (UUID) de la transacción de timbrado conforme }
        /// al estándar RFC 4122
        /// </summary>
        public string UUID { get; set; }
        /// <summary>
        /// Propiedad que expresa la fecha y hora, de la generación del
        /// timbre por la certificación digital del SAT.
        /// </summary>
        public string FechaTImbrado { get; set; }
        /// <summary>
        /// Atributo requerido para expresar el RFC del proveedor de certificación 
        /// de comprobantes fiscales digitales que genera el timbre fiscal digital.
        /// </summary>
        public string RfcProvCertif{ get; set; }
        /// <summary>
        /// Propiedad opcional para registrar información que el SAT comunique a los usuarios del CFDI.
        /// </summary>
        public string? Leyenda { get; set; }
        /// <summary>
        /// Atributo requerido para contener el sello digital del comprobante fiscal o del comprobante 
        /// de retenciones, que se ha timbrado.
        /// </summary>
        public string SelloCFD { get; set; }
        /// <summary>
        /// Atributo requerido para expresar el número de serie del certificado del SAT usado para generar
        /// el sello digital del Timbre Fiscal Digital.
        /// </summary>
        public string NoCertificadoSAT { get; set; }
        /// <summary>
        /// Atributo requerido para contener el sello digital del Timbre Fiscal Digital, al que hacen referencia las reglas de la Resolución Miscelánea vigente.
        /// </summary>
        public string SelloSAT { get; set; }

    }
}
