namespace modelo.sat.cfdi.v40
{
    /// <summary>
    /// Nodo requerido para precisar la informacion de los comprobantes relacionados.
    /// </summary>
    public class CfdiRelacionado
    {
        /// <summary>
        /// Propiedad requerida para registrar el folio fiscal (UUID) de un CFDI relacionado
        /// con el presente comprobante.
        /// </summary>
        public string UUID { get; set; }


    }
}
