namespace modelo.sat.cfdi.v33
{/// <summary>
 /// Nodo opcional para asentar los impuestos trasladados aplicables al presente concepto.
 /// </summary>
    public class TrasladosConcepto
    {
        public List<TrasladoConcepto> Traslado { get; set; }
        public TrasladosConcepto() { Traslado = new List<TrasladoConcepto>();}

    }
}
