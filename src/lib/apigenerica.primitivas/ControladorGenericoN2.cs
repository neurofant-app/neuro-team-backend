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
[SwaggerTag(description: "Controlador Genérico Entidad 2 Niveles")]
public abstract class ControladorGenericoN2 : ControladorBaseGenerico
{
    private readonly ILogger<ControladorGenericoN2> _logger;

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
    public ControladorGenericoN2(ILogger<ControladorGenericoN2> logger, IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
    {
        this._logger = logger;
        entidadAPI = (IServicioEntidadAPI)httpContextAccessor.HttpContext!.Items[EntidadAPIMiddleware.GenericAPIServiceKey]!;
        parametros = (StringDictionary?)httpContextAccessor.HttpContext!.Items[EntidadAPIMiddleware.DiccionarioNivelGenericoKey]!;
    }

    /// <summary>
    /// Obtiene los metadatos de una entidad
    /// </summary>
    /// <param name="n0">Tipo de la entidad padre</param>
    /// <param name="n0Id">Identifiador único del padre</param>
    /// <param name="n1">Tipo de entidad en base al ruteo</param>
    /// <returns></returns>
    [HttpGet("/entidad/{n0}/{n0Id}/{n1}/metadatos", Name = "DefinicionEntidadN2")]
    [SwaggerOperation("Obtiene los Metadatos de una entidad del tipo especificado por el ruteo", OperationId = "DefinicionEntidadN2")]
    [SwaggerResponse(statusCode: 200, type: typeof(Entidad), description: "Metadatos de la entidad N2")]
    [SwaggerResponse(statusCode: 404, description: "Entidad Hijo no localizada o inexistente")]
    public async Task<IActionResult> Metadatos(string n0, string n0Id, string n1)
    {
        _logger.LogDebug($"Metadatos {n0}/{n0Id}/{n1}");
        // calcula la Entidad desde el servico y s ni devuelve null devolver 404
        var resultado = await entidadAPI.Metadatos(n1, parametros);
        if (resultado == null)
        {
            return NotFound(n1);
        }
        return Ok(resultado);
    }


    /// <summary>
    /// Obtiene una entidad tal como se almacena en el repositorio en base a su identificador único
    /// </summary>
    /// <param name="n0">Tipo de la entidad padre</param>
    /// <param name="n0Id">Identificador único del padre</param>
    /// <param name="n1">Tipo de entidad en base al ruteo</param>
    /// <param name="n1Id">Identificador único de la entidad base</param>
    /// <param name="dominioId">Id del sominio del usuario en sesión</param>
    /// <param name="uOrgID">Id de la unidad organizacional del usuario en sesión</param>
    /// <param name="despliegue">Determina una entidad para despliegue en base a su identificador único</param>
    /// <returns></returns>
    [HttpGet("/entidad/{n0}/{n0Id}/{n1}/{n1Id}", Name = "UnicoPorIdN2")]
    [SwaggerOperation("Obtiene una entidad en base a su identificador único", OperationId = "UnicoPorIdN2")]
    [SwaggerResponse(statusCode: 200, type: typeof(object), description: "Entidad localizada")]
    [SwaggerResponse(statusCode: 404, description: "Entidad no localizada o inexistente")]
    [SwaggerResponse(statusCode: 403, description: "El usuario en sesión no tiene acceso a la operación")]
    [SwaggerResponse(statusCode: 401, description: "Usuario no autenticado")]
    [SwaggerResponse(statusCode: 405, description: "Método no implementado")]
    public async Task<IActionResult> PorId(string n0, string n0Id, string n1, string n1Id,
        [FromHeader(Name = DOMINIOHEADER)] string dominioId, [FromHeader(Name = UORGHEADER)] string uOrgID, [FromQuery(Name = "d")] bool? despliegue = true)
    {
        _logger.LogDebug($"PorId {n0}/{n0Id}/{n1} despliegue {despliegue}");
        RespuestaPayload<object> respuesta = new RespuestaPayload<object>();

        if (despliegue == true)
        {
            respuesta = await entidadAPI.UnicaPorIdDespliegueAPI((object)n1Id, parametros);
        }
        else
        {
            respuesta = await entidadAPI.UnicaPorIdAPI((object)n1Id, parametros);
        }

        if (respuesta.Ok)
        {
            return Ok(respuesta.Payload);
        }
        return StatusCode(respuesta.HttpCode.GetHashCode(), respuesta.Error);
    }

    /// <summary>
    /// Crea una entidad del tipo especificado por el ruteo
    /// </summary>
    /// <param name="n0">Tipo de la entidad padre</param>
    /// <param name="n0Id">Identificador único del padre</param>
    /// <param name="n1">Tipo de la entidad base al ruteo</param>
    /// <param name="dtoInsert">DTO para inserción e la entidad, no debe incluir el Id</param>
    /// <param name="dominioId">Id del dominio del usuario en sesión</param>
    /// <param name="uOrgID">Id de la unidad organizacional del usuario en sesión</param>
    /// <returns></returns>
    [HttpPost("/entidad/{n0}/{n0Id}/{n1}", Name = "POSTGenericoN2")]
    [SwaggerOperation("Crea una entidad del tipo especificado por el ruteo", OperationId = "POSTGenericoN2")]
    [SwaggerResponse(statusCode: 200, type: typeof(object), description: "La entidad ha sido creada")]
    [SwaggerResponse(statusCode: 400, type: typeof(ErrorProceso), description: "Los datos proporcionados no son válidos")]
    [SwaggerResponse(statusCode: 409, type: typeof(ErrorProceso), description: "Los datos proporcionados causan conflictos en el repositorio")]
    [SwaggerResponse(statusCode: 403, description: "El usuario en sesión no tiene acceso a la operación")]
    [SwaggerResponse(statusCode: 401, description: "Usuario no autenticado")]
    [SwaggerResponse(statusCode: 405, description: "Método no implementado")]
    public async Task<IActionResult> Crea(string n0, string n0Id, string n1, [FromBody] JsonElement dtoInsert,
        [FromHeader(Name = DOMINIOHEADER)] string dominioId, [FromHeader(Name = UORGHEADER)] string uOrgID)
    {
        _logger.LogDebug($"Crea {n0}/{n0Id}/{n1}");
        var response = await entidadAPI.InsertarAPI(dtoInsert, parametros);
        if (response.Ok)
        {
            return Ok(response.Payload);
        }
        return StatusCode(response.HttpCode.GetHashCode(), response.Error);
    }

    /// <summary>
    /// Actualiza una entidad del tipo especificado por el ruteo
    /// </summary>
    /// <param name="n0">Tipo de la entidad padre</param>
    /// <param name="n0Id">Identificador único del padre</param>
    /// <param name="n1">Tipo de la entidad en base al ruteo</param>
    /// <param name="n1Id">Identificador único de la entidad base</param>
    /// <param name="dtoUpdate">DTO para la actualización</param>
    /// <param name="dominioId">Id del dominio del usuario en sesión</param>
    /// <param name="uOrgID">Id de la unidad organizacional del usuario en sesión</param>
    /// <returns></returns>
    [HttpPut("/entidad/{n0}/{n0Id}/{n1}/{n1Id}", Name = "PUTGenericoN2")]
    [SwaggerOperation("Actualiza una entidad del tipo especificado por el ruteo", OperationId = "PUTGenericoN2")]
    [SwaggerResponse(statusCode: 204, description: "La entidad ha sido actualizada")]
    [SwaggerResponse(statusCode: 400, type: typeof(ErrorProceso), description: "Los datos proporcionados no son válidos")]
    [SwaggerResponse(statusCode: 404, description: "Entidad no localizada o inexistente")]
    [SwaggerResponse(statusCode: 409, type: typeof(ErrorProceso), description: "Los datos proporcionados causan conflictos en el repositorio")]
    [SwaggerResponse(statusCode: 403, description: "El usuario en sesión no tiene acceso a la operación")]
    [SwaggerResponse(statusCode: 401, description: "Usuario no autenticado")]
    [SwaggerResponse(statusCode: 405, description: "Método no implementado")]
    public async Task<IActionResult> ActualizaPorId(string n0, string n0Id, string n1, string n1Id, [FromBody] JsonElement dtoUpdate,
        [FromHeader(Name = DOMINIOHEADER)] string dominioId, [FromHeader(Name = UORGHEADER)] string uOrgID)
    {
        _logger.LogDebug($"ActualizaPorId {n0}/{n0Id}/{n1}/{n1Id}");
        var response = await entidadAPI.ActualizarAPI((object)n1Id, dtoUpdate, parametros);
        if (response.Ok)
        {
            return NoContent();
        }
        return StatusCode(response.HttpCode.GetHashCode(), response.Error);
    }


    /// <summary>
    /// Obtiene una entidad tal como se almacena en el repositorio en base a su identificador único
    /// </summary>
    /// <param name="n0">Tipo de la entidad padre</param>
    /// <param name="n0Id">Identificador único de la entidad padre</param>
    /// <param name="n1">Tipo de la entidad en base al ruteo</param>
    /// <param name="n1Id">Identificador único de la entidad base</param>
    /// <param name="dominioId">Id del dominio del usuario en sesión</param>
    /// <param name="uOrgID">Id de la unidad organizacional del usuario en sesión</param>
    /// <returns></returns>
    [HttpDelete("/entidad/{n0}/{n0Id}/{n1}/{n1Id}", Name = "EliminarUnicoN2")]
    [SwaggerOperation("Elimina una entidad en base a su identificador único", OperationId = "EliminarUnicoN2")]
    [SwaggerResponse(statusCode: 204, description: "La entidad ha sido eliminada")]
    [SwaggerResponse(statusCode: 404, description: "Entidad inexistente o no localizada")]
    [SwaggerResponse(statusCode: 403, description: "El usuario en sesión no tiene acceso a la operación")]
    [SwaggerResponse(statusCode: 401, description: "Usuario no autenticado")]
    [SwaggerResponse(statusCode: 405, description: "Método no implementado")]
    public async Task<IActionResult> EliminaPorId(string n0, string n0Id, string n1, string n1Id,
        [FromHeader(Name = DOMINIOHEADER)] string dominioId, [FromHeader(Name = UORGHEADER)] string uOrgID)
    {
        _logger.LogDebug($"EliminaPorId {n0}/{n0Id}/{n1}/{n1Id}");
        var response = await entidadAPI.EliminarAPI((object)n1Id, parametros);
        if (response.Ok)
        {
            return NoContent();
        }
        return StatusCode(response.HttpCode.GetHashCode(), response.Error);
    }

    /// <summary>
    /// Obtiene una página de entidades tal como se almacena en el repositorio en base a la consulta
    /// </summary>
    /// <param name="n0">Tipo de la entidad padre</param>
    /// <param name="n0Id">Identificador único de la entidad padre</param>
    /// <param name="n1">Tipo de entidad en base al ruteo</param>
    /// <param name="n1Id">Identificador único de la entidad base</param>
    /// <param name="consulta">Configuración para la consulta</param>
    /// <param name="dominioId">Id del dominio del usuario en sesión</param>
    /// <param name="uOrgID">Id de la unidad organizacional del usuario en sesión</param>
    /// <param name="despliegue">Determina una página de entidades para despliegue en base a la consulta</param>
    /// <returns></returns>
    [HttpPost("/entidad/{n0}/{n0Id}/{n1}/pagina", Name = "PaginaN2")]
    [SwaggerOperation("Obtiene una página de entidades tal como se almacena en el repositorio en base a la consulta", OperationId = "PaginaN2")]
    [SwaggerResponse(statusCode: 200, type: typeof(PaginaGenerica<object>), description: "Página de datos de entidad")]
    [SwaggerResponse(statusCode: 400, description: "Datos de consulta incorrectos")]
    [SwaggerResponse(statusCode: 403, description: "El usuario en sesión no tiene acceso a la operación")]
    [SwaggerResponse(statusCode: 401, description: "Usuario no autenticado")]
    [SwaggerResponse(statusCode: 405, description: "Método no implementado")]
    public async Task<IActionResult> Pagina(string n0, string n0Id, string n1,
       [FromBody] Consulta consulta, [FromHeader(Name = DOMINIOHEADER)] string dominioId,
       [FromHeader(Name = UORGHEADER)] string uOrgID, [FromQuery(Name = "d")] bool? despliegue = true)
    {
        _logger.LogDebug($"Pagina {n0}/{n0Id}/{n1} despliegue {despliegue} consulta {JsonSerializer.Serialize(consulta)}");
        RespuestaPayload<PaginaGenerica<object>> response = new RespuestaPayload<PaginaGenerica<object>>();
        if (despliegue == true)
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
}
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
