using apigenerica.model.modelos;
using comunes.primitivas;

namespace apigenerica.model.reflectores;

/// <summary>
/// Interfaz para el servicio de enntidades de APi genéricos
/// </summary>
public interface IServicioCatalogoAPI
{

    /// <summary>
    /// Especifica si el servicio require autenticacion
    /// </summary>
    public bool RequiereAutenticacion { get; }

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
    /// DEvuelve el idioma por defecto del catálogo
    /// </summary>
    public string IdiomaDefault { get; }

    /// <summary>
    /// Devuelve la lista completa de elemntos del catálogo en base al idioma
    /// </summary>
    /// <param name="Idioma">Idioma del catálogo</param>
    /// <returns></returns>
    Task<RespuestaPayload<List<ParClaveTexto>>> Todo(string catalogoId, string? idioma);


    /// <summary>
    /// Devuelve una lista de entradas que contienen el texto buscad
    /// </summary>
    /// <param name="idioma">Idioma del catálogo</param>
    /// <param name="buscar">Texto a buscar</param>
    /// <returns></returns>
    Task<RespuestaPayload<List<ParClaveTexto>>> PorTexto(string catalogoId, string? idioma, string? buscar);


    /// <summary>
    /// Crea una nueva entrada nueva en el catálogo
    /// </summary>
    /// <param name="elemento"></param>
    /// <returns></returns>
    Task<RespuestaPayload<ElementoCatalogo>> CreaEntrada(string catalogoId, ElementoCatalogoInsertar elemento);

    /// <summary>
    /// Elimina una entrada del catálogo para todos los idiomas
    /// </summary>
    /// <param name="Id">Identificador único de la entrada</param>
    /// <returns></returns>
    Task<Respuesta> EliminaEntrada(string catalogoId, string Id);


    /// <summary>
    /// Actualiza una entrada en el catálogo por idioma
    /// </summary>
    /// <param name="Id">Identificador único de la entrada</param>
    /// <param name="idioma">Idioma de la entrada</param>
    /// <param name="texto">texto para la enrada</param>
    /// <returns></returns>
    Task<Respuesta> ActualizaEntrada(string catalogoId, string Id, ElementoCatalogoActualizar elemento);


    /// <summary>
    /// Devuelve la lista de idiomas disponibles para el catálogo
    /// </summary>
    /// <returns></returns>
    Task<List<string>> Idiomas();

    /// <summary>
    /// Lista de elementos del catálogo por defecto, sirve para poblar el repositorio inicial
    /// </summary>
    /// <returns></returns>
    List<ElementoCatalogo> ElementosDefault();

    /// <summary>
    /// Verifica si es posible actualizar una entrada del catálogo
    /// </summary>
    /// <param name="id"></param>
    /// <param name="actualizacion"></param>
    /// <param name="original"></param>
    /// <returns></returns>
    Task<ResultadoValidacion> ValidarActualizar(string id, ElementoCatalogoActualizar actualizacion, ElementoCatalogo original);
    
    
    /// <summary>
    /// Verifica que pueda eliminarse una eliminarse una entrada del catálogo
    /// </summary>
    /// <param name="id"></param>
    /// <param name="original"></param>
    /// <returns></returns>
    Task<ResultadoValidacion> ValidarEliminacion(string id, ElementoCatalogo original);
    
    /// <summary>
    /// Verifica si es p osible insertar un elemento de catálogo
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    Task<ResultadoValidacion> ValidarInsertar(ElementoCatalogoInsertar data);
    
    /// <summary>
    /// Convierte a una entidad complete desde la inserción
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    ElementoCatalogo ADTOFull(string catalogoId, ElementoCatalogoInsertar data);

    /// <summary>
    /// Convierte a una entidad complete desde la actualización
    /// </summary>
    /// <param name="data"></param>
    /// <param name="elemento"></param>
    /// <returns></returns>
    ElementoCatalogo ADTOFull(ElementoCatalogoActualizar data, ElementoCatalogo elemento);
}
