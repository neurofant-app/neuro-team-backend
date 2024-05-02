namespace modelo.sat.cfdi.v40
{
    /// <summary>
    /// Nodo requerido para la información detallada de un traslado de impuesto específico.
    /// </summary>
    public class Traslado
    {
        /// <summary>
        /// Propiedad requerido para señalar la suma de los atributos Base de los conceptos del impuesto trasladado.
        /// </summary>
        public decimal Base { get; set; }
        /// <summary>
        /// Propiedad requerido para señalar la clave del tipo de impuesto trasladado.
        /// </summary>
        public string Impuesto { get; set; }
        /// <summary>
        /// Propiedad requerido para señalar la clave del tipo de factor que se aplica a la base del impuesto.
        /// </summary>
        public string TipoFactor { get; set; }
        /// <summary>
        /// Atributo condicional para señalar el valor de la tasa o cuota del impuesto que se traslada por los 
        /// conceptos amparados en el comprobante.
        /// </summary>
        public decimal? TasaOCuota { get; set; }
        /// <summary>
        /// Propiedad condicional para señalar la suma del importe del impuesto trasladado.
        /// </summary>
        public decimal? Importe { get; set; }
    }
}
