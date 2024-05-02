namespace modelo.sat.cfdi.v40
{
    public class Retencion
    {
        /// <summary>
        /// Propiedad requerido para señalar la clave del tipo de impuesto retenido.
        /// </summary>
        public string Impuesto { get; set; }
        /// <summary>
        /// Propiedad requerido para señalar el monto del impuesto retenido.
        /// </summary>
        public decimal Importe { get; set; }
    }
}
