namespace modelo.sat.cfdi.v40
{
    /// <summary>
    /// Nodo opcional para expresar las partes o componentes que integran la totalidad del concepto expresado en
    /// el CFDI.
    /// </summary>
    public class Parte
    {
        /// <summary>
        /// Propiedad requerida para expresar la clave del producto o del servicio amparado por la
        /// presente parte.
        /// </summary>
        public string ClaveProdServ { get; set; }
        /// <summary>
        /// Propiedad opcional para expresar el no. de serie o del servicio amparado por la presente
        /// parte.
        /// </summary>
        public string? NoIdentificacion { get; set; }
        /// <summary>
        /// Propiedad requerida para precisar la cantidad de bienes o servicios del tipo particular 
        /// definido por la presente parte.
        /// </summary>
        public decimal Cantidad { get; set; }
        /// <summary>
        /// Propiedad opcional para precisar la unidad de medida propia de la operación del emisor,
        /// aplicable para la cantidad expresada en la parte.
        /// </summary>
        public string? Unidad { get; set; }
        /// <summary>
        /// Propiedad requerido para precisar la descripción del bien o servicio cubierto por la presente parte.
        /// </summary>
        public string Descripcion { get; set; }
        /// <summary>
        /// Propiedad opcional para precisar el valor o precio unitario del bien o servicio cubierto por la presente parte. 
        /// </summary>
        public decimal? ValorUnitario { get; set; }
        /// <summary>
        /// Propiedad opcional para precisar el importe total de los bienes o servicios de la presente parte.
        /// </summary>
        public decimal? Importe { get; set; }


        /// <summary>
        /// Nodo Opcional para expresar las partes o componentes que integran la totalidad del concepto expresado
        /// en el CFDI.
        /// </summary>
        public List<InformacionAduaneraParte>? InformacionAduaneraParte { get; set; }

        public Parte()
        {
            InformacionAduaneraParte = new List<InformacionAduaneraParte> ();
        }
    }
}
