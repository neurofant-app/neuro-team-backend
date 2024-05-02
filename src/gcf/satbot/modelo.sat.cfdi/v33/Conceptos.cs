
namespace modelo.sat.cfdi.v33
{/// <summary>
 /// Nodo requerido para listar los conceptos cubiertos por el comprobante.
 /// </summary>
    public class Conceptos
    {
        public List<Concepto> Concepto { get; set; }
        public Conceptos() { this.Concepto = new List<Concepto>();}
    }
}
