namespace modelo.sat.cfdi.v40
{
    /// <summary>
    /// Nodo opcional para registrar información del contribuyente Tercero, a cuenta del
    /// que se realiza la operación.
    /// </summary>
    public class ACuentaTerceros
    {
        /// <summary>
        /// Propiedad requerida para registrar la Clave RFC de Contribuyentes del con contribuyente
        /// Tercero, a cuenta del que se realiza la operación.
        /// </summary>
        public string RfcAcuentaTerceros { get; set; }
        /// <summary>
        /// Propiedad requerida para registrar el nombre, del contribuyente Tercero correspondiente
        /// con el Rfc, al cuenta del que se realiza la operación.
        /// </summary>
        public string NombreACuentaTerceros { get; set; }
        /// <summary>
        /// Propiedad requerida para incorporar la clave del régimen del contribuyente Tercero, a cuenta
        /// del que se realiza la operación.
        /// </summary>
        public string RegimenFiscalACuentaTerceros { get; set; }
        /// <summary>
        /// Propiedad requerida para incorporar el código postal del domicilio fiscal del Tercero, a cuenta
        /// del que se realiza la operación.
        /// </summary>
        public string DomicilioFiscalACuentaTerceros { get; set; }
    }
}
