using apigenerica.model.modelos;

namespace apigenerica.primitivas.seguridad;

public interface ICacheAtributos
{
    /// <summary>
    /// Obtiene los roles y permisos de cada metodo de un servicio 
    /// </summary>
    /// <param name="tipoServicio"></param>
    /// <returns></returns>
    Task<List<AtributosMetodo>> AtributosServicio(Type tipoServicio);

}
