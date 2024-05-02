namespace modelo.sat.cfdi.v40
{
    /// <summary>
    /// Nodo condicional para capturar los impuestos retenidos aplicables.
    /// </summary>
    public class Retenciones
    {
        /// <summary>
        /// Nodo requerido para la información detallada de una retención de impuesto específico.
        /// </summary>
        public List<Retencion> Retencion { get; set; }

        public Retenciones() {
            Retencion= new List<Retencion>();

        }
    }
}
