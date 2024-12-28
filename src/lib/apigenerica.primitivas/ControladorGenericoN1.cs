using apigenerica.model.modelos;
using apigenerica.model.reflectores;
using comunes.primitivas;
using extensibilidad.metadatos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Specialized;
using System.Text.Json;

namespace apigenerica.primitivas;
#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
[ApiController]
[SwaggerTag(description: "Controlador Genérico Entidad 1 Nivel")]
public abstract class ControladorGenericoN1 : ControladorBaseGenerico
{
    private readonly ILogger<ControladorGenericoN1> _logger;

    /// <summary>
    /// Servicio para el CRUD de la entidad
    /// </summary>
    protected IServicioEntidadAPI entidadAPI;

    /// <summary>
    /// Parametros de la petición que vienen desde el Middleware
    /// </summary>
    protected StringDictionary parametros;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="httpContextAccessor"></param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0290:Use primary constructor", Justification = "<Pending>")]
    public ControladorGenericoN1(ILogger<ControladorGenericoN1> logger, IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
    { 
        _logger = logger; 
        this._logger = logger;
        entidadAPI = (IServicioEntidadAPI)httpContextAccessor.HttpContext!.Items[EntidadAPIMiddleware.GenericAPIServiceKey]!;
        parametros = (StringDictionary?)httpContextAccessor.HttpContext!.Items[EntidadAPIMiddleware.DiccionarioNivelGenericoKey]!;
    }

    /// <summary>
    /// Obtiene los metadatos de una entidad
    /// </summary>
    /// <param name="entidad"></param>
    /// <returns></returns>
    [HttpGet("/entidad/{n0}/metadatos", Name = "DefinicionEntidadN1")]
    [SwaggerOperation("Obtiene los Metadatos de una entidad del tipo especificado por el ruteo", OperationId = "DefinicionEntidadN1")]
    [SwaggerResponse(statusCode: 200, type: typeof(Entidad), description: "Metadatos de la entidad")]
    [SwaggerResponse(statusCode: 404, description: "Entidad no localizada o inexistente")]
    public async Task<IActionResult> Metadatos(string n0)
    {
        _logger.LogDebug($"Metadatos {n0}");
        // calcula la Entidad desde el servico y s ni devuelve null devolver 404
        var resultado = await entidadAPI.Metadatos(n0, parametros);
        if (resultado == null)
        {
            return NotFound(n0);
        }
        return Ok(resultado);
    }

    [HttpGet("/entidad/{n0}/{n0Id}", Name = "UnicoPorIdN1")]
    [SwaggerOperation("Obtiene una entidad en base a su identificador único", OperationId = "UnicoPorIdN1")]
    [SwaggerResponse(statusCode: 200, type: typeof(object), description: "Entidad localizada")]
    [SwaggerResponse(statusCode: 404, description: "Entidad no localizada o inexistente")]
    [SwaggerResponse(statusCode: 403, description: "El usuario en sesión no tiene acceso a la operación")]
    [SwaggerResponse(statusCode: 401, description: "Usuario no autenticado")]
    [SwaggerResponse(statusCode: 405, description: "Método no implementado")]
    public async Task<IActionResult> PorId(string n0, string n0Id, [FromHeader(Name = DOMINIOHEADER)] string dominioId, 
        [FromHeader(Name = UORGHEADER)] string uOrgIDss, [FromQuery(Name = "d")] bool? despliegue = true)
    {
        _logger.LogDebug($"PorId {n0}/{n0Id} despliegue {despliegue}");

        RespuestaPayload<object> respuesta = new RespuestaPayload<object>();

        if(despliegue == true)
        {
            respuesta = await entidadAPI.UnicaPorIdDespliegueAPI((object)n0Id, parametros);
        }
        else
        {
            respuesta = await entidadAPI.UnicaPorIdAPI((object)n0Id, parametros);
        }

        if (respuesta.Ok)
        {
            return Ok(respuesta.Payload);
        }
        return StatusCode(respuesta.HttpCode.GetHashCode(), respuesta.Error);
    }

    [HttpPost("/entidad/{n0}", Name = "POSTGenericoN1")]
    [SwaggerOperation("Crea una entidad del tipo especificado por el ruteo", OperationId = "POSTGenericoN1")]
    [SwaggerResponse(statusCode: 200, type: typeof(object), description: "La entidad ha sido creada")]
    [SwaggerResponse(statusCode: 400, type: typeof(ErrorProceso), description: "Los datos proporcionados no son válidos")]
    [SwaggerResponse(statusCode: 409, type: typeof(ErrorProceso), description: "Los datos proporcionados causan conflictos en el repositorio")]
    [SwaggerResponse(statusCode: 403, description: "El usuario en sesión no tiene acceso a la operación")]
    [SwaggerResponse(statusCode: 401, description: "Usuario no autenticado")]
    [SwaggerResponse(statusCode: 405, description: "Método no implementado")]
    public async Task<IActionResult> Crea(string n0, [FromBody] JsonElement dtoInsert, 
        [FromHeader(Name = DOMINIOHEADER)] string dominioId, [FromHeader(Name = UORGHEADER)] string uOrgID)
    {
        _logger.LogDebug($"Crea {n0}");
        var response = await entidadAPI.InsertarAPI(dtoInsert, parametros);
        if (response.Ok)
        {
            return Ok(response.Payload);
        }
        return StatusCode(response.HttpCode.GetHashCode(), response.Error);
    }

    [HttpPut("/entidad/{n0}/{n0Id}", Name = "PUTGenericoN1")]
    [SwaggerOperation("Actualiza una entidad del tipo especificado por el ruteo", OperationId = "PUTGenericoN1")]
    [SwaggerResponse(statusCode: 204, description: "La entidad ha sido actualizada")]
    [SwaggerResponse(statusCode: 400, type: typeof(ErrorProceso), description: "Los datos proporcionados no son válidos")]
    [SwaggerResponse(statusCode: 404, description: "Entidad no localizada o inexistente")]
    [SwaggerResponse(statusCode: 409, type: typeof(ErrorProceso), description: "Los datos proporcionados causan conflictos en el repositorio")]
    [SwaggerResponse(statusCode: 403, description: "El usuario en sesión no tiene acceso a la operación")]
    [SwaggerResponse(statusCode: 401, description: "Usuario no autenticado")]
    [SwaggerResponse(statusCode: 405, description: "Método no implementado")]
    public async Task<IActionResult> ActualizaPorId(string n0, string n0Id, [FromBody] JsonElement dtoUpdate, 
        [FromHeader(Name = DOMINIOHEADER)] string dominioId, [FromHeader(Name = UORGHEADER)] string uOrgID)
    {
        _logger.LogDebug($"ActualizaPorId {n0}/{n0Id}");
        var response = await entidadAPI.ActualizarAPI((object)n0Id, dtoUpdate, parametros);
        if (response.Ok)
        {
            return NoContent();
        }
        return StatusCode(response.HttpCode.GetHashCode(), response.Error);
    }

    [HttpDelete("/entidad/{n0}/{n0Id}", Name = "EliminarUnicoN1")]
    [SwaggerOperation("Elimina una entidad en base a su identificador único", OperationId = "EliminarUnicoN1")]
    [SwaggerResponse(statusCode: 204, description: "Laentidad ha sido eliminada")]
    [SwaggerResponse(statusCode: 404, description: "Entidad inexistente o no localizada")]
    [SwaggerResponse(statusCode: 403, description: "El usuario en sesión no tiene acceso a la operación")]
    [SwaggerResponse(statusCode: 401, description: "Usuario no autenticado")]
    [SwaggerResponse(statusCode: 405, description: "Método no implementado")]
    public async Task<IActionResult> EliminarPorId(string n0, string n0Id,  
        [FromHeader(Name = DOMINIOHEADER)] string dominioId, [FromHeader(Name = UORGHEADER)] string uOrgID,
        [FromQuery(Name = "forzar")] bool forzarEliminacion = false)
    {
        _logger.LogDebug($"EliminaPorId {n0}/{n0Id}");
        var response = await entidadAPI.EliminarAPI((object)n0Id, parametros, forzarEliminacion);
        if (response.Ok)
        {
            return NoContent();
        }
        return StatusCode(response.HttpCode.GetHashCode(), response.Error);
    }

    [HttpPost("/entidad/{n0}/pagina", Name = "PaginaN1")]
    [SwaggerOperation("Obtiene una página de entidades tal como se almacena en el repositorio en base a la consulta", OperationId = "PaginaN1")]
    [SwaggerResponse(statusCode: 200, type: typeof(PaginaGenerica<object>), description: "Página de datos de entidad")]
    [SwaggerResponse(statusCode: 400, description: "Datos de consulta incorrectos")]
    [SwaggerResponse(statusCode: 403, description: "El usuario en sesión no tiene acceso a la operación")]
    [SwaggerResponse(statusCode: 401, description: "Usuario no autenticado")]
    [SwaggerResponse(statusCode: 405, description: "Método no implementado")]
    public async Task<IActionResult> Pagina(string n0,[FromBody] Consulta consulta, [FromQuery(Name = "d")] bool? despliegue = true)
    {
        _logger.LogDebug($"Pagina {n0} despliegue {despliegue} consulta {JsonSerializer.Serialize(consulta)}");
        RespuestaPayload<PaginaGenerica<object>> response = new();
        if(despliegue == true)
        {
            response = await entidadAPI.PaginaDespliegueAPI(consulta, parametros);
        }
        else
        {
            response = await entidadAPI.PaginaAPI(consulta, parametros);
        }
        
        if (response.Ok)
        {
            return Ok(response.Payload);
        }
        return StatusCode(response.HttpCode.GetHashCode(), response.Error);
    }

    [HttpPost("/entidad/{n0}/arbol", Name = "ArbolPaginaN1")]
    [SwaggerOperation("Obtiene un arbol aplanado con los nodos que lo definen", OperationId = "ArbolPaginaN1")]
    [SwaggerResponse(statusCode: 200, type: typeof(List<NodoArbol<object>>), description: "Árbol aplanado de datos de una entidad jerárquica")]
    [SwaggerResponse(statusCode: 400, description: "Datos de consulta incorrectos")]
    [SwaggerResponse(statusCode: 403, description: "El usuario en sesión no tiene acceso a la operación")]
    [SwaggerResponse(statusCode: 401, description: "Usuario no autenticado")]
    [SwaggerResponse(statusCode: 409, description: "No es posible elimiminar")]
    [SwaggerResponse(statusCode: 405, description: "Método no implementado")]
    public async Task<IActionResult> Arbol(string n0, [FromQuery(Name = "id")] string? id = null, [FromQuery(Name = "parcial")] bool parcial = false, [FromQuery(Name = "payload")] bool payload = false)
    {
        _logger.LogDebug($"Arbol aplanado de {n0} para id = {id} parcial = {parcial} payload = {payload}");
        var response = await entidadAPI.Arbol(id, parcial, payload, parametros);

        if (response.Ok)
        {
            return Ok(response.Payload);
        }
        return StatusCode(response.HttpCode.GetHashCode(), response.Error);
    }


}
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
