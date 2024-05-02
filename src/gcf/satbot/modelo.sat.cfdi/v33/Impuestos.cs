
namespace modelo.sat.cfdi.v33
{/// <summary>
 /// Nodo condicional para expresar el resumen de los impuestos aplicables.
 /// </summary>
    public class Impuestos
    {   /// <summary>
        /// total de los impuestos retenidos que se desprenden de los conceptos expresados
        ///en el comprobante fiscal digital por Internet.
        /// </summary>
        public decimal? TotalImpuestosRetenidos { get; set; }//t_importe
        /// <summary>
        /// total de los impuestos trasladados que se desprenden de los conceptos expresados en el comprobante
        /// </summary>
        public decimal? TotalImpuestosTrasladados { get; set; }//t_importe
        public Traslados? Traslados { get; set; }
        public Retenciones? Retenciones { get; set; }
        public Impuestos() { 
        
            Traslados = new Traslados();
            Retenciones = new Retenciones();
        }

    }
}
