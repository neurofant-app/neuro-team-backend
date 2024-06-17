using comunes.primitivas.seguridad;

namespace apigenerica.primitivas.aplicacion;

public interface IProveedorAplicaciones
{
    /// <summary>
    /// Obtiene la lista de aplicaciones desde el contexto actual
    /// </summary>
    /// <returns></returns>
    Task<List<Aplicacion>> ObtieneApliaciones();
}
