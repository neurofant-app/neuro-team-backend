using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace modelo.sat.cfdi.v40
{
    /// <summary>
    /// Nodo Opcional para expresar las partes o componentes que integran la totalidad del concepto expresado
    /// en el CFDI.
    /// </summary>
    public class InformacionAduaneraParte
    {
        /// <summary>
        /// Propiedad requerida para expresar el pedimento que ampara la importación del bien.
        /// </summary>
        public string NumeroPedimento { get; set; }
    }
}