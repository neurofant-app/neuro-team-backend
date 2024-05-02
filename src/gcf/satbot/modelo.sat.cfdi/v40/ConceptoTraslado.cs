namespace modelo.sat.cfdi.v40
{
    /// <summary>
    /// Nodo requerido para asentar la información detallada de un traslado de impuestos aplicables al presente concepto.
    /// </summary>
    public class ConceptoTraslado
    {
        /// <summary>
        /// Propiedad requerida para señalar la base para el cálculo del impuesto.
        /// </summary>
        public decimal Base { get; set; }
        /// <summary>
        /// Propiedad requerida para señalar la clave del tipo de impuesto trasladado aplicable al concepto.
        /// </summary>
        public string Impuesto { get; set; }
        /// <summary>
        /// Propiedad requerida para señalar la clave del tipo de factor que se aplica a la base del impuesto.
        /// </summary>
        public string TipoFactor { get; set; }
        /// <summary>
        /// Propiedad condicional para señalar el valor de la tasa o cuota del impuesto que se traslada para el
        /// presente concepto.
        /// </summary>
        public decimal? TasaOCuota { get; set; }
        /// <summary>
        /// Propiedad condicional para señalar el importe del impuesto traslaldado que aplica al concepto.
        /// </summary>
        public decimal? Importe { get; set; }

    }
}
