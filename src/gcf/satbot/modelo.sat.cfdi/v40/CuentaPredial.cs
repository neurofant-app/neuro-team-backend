namespace modelo.sat.cfdi.v40
{
    /// <summary>
    /// Nodo opcional para asentar el no. de cuenta predial con el que fue registrado el inmueble.
    /// </summary>
    public class CuentaPredial
    {
        /// <summary>
        /// Propiedad requerida para precisar el número de la cuenta predial del inmueble cubierto
        /// por el presente concepto.
        /// </summary>
        public string Numero { get; set; }
    }
}
