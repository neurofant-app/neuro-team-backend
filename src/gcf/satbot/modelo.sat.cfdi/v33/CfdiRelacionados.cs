

namespace modelo.sat.cfdi.v33
{
    /// <summary>
    /// Nodo opcional para precisar la información de los comprobantes relacionados.
    /// </summary>
    public class CfdiRelacionados
    {
        /// <summary>
        /// lave de la relación que  existe entre éste que se esta generando y el o los CFDI previos
        /// </summary>
        public string TipoRelacion { get; set; }//catCFDI:c_TipoRelacion
        public List<CfdiRelacionado> CfdiRelacionado { get; set; }
        public CfdiRelacionados () { CfdiRelacionado = new List<CfdiRelacionado>(); }
    }
}
