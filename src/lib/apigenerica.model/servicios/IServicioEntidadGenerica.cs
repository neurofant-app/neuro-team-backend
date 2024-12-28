using extensibilidad.metadatos;
using apigenerica.model.modelos;
using comunes.primitivas;
using System.Collections.Specialized;

namespace apigenerica.model.servicios;

/// <summary>
/// Define los métodos comunes al servicio de entidades genpericas
/// </summary>
/// <typeparam name="Store"></typeparam>
/// <typeparam name="DTOInsert"></typeparam>
/// <typeparam name="DTOUpdate"></typeparam>
/// <typeparam name="TipoId"></typeparam>
public interface IServicioEntidadGenerica<DTOFull, DTOInsert, DTOUpdate, DTODespliegue, TipoId>
    where DTOFull : class
    where DTODespliegue : class
    where DTOUpdate : class
    where DTOInsert : class
{

    /// <summary>
    /// Devuelve los metadatos de la entidad completa tal como se almacena en el repositorio
    /// </summary>
    /// <returns></returns>
    Entidad EntidadRepo();

    /// <summary>
    /// Devuelve los datos de la entidad para realizar la inserción al repositorio
    /// </summary>
    /// <returns></returns>
    Entidad EntidadInsert();


    /// <summary>
    /// Devuelve los metadatos de la entidad para realizar la actualización al repositorio
    /// </summary>
    /// <returns></returns>
    Entidad EntidadUpdate();

    /// <summary>
    /// Devuelve los metadatos de la entidad para el despliegue
    /// </summary>
    /// <returns></returns>
    Entidad EntidadDespliegue();


    /// <summary>
    /// Establece el contexto de ejecución del usuario 
    /// </summary>
    /// <param name="contexto"></param>
    void EstableceContextoUsuario(ContextoUsuario contexto);


    /// <summary>
    /// Obtiene el contexto de ejecución actual del usuario
    /// </summary>
    /// <returns></returns>
    ContextoUsuario? ObtieneContextoUsuario();

    /// <summary>
    /// Método para insertar una entidad nueva en el repositorio
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    Task<RespuestaPayload<DTODespliegue>> Insertar(DTOInsert data, StringDictionary? parametros =null);


    /// <summary>
    /// Actualiza el contenido de una entidad en el repositorio 
    /// </summary>
    /// <param name="id"></param>
    /// <param name="data"></param>
    /// <returns></returns>
    Task<Respuesta> Actualizar(TipoId id, DTOUpdate data, StringDictionary? parametros = null);


    /// <summary>
    /// Elimina una entidad del repositorio eb base a su Id único
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<Respuesta> Eliminar(TipoId id, StringDictionary? parametros = null, bool forzarEliminacion = false);


    /// <summary>
    /// Obtiene una entidad del repositorio por Id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<RespuestaPayload<DTOFull>> UnicaPorId(TipoId id, StringDictionary? parametros = null);

    /// <summary>
    /// Obtiene una entidad para despliegue del repositorio por Id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<RespuestaPayload<DTODespliegue>> UnicaPorIdDespliegue(TipoId id, StringDictionary? parametros = null);


    /// <summary>
    /// Obtiene una lista de elementos en base a la configuración de la consulta y su paginado
    /// </summary>
    /// <param name="consulta"></param>
    /// <returns></returns>
    Task<RespuestaPayload<PaginaGenerica<DTOFull>>> Pagina(Consulta consulta, StringDictionary? parametros = null);

    /// <summary>
    /// Obtiene una lista de elementos para el despliegue en base a la configuración de la consulta y su paginado
    /// </summary>
    /// <param name="consulta"></param>
    /// <returns></returns>
    Task<RespuestaPayload<PaginaGenerica<DTODespliegue>>> PaginaDespliegue(Consulta consulta, StringDictionary? parametros = null);


    /// <summary>
    /// Ejecuta la validación para un proceso de inserción
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    Task<ResultadoValidacion> ValidarInsertar(DTOInsert data);


    /// <summary>
    /// Ejecuta la validación para un proceso de actiualización
    /// </summary>
    /// <param name="id"></param>
    /// <param name="data"></param>
    /// <returns></returns>
    Task<ResultadoValidacion> ValidarActualizar(TipoId id, DTOUpdate actualizacion, DTOFull original);


    /// <summary>
    /// Ejecuta la validación para un proceso de eliminación
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<ResultadoValidacion> ValidarEliminacion(TipoId id, DTOFull original, bool forzarEliminacion = false);

    /// <summary>
    /// Convierte un DTO de inserción a la versión de entidad en el repositorio
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    DTOFull ADTOFull(DTOInsert data);

    /// <summary>
    /// Crea una entidad de actualización para un DTO Full vigente en el repositorio utilizando los datos de un DTO de actualizacion
    /// </summary>
    /// <param name="actualizacion"></param>
    /// <param name="actual"></param>
    /// <returns></returns>
    DTOFull ADTOFull(DTOUpdate actualizacion, DTOFull actual);


    /// <summary>
    /// Convierte un elemento de repositorio a despliegue
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    DTODespliegue ADTODespliegue(DTOFull data);

}
