using modelo.sat.cfdi.v40;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics.X86;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace modelo.sat.cfdi.v33
{/// <summary>
 /// Complemento requerido para el Timbrado Fiscal Digital que da validez al 
 /// Comprobante fiscal digital a través de Internet que ampara retenciones e
 //  información de pagos
/// </summary>
    public  class TimbreFiscalDigital
    {   /// <summary>
        /// Versión del estándar del Timbre Fiscal Digital
        /// </summary>
        public string Version { get; set; }
        /// <summary>
        /// Atributo requerido para expresar los 36 caracteres del folio fiscal(UUID) de la transacción de timbrado conforme al
        /// estándar RFC 4122
        /// </summary>
        public string UUID { get; set; }
        /// <summary>
        /// Fecha y hora de la generación del timbre por la certificación digital del SAT. Se expresa en la forma aaaa-mm-ddThh:mm:ss,
        /// </summary>
        public DateTime FechaTimbrado { get; set;}
        /// <summary>
        /// Sello digital del comprobante fiscal, que será timbrado
        /// </summary>
        public string selloCFD { get; set; }
        /// <summary>
        ///  Número de serie del certificado del SAT usado para generar el sello digital del Timbre Fiscal Digital
        /// </summary>
        public string oCertificadoSAT { get; set; }
        /// <summary>
        ///  Sello digital del Timbre Fiscal Digital, al que hacen referencia las reglas de resolución miscelánea aplicable.
        /// </summary>
        public string selloSAT { get; set; }

    }
}
