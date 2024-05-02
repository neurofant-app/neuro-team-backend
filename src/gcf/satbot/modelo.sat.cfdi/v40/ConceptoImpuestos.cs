namespace modelo.sat.cfdi.v40
{
    /// <summary>
    /// Nodo condicional para captura los impuestos aplicables al presente concepto.
    /// </summary>
    public class ConceptoImpuestos
    {
        /// <summary>
        /// Nodo opcional para asentar los impuestos trasladados aplicables al presente concepto.
        /// </summary>
        public ConceptoTraslados? Traslados { get; set; }
        /// <summary>
        /// Nodo opcional para asentar los impuestos retenidos aplicables al presente concepto.
        /// </summary>
        public ConceptoRetenciones? Retenciones { get; set; }
    }
}
