namespace modelo.sat.cfdi.v40
{
    /// <summary>
    /// Nodo opcional para introducir la información aduanera aplicable cuando se trate de
    /// ventas de primera mano de mercancías importadas o se trate de operacones de comercio
    /// exterior con bienes o servicios
    /// </summary>
    public class InformacionAduanera
    {
        /// <summary>
        /// Propiedad requerida para expresar el no. del pedimento que ampara la importación del bien.
        /// </summary>
        public string NumeroPedimento { get; set; }


    }
}