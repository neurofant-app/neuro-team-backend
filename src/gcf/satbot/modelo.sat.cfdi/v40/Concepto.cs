namespace modelo.sat.cfdi.v40
{
    /// <summary>
    /// Nodo requerido para registrar la información detallada de un bien o servicio amparado
    /// en el comprobante.
    /// </summary>
    public class Concepto
    {
        /// <summary>
        /// Propiedad requerida para expresar la clave del producto o del servicio amparado por
        /// el presente concepto.
        /// </summary>
        public string ClaveProdServ { get; set; }

        /// <summary>
        /// Propiedad opcional para expresar el no. de parte, propieda del emisor, amparado por
        /// el presente concepto.
        /// </summary>
        public string? NoIdentificacion { get; set; }

        /// <summary>
        /// Propiedad requerida para precisar la cantidad de bienes o servicios dle tipo particu-
        /// lar definido por el pregente concepto.
        /// </summary>
        public decimal Cantidad { get; set; }

        /// <summary>
        /// Propiedad requerida para precisar la clave de unidad medida estandarizada, aplicable
        /// para la cantidad expresada en el conceptor.
        /// </summary>
        public string ClaveUnidad { get; set; }

        /// <summary>
        /// Propiedad opcional para precisar la unidad de medida propia de la operación del emisor
        /// aplicables para la canitdad expresada en el concepto.
        /// </summary>
        public string? Unidad { get; set; }


        /// <summary>
        /// Propiedad requerida para precisar la descripción del bien o servicio cubierto por el
        /// presente concepto.
        /// </summary>
        public string Descripcion { get; set; }

        /// <summary>
        /// Propiedad requerida para precisar el valor o precio unitario del bien o servicio cubierto
        /// por el presente concepto.
        /// </summary>
        public decimal ValorUnitario { get; set; }

        /// <summary>
        /// Propiedad requerida para precisar el importe total de los bienes o servicios del presente concepto.
        /// </summary>
        public decimal Importe { get; set; }

        /// <summary>
        /// Propiedad opcional para representar el importe de los descuentos aplicables al concepto.
        /// </summary>
        public decimal? Descuento { get; set; }

        /// <summary>
        /// Propiedad requerida para expresar si la operación comercial es objeto o no de impuesto.
        /// </summary>
        public string ObtejoImp { get; set; }
        /// <summary>
        /// Nodo condicional para capturar los impuestos aplicables al presente concepto.
        /// </summary>

        //NODOS

        /// <summary>
        /// Nodo condicional para captura los impuestos aplicables al presente concepto.
        /// </summary>
        public ConceptoImpuestos? ConceptoImpuestos { get; set; }
        /// <summary>
        /// Nodo opcional para registrar la información del contribuyente Tercero, a cuenta
        /// del que se realiza la operación.
        /// </summary>
        public ACuentaTerceros? ACuentaTerceros { get; set; }
        /// <summary>
        /// Nodo opcional para introducir la información aduanera aplicable cuando se trate de
        /// ventas de primera mano de mercancías importadas o se trate de operacones de comercio
        /// exterior con bienes o servicios.
        /// </summary>
        public List<InformacionAduanera>? InformacionAduanera { get; set; }
        /// <summary>
        /// Nodo opcional para asentar el no. de cuenta predial con el que fue registrado el inmueble.
        /// </summary>
        public List<CuentaPredial>? CuentaPredial { get; set; }
        /// <summary>
        /// Nodo opcional donde se incluyen los nodos complementarios de extensión al concepto definidos por el SAT.
        /// </summary>
        public ComplementoConcepto? ComplementoConcepto { get; set; }
        /// <summary>
        /// Nodo opcional para expresar las partes o componentes que integran la totalidad del concepto expresado en
        /// el CFDI.
        /// </summary>
        public List<Parte>? Parte { get; set; }

        public Concepto()
        {
            ConceptoImpuestos = new ConceptoImpuestos();
            ACuentaTerceros = new ACuentaTerceros();
            InformacionAduanera = new List<InformacionAduanera>();
            CuentaPredial = new List<CuentaPredial>();
            ComplementoConcepto = new ComplementoConcepto();
            Parte = new List<Parte>();
        }
    }
}