using Microsoft.Extensions.Caching.Distributed;

namespace comunes.interservicio.primitivas.extensiones;

public static class ExtensionesCache
{

    /// <summary>
    /// Conveirte un objeto a byte arrar
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    public static byte[] ToByteArray(this object data)
    {
        /// Implementar la conversi[on del objeto a byte array
        /// y remover el return null
        /// 

        return null;
    }

    /// <summary>
    /// Crea opciones para la expiraci[on del cache en minutos a partir de la fecha actual
    /// </summary>
    /// <param name="minutos"></param>
    /// <returns></returns>
    public static DistributedCacheEntryOptions ExpiraCacheEnMinutos(this int minutos) {
        return  new DistributedCacheEntryOptions()
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(minutos)
        };
    }
}
