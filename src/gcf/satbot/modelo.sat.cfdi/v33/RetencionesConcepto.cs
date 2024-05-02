
namespace modelo.sat.cfdi.v33
{/// <summary>
 /// Nodo opcional para asentar los impuestos retenidos aplicables al presente concepto.
 /// </summary>
    public class RetencionesConcepto
    {

       public List<RetencionConcepto>? Retencion { get; set; }

        public RetencionesConcepto() { Retencion= new List<RetencionConcepto>(); }
    }
}
