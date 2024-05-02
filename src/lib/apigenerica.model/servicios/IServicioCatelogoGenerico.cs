using apigenerica.model.modelos;

namespace apigenerica.model.servicios;

/// <summary>
/// Define los métodos comunes al servicio de catálogos genericos
/// </summary>
public interface IServicioCatalogoGenerico
{

    /// <summary>
    /// Establece el contexto de ejecución del usuario 
    /// </summary>
    /// <param name="contexto"></param>
    void EstableceContextoUsuario(ContextoUsuario? contexto);

    /// <summary>
    /// Obtiene el contexto de ejecución actual del usuario
    /// </summary>
    /// <returns></returns>
    ContextoUsuario? ObtieneContextoUsuario();

}
