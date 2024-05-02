namespace modelo.sat.cfdi.v40
{
    /// <summary>
    /// Nodo condicional para precisar la información relacionada con el comprobante legal.
    /// </summary>
    public class InformacionGlobal
    {
        //Propiedades

        /// <summary>
        /// Propiedad requerida para expresar el periodo al que corresponde la información
        /// del comprobante global.
        /// </summary>
        public string Periodicidad { get; set; }

        /// <summary>
        /// Propiedad requerida para expresar el mes o los meses al que corresponde la info-
        /// del comprobante global.
        /// </summary>
        public string Meses { get; set; }

        /// <summary>
        /// Propiedad requerida para expresar el año al que corresponde la información comprobante global.
        /// </summary>
        public string año { get; set; }
    }
}

