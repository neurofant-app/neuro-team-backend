

namespace modelo.sat.cfdi.v33
{/// <summary>
 /// Nodo opcional para capturar los impuestos aplicables al presente concepto. Cuando un
 ///   concepto no registra un impuesto, implica que no es objeto del mismo.
 /// </summary>
    public class ImpuestosConcepto
    {
        public TrasladosConcepto? Traslados { get; set; }
        public RetencionesConcepto? Retenciones { get; set; }

        public ImpuestosConcepto()
        {
            Traslados = new TrasladosConcepto();
            Retenciones = new RetencionesConcepto();
        }
    }
}
