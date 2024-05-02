namespace modelo.sat.cfdi.v40
{
    /// <summary>
    /// Nodo opcional para precisar la informacion de los comprobantes relacionados.
    /// </summary>
    public class CfdiRelacionados
    {
        //PROPIEDADES

        /// <summary>
        /// Atributo requerido para indicar la clave de la relación que existe entre
        /// éste que se está genereando y el o los CFDI previos.
        /// </summary>
        public string TipoRelacion { get; set; }


        //NODOS
        /// <summary>
        /// Nodo requerido para precisar la información de los comprobantes relacionados.
        /// </summary>
        public List<CfdiRelacionado> CfdiRelacionado { get; set; }

        public CfdiRelacionados()
        {
            CfdiRelacionado = new List<CfdiRelacionado>();
        }

    }
}