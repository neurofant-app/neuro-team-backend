namespace modelo.sat.cfdi.v40
{
    public class ConceptoRetencion
    {
        /// <summary>
        /// Propiedad requerida para señalar la base para el cálculo de la retención, la determinacion
        /// de la base se realiza de acuerdo con las disposiciones fiscales vigentes.
        /// </summary>
        public decimal Base { get; set; }
        /// <summary>
        /// Propiedad requerida para señalar la clave del tipo de impuesto retenido aplicable al concepto.
        /// </summary>
        public decimal Impuesto { get; set; }
        /// <summary>
        /// Propiedad requerida para señalar la clave del tipo de factor que se aplica a la base del impuesto.
        /// </summary>
        public string TipoFactor { get; set; }
        /// <summary>
        /// Propiedad requerida para señalar la tasa o cuota del impuesto que se retiene para el presente concepto.
        /// </summary>
        public decimal TasaOCuota { get; set; }
        /// <summary>
        /// Propiedad requerida para señalar el importe del impuesto retenido que aplica al concepto.
        /// </summary>
        public decimal Importe { get; set; }

    }
}

