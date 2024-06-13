using comunes.primitivas.seguridad;

namespace apigenerica.primitivas.aplicacion;

public interface IProveedorAplicaciones
{
    /// <summary>
    /// Obtiene la lista de apiicaciones desde el contexto actual
    /// </summary>
    /// <returns></returns>
    Task<List<Aplicacion>> ObtieneApliaciones();
}
