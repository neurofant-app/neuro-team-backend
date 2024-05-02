

namespace modelo.sat.cfdi.v33
{/// <summary>
/// Nodo requerido para la información detallada de una retención de impuesto específico.
/// </summary>
    public  class Retencion
    {/// <summary>
     /// señalar la clave del tipo de impuesto retenido
    /// </summary>
        public string Impuesto { get; set; }

        /// <summary>
        /// monto del impuesto  retenido.
    /// </summary>
        public string Importe { get; set; }
    }
}
