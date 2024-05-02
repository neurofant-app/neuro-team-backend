
namespace modelo.sat.cfdi.v33
{/// <summary>
/// Nodo requerido para precisar la información de los comprobantes relacionados.
/// </summary>
    public class CfdiRelacionado
    {
        /// <summary>
        /// folio fiscal (UUID) de un CFDI relacionado con el presente comprobante.
        /// </summary>
        public string UUID { get; set; }
    }
}
