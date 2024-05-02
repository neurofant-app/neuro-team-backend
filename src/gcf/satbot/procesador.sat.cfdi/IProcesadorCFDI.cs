using modelo.repositorio.cfdi;
using sat.bot.modelo;

namespace procesador.sat.cfdi
{
    public interface IProcesadorCFDI
    {
        /// <summary>
        /// Se procesa el XML para la obtencion
        /// de un CFDI ya sea v33 ó v40.
        /// </summary>
        /// <param name="xmlCFID"></param>
        /// <returns></returns>
        object? ProcesaXML(string xmlCFID, VersionCFDI? version);
        /// <summary>
        /// Obtiene la versión de un XML de CFDI en base al texto enviado
        /// </summary>
        /// <param name="xmlCFID">Cadena de texto conteniendo el XML</param>
        /// <returns>NoEsCFDI si no es un CFDI válido o la versión del CFDI</returns>
        VersionCFDI VersionCFDI(string xmlCFID);
        /// <summary>
        /// Devuelve los datos del CFDI para el repositorio a partor de un string que contiene el XML de un XFDI
        /// </summary>
        /// <param name="xmlCFID">String XML de un XFDI</param>
        /// <returns>Datos para almacenar en el repositorio o nulo si no es un CFDI</returns>
        CFDI? EntidadRepositorio(string xmlCFID);

        /// <summary>
        /// Obtiene en string el RFC del Emisor.
        /// </summary>
        /// <param name="xmlCFID"></param>
        /// <returns></returns>
        string DevuelveRFCEmisor(string xmlCFID);
        /// <summary>
        /// Obtiene en string el RFC del Receptor.
        /// </summary>
        /// <param name="xmlCFID"></param>
        /// <returns></returns>
        string DevuelveRFCReceptor(string xmlCFID);
        Task<CFDI> ProcesarEmitido(CfdiEmitido emitido);
        Task<CFDI> ProcesarRecibido(CfdiRecibido emitido);
        Task guardarCfdiUiEmitido(CfdiEmitido emitido);
        Task guardarCfdiUiRecibido(CfdiRecibido recibido);
        Task SaveChanges();
    }
}
