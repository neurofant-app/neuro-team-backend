

namespace modelo.sat.cfdi.v33
{/// <summary>
 /// Nodo requerido para registrar la información detallada de un bien o servicio amparado en el comprobante.
/// </summary>
    public class Concepto
    {/// <summary>
     /// clave del producto o del servicio amparado por el presente concepto.
    /// </summary>
        public string ClaveProdServ { get; set; }//catCFDI:c_ClaveProdServ

        /// <summary>
        /// número de parte, identificador del producto o del servicio, la clave de producto o servicio, SKU o equivalente, propia de la
        ///operación del emisor, amparado por el presente concepto
        /// </summary>
        public string? NoIdentificacion { get; set; }

        /// <summary>
        /// cantidad de bienes o  servicios del tipo particular definido por el presente concepto
        /// </summary>
        public decimal Cantidad { get; set; }

        /// <summary>
        /// clave de unidad de   medida estandarizada aplicable para la cantidad expresada en el concepto.
        /// </summary>
        public string ClaveUnidad { get; set; }//catCFDI:c_ClaveUnidad

        /// <summary>
        /// unidad de medida propia  de la operación del emisor, aplicable para la cantidad expresada en el concepto.
        /// </summary>
        public string? Unidad { get; set; }

        /// <summary>
        /// descripción del bien o  servicio cubierto por el presente concepto
        /// </summary>
        public string Descripcion { get; set; }

        /// <summary>
        ///  valor o precio unitario  del bien o servicio cubierto por el presente concepto.
        /// </summary>
        public decimal ValorUnitario { get; set; }//t_Importe

        /// <summary>
        /// mporte total de los  bienes o servicios del presente concepto.
        /// </summary>
        public decimal Importe { get; set; }//t_Importe

        /// <summary>
        /// importe de los  descuentos aplicables al concepto.
        /// </summary>
        public decimal? Descuento { get; set; }//t_Importe


        public ImpuestosConcepto? Impuestos { get; set; }
        public List<InformacionAduaneraConcepto>? InformacionAduanera { get; set; }
        public CuentaPredial? CuentaPredial { get; set; }
        public ComplementoConcepto? ComplementoConcepto { get; set;}
        public List<Parte>? Parte { get; set; }

        public Concepto() 
        {
            Impuestos = new ImpuestosConcepto();
            InformacionAduanera = new List<InformacionAduaneraConcepto>();
            CuentaPredial = new CuentaPredial();
            Parte = new List<Parte>();
        
        }

    }
}
