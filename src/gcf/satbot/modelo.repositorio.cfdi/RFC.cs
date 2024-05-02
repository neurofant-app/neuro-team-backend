using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace modelo.repositorio.cfdi
{
    public class RFC
    {
        /// <summary>
        /// Identificador único del registro para la tabla RFC en sqlite
        /// </summary>
        public long rowid { get; set; }

        /// <summary>
        /// Cadena del Regitro Único de Contribuyentes generado por el SAT
        /// </summary>
        public string Rfc { get; set; }//string(15)
        
        /// <summary>
        /// Nombre o razon social del RFC
        /// de llave primaria
        /// </summary>
        public string Nombre { get; set; } //string (250)
        
        public List<CFDI> Cfdis { get; set; }
        //public List<Resumen> Resumenes { get; set; }
        //public List<Estadistica> Estadisticas { get; set; }
        //public List<Bitacora> Bitacoras { get; set; }



    }
}
