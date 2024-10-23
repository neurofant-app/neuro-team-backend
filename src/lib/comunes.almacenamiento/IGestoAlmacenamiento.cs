using comunes.primitivas;

namespace comunes.almacenamiento;

/// <summary>
/// Define un servicio para leer y escribir en el almacenamiento
/// </summary>
public interface IGestoAlmacenamiento
{
    /// <summary>
    /// Genera la ruta en el almacenamiento para un objeto a persistir
    /// </summary>
    /// <param name="almacenamiento"></param>
    /// <param name="objeto"></param>
    /// <returns></returns>
    Task<RespuestaPayload<string>> Ruta(Almacenamiento almacenamiento, object objeto);

    /// <summary>
    /// Obtiene los bytes asociados a una entidad almacenada
    /// </summary>
    /// <param name="almacenamiento"></param>
    /// <param name="objeto"></param>
    /// <returns></returns>
    Task<RespuestaPayload<byte[]>> LeeBytes(Almacenamiento almacenamiento, object objeto);

    /// <summary>
    /// Devuelve la ruta al escribir los bytes de la entidad en el sistema de almacenamiento
    /// </summary>
    /// <param name="almacenamiento"></param>
    /// <param name="objeto"></param>
    /// <param name="bytes"></param>
    /// <param name="sobrescribir"></param>
    /// <returns></returns>
    Task<RespuestaPayload<string>> EscribeBytes(Almacenamiento almacenamiento, object objeto, byte[] bytes, bool sobrescribir);
}
