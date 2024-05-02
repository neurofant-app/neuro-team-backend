namespace modelo.sat.cfdi.v40
{
    /// <summary>
    /// Nodo requerido para listar los conceptos cubiertos por el comprobante.
    /// </summary>
    public class Conceptos
    {
        /// <summary>
        /// Nodo requerido para registrar la información detallada de un bien o servicio amparado
        /// en el comprobante.
        /// </summary>
        public List<Concepto> Concepto { get; set; }
    }
}