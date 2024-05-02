namespace modelo.sat.cfdi.v40
{
    /// <summary>
    /// Nodo opcional para asentar los impuestos trasladados aplicables al presente concepto.
    /// </summary>
    public class ConceptoTraslados
    {
        /// <summary>
        /// Lista de Nodos requerido para asentar la información detallada de un traslado de impuestos aplicables al presente concepto.
        /// </summary>
        public List<ConceptoTraslado> ConceptoTraslado { get; set; }

        public ConceptoTraslados()
        {
            ConceptoTraslado = new List<ConceptoTraslado>();
        }
    }
}
