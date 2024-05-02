namespace modelo.sat.cfdi.v40
{
    /// <summary>
    /// Nodo condicional para capturar los impuestos trasladados aplicables.
    /// </summary>
    public class Traslados
    {
        /// <summary>
        /// Nodo requerido para la información detallada de un traslado de impuesto específico.
        /// </summary>
        public List<Traslado> Traslado { get; set; }
        public Traslados() 
        {
            Traslado= new List<Traslado>();
        }
    }
}
