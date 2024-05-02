
namespace modelo.sat.cfdi.v33
{/// <summary>
 /// Nodo condicional para capturar los impuestos trasladados aplicables. Es requerido
 ///   cuando en los conceptos se registre un impuesto trasladado.
/// </summary>
    public class Traslados
    {
        public List<Traslado> Traslado { get; set; }
        public Traslados() { Traslado = new List<Traslado>(); }
    }
}
