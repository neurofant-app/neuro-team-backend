namespace modelo.sat.cfdi.v40
{
    public class ConceptoRetenciones
    {
        /// <summary>
        /// Lista de Nodos requeridos para asentar la información detallada de una retención de impuestos aplicables
        /// al presente concepto.
        /// </summary>
        public List<ConceptoRetencion> ConceptoRetencion { get; set; }
        public ConceptoRetenciones()
        {
            ConceptoRetencion = new List<ConceptoRetencion>();
        }
    }
}