namespace sat.bot.comun;

/// <summary>
/// Ofrece servicios para la salvaguarda y recuepracion de archivos
/// </summary>
public interface IGestorArchivos
{
    /// <summary>
    /// Obtiene la ruta de acceso para lectura y escritura de la base de datos de SQLite
    /// </summary>
    /// <param name="rfc"></param>
    /// <param name="subscripcionId"></param>
    /// <param name="ruta">Ruta donde se espera la base de datos almacenada</param>
    /// <returns></returns>
    Task<string?> RutaRWDBSqlite(string rfc, string subscripcionId, string ruta);

    /// <summary>
    /// Almacena en el storage permanente la ultima version de SQLite para la conciliacion de un RFC
    /// </summary>
    /// <param name="rfc"></param>
    /// <param name="subscripcionId"></param>
    /// <param name="ruta"></param>
    /// <param name="version"></param>
    /// <returns></returns>
    Task<bool> AlmacenaDBSqlite(string rfc, string subscripcionId, string ruta, string version);

    /// <summary>
    /// Almacena un XML relacionado con un RFC y suscripcion
    /// </summary>
    /// <param name="rfc"></param>
    /// <param name="subscripcionId"></param>
    /// <param name="UUID"></param>
    /// <param name="xml"></param>
    /// <returns></returns>
    Task<bool> AlmacenaXML(string rfc, string subscripcionId, Guid UUID, byte[] xml);

    /// <summary>
    /// Almacena un XML relacionado con un RFC y suscripcion
    /// </summary>
    /// <param name="rfc"></param>
    /// <param name="subscripcionId"></param>
    /// <param name="UUID"></param>
    /// <param name="pdf"></param>
    /// <returns></returns>
    Task<bool> AlmacenaPDF(string rfc, string subscripcionId, Guid UUID, byte[] pdf);


    /// <summary>
    /// Obtiene un XML almacenado para un RFC y susbcripcion
    /// </summary>
    /// <param name="rfc"></param>
    /// <param name="subscripcionId"></param>
    /// <param name="UUID"></param>
    /// <returns></returns>
    Task<byte[]?> LeeXML(string rfc, string subscripcionId, Guid UUID);

    /// <summary>
    /// Obtiene un PDF almacenado para un RFC y susbcripcion
    /// </summary>
    /// <param name="rfc"></param>
    /// <param name="subscripcionId"></param>
    /// <param name="UUID"></param>
    /// <returns></returns>
    Task<byte[]?> LeePDF(string rfc, string subscripcionId, Guid UUID);


}
