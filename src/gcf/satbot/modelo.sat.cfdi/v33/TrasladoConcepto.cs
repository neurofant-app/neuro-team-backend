
namespace modelo.sat.cfdi.v33
{/// <summary>
 /// Nodo requerido para asentar la información detallada de un traslado de impuestos aplicable al presente concepto.
/// </summary>
    public class TrasladoConcepto
    {
        /// <summary>
        /// base para el cálculo del impuesto, la determinación de la base se
        ///realiza de acuerdo con las disposiciones fiscales sigentes.
        /// </summary>
        public decimal Base { get; set; }

        /// <summary>
        /// clave del tipo de impuesto trasladado aplicable al concepto.
        /// </summary>
        public string Impuesto { get; set; }//catCFDI:c_Impuesto

        /// <summary>
        ///  clave del tipo de factor que se aplica a la base del impuesto.
        /// </summary>
        public string TipoFactor { get; set; }//catCFDI:c_TipoFactor

        /// <summary>
        /// tasa o cuota del impuesto que se traslada para el presente concepto.
        /// </summary>
        public decimal? TasaOCuota { get; set; }

        /// <summary>
        /// importe del  impuesto trasladado que aplica al concepto.
        /// </summary>
        public decimal? Importe { get; set; }//tdCFDI:t_Importe

    }
}
