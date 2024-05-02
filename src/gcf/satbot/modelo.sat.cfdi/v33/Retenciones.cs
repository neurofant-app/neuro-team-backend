

namespace modelo.sat.cfdi.v33
{/// <summary>
 /// Nodo condicional para capturar los impuestos retenidos aplicables. Es requerido cuando en
    //   los conceptos se registre algún impuesto retenido.
    /// </summary>
    public  class Retenciones
    {
        public List<Retencion> Retencion { get; set; }
        public Retenciones() {

            Retencion = new List<Retencion>();

        }
    }
}
