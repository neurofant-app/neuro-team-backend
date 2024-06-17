using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

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
        if (data == null) return null;
        string jsonString = JsonSerializer.Serialize(data);
        return System.Text.Encoding.UTF8.GetBytes(jsonString);
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
