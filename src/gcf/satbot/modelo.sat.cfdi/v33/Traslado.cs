

namespace modelo.sat.cfdi.v33
{/// <summary>
 /// Nodo requerido para la información detallada de un traslado de impuesto específico.
 /// </summary>
    public class Traslado
        {/// <summary>
         ///  clave del tipo de impuesto trasladado.
        /// </summary>
        public string Impuesto { get; set; }
        /// <summary>
        ///  clave del tipo de factor que se aplica a la base del impuesto.
        /// </summary>
        public string TipoFactor { get; set; }

        /// <summary>
        /// valor de la tasa o cuota del impuesto que se traslada por los conceptos amparados
        /// </summary>
        public decimal TasaOCuota { get; set; }

        /// <summary>
        /// suma del importe del impuesto trasladado, agrupado por impuesto, TipoFactor y  TasaOCuota.
        /// </summary>
        public decimal Importe { get; set; }
    }
}
