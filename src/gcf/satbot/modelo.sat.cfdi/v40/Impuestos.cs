namespace modelo.sat.cfdi.v40
{
    /// <summary>
    /// Nodo condicional para expresar el resumen de los impuestos aplicables.
    /// </summary>
    public class Impuestos
    {
        /// <summary>
        /// Propiedad condicional para expresar el total de los impuestos retenidos que se desprenden de los conceptos expresados en el CFDI.
        /// </summary>
        public decimal? TotalImpuestosRetenidos { get; set; }

        /// <summary>
        /// Propiedad condicional para expresar el total de los impuestos trasladados que se desprenden de los conceptos expresados en el CFDI.
        /// </summary>
        public decimal? TotalImpuestosTrasladados { get; set; }

        /// <summary>
        /// Nodo condicional para capturar los impuestos retenidos aplicables.
        /// </summary>
        public Retenciones? Retenciones { get; set; }
        /// <summary>
        /// Nodo condicional para capturar los impuestos trasladados aplicables.
        /// </summary>
        public Traslados? Traslados { get; set; }

        public Impuestos()
        {
            Retenciones = new Retenciones();
            Traslados= new Traslados();
        }
    }
}

