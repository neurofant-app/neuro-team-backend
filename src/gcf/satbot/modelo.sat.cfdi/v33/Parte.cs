
namespace modelo.sat.cfdi.v33
{/// <summary>
 /// Nodo opcional para expresar las partes o componentes que integran la totalidad del
 /// concepto expresado en el comprobante fiscal digital por Internet.
/// </summary>
    public class Parte
    {/// <summary>
     /// clave del producto o del servicio amparado por la presente parte.
    /// </summary>
        public string ClaveProdServ { get; set; }//catCFDI:c_ClaveProdServ

        /// <summary>
        /// número de serie, número de parte del bien o identificador del producto o del servicio
        /// amparado por la presente parte.
        /// </summary>
        public string? NoIdentificacion { get; set; }

        /// <summary>
        /// cantidad de bienes o servicios del tipo particular definido por la presente parte.
        /// </summary>
        public decimal Cantidad { get; set; }

        /// <summary>
        /// unidad de medida propia de la operación del emisor, aplicable para la cantidad expresada en la parte.
        /// </summary>
        public string? Unidad { get; set; }

        /// <summary>
        ///  descripción del bien o  servicio cubierto por la presente parte.
        /// </summary>
        public string Descripcion { get; set; }

        /// <summary>
        /// valor o precio unitario del bien o servicio cubierto por la presente parte.
        /// </summary>
        public decimal? ValorUnitario { get; set; }//t_Importe

        /// <summary>
        ///  importe total de los bienes o servicios de la presente parte.
        /// </summary>
        public decimal? Importe { get; set; }//t_Importe


        public List<InformacionAduaneraParte>? InformacionAduanera { get; set; }
        public Parte()
        {
            InformacionAduanera = new List<InformacionAduaneraParte>();
        }
    }
}
