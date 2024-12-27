using extensibilidad.metadatos;
using apigenerica.model.modelos;
using comunes.primitivas;
using System.Text.Json;
using System.Collections.Specialized;

namespace apigenerica.model.reflectores;

/// <summary>
/// Iinterfaz para marcar las etidades que deben rutearse a través de la API genérica
/// </summary>
public interface IServicioEntidadAPI
{

    /// <summary>
    /// Determina si el servicio requiere de autenticación
    /// </summary>
    bool RequiereAutenticacion { get; } 

    /// <summary>
    /// Devuelve los metadatos de la entidad completa tal como se almacena en el repositorio
    /// </summary>
    /// <returns></returns>
    Entidad EntidadRepoAPI();

    /// <summary>
    /// Devuelve los datos de la entidad para realizar la inserción al repositorio
    /// </summary>
    /// <returns></returns>
    Entidad EntidadInsertAPI();


    /// <summary>
    /// Devuelve los metadatos de la entidad para realizar la actualización al repositorio
    /// </summary>
    /// <returns></returns>
    Entidad EntidadUpdateAPI();

    /// <summary>
    /// Devuelve los metadatos de la entidad para el despliegue
    /// </summary>
    /// <returns></returns>
    Entidad EntidadDespliegueAPI();


    /// <summary>
    /// Establece el contexto de ejecución del usuario 
    /// </summary>
    /// <param name="contexto"></param>
    void EstableceContextoUsuarioAPI(ContextoUsuario contexto);

    /// <summary>
    /// Otiene los datos del contexto de ejecución del usuario en sesión
    /// </summary>
    /// <returns></returns>
    ContextoUsuario? ObtieneContextoUsuarioAPI();

    /// <summary>
    /// Método para insertar una entidad nueva en el repositorio
    /// </summary>
    /// <param name="data"></param>
    /// <param name="parametros"></param>
    /// <returns></returns>
    Task<RespuestaPayload<object>> InsertarAPI(JsonElement data, StringDictionary? parametros = null);


    /// <summary>
    /// Actualiza el contenido de una entidad en el repositorio 
    /// </summary>
    /// <param name="id"></param>
    /// <param name="data"></param>
    /// <param name="parametros"></param>
    /// <returns></returns>
    Task<Respuesta> ActualizarAPI(object id, JsonElement data, StringDictionary? parametros = null);


    /// <summary>
    /// Elimina una entidad del repositorio eb base a su Id único
    /// </summary>
    /// <param name="id"></param>
    /// <param name="parametros"></param>
    /// <returns></returns>
    Task<Respuesta> EliminarAPI(object id, StringDictionary? parametros = null);


    /// <summary>
    /// Obtiene una entidad del repositorio por Id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<RespuestaPayload<object>> UnicaPorIdAPI(object  id, StringDictionary? parametros = null);

    /// <summary>
    /// Obtiene una entidad para despliegue del repositorio por Id
    /// </summary>
    /// <param name="id"></param>
    /// <param name="parametros"></param>
    /// <returns></returns>
    Task<RespuestaPayload<object>> UnicaPorIdDespliegueAPI(object id, StringDictionary? parametros = null);


    /// <summary>
    /// Obtiene una lista de elementos en base a la configuración de la consulta y su paginado
    /// </summary>
    /// <param name="consulta"></param>
    /// <param name="parametros"></param>
    /// <returns></returns>
    Task<RespuestaPayload<PaginaGenerica<object>>> PaginaAPI(Consulta consulta, StringDictionary? parametros = null);

    /// <summary>
    /// Obtiene una lista de elementos para el despliegue en base a la configuración de la consulta y su paginado
    /// </summary>
    /// <param name="consulta"></param>
    /// <param name="parametros"></param>
    /// <returns></returns>
    Task<RespuestaPayload<PaginaGenerica<object>>> PaginaDespliegueAPI(Consulta consulta, StringDictionary? parametros = null);

    /// <summary>
    /// DEvulver la conjunción de DTOFull, DTOInsert y DTOUpdate para la entidad con el tipo
    /// </summary>
    /// <param name="Tipo"></param>
    /// <param name="parametros"></param>
    /// <returns></returns>
    Task<Entidad>? Metadatos(string Tipo, StringDictionary? parametros = null);


    /// <summary>
    /// Obtiene los datos de un árbol
    /// </summary>
    /// <param name="parcial">Si es true devuelve sólo los hijos directos de raizId</param>
    /// <param name="raizId">Id de la raíz si es nulo se devuelven los nodos directamente conectados a la raíz</param>
    /// <param name="incluirPayload">Determina si en la información del árbol debe debolverse la entidad completa</param>
    /// <returns></returns>
    /// <returns></returns>
    Task<RespuestaPayload<List<NodoArbol<object>>>> Arbol(string? raizId = null, bool parcial = false,  bool incluirPayload = false, StringDictionary? parametros = null);

    /// <summary>
    /// Obtiene una lista de rextos asociados a un Id de entidad, por ejemplo para hace cache de los nombres 
    /// </summary>
    /// <param name="Ids">Lista de Ids para obtener los textos</param>
    /// <returns></returns>
    Task<RespuestaPayload<List<ParClaveTexto>>> TextoIds(List<string> Ids, StringDictionary? parametros = null);

    /// <summary>
    /// Método para insertar múltiples entidades nuevas en el repositorio
    /// </summary>
    /// <param name="data">Elemento JSON en forma de Lista de objetos</param>
    /// <param name="parametros"></param>
    /// <returns></returns>
    Task<RespuestaPayload<List<object>>> InsertarMultipleAPI(JsonElement data, StringDictionary? parametros = null);


    /// <summary>
    /// Elimina una lista de entidades del repositorio eb base a sus Ids únicos
    /// </summary>
    /// <param name="ids">Lista de identificadores</param>
    /// <param name="parametros"></param>
    /// <returns></returns>
    Task<Respuesta> EliminarAPI(List<string> ids, StringDictionary? parametros = null);
}


