using apigenerica.model.modelos;
using apigenerica.model.reflectores;
using comunes.primitivas;
using extensibilidad.metadatos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace apigenerica.primitivas;
#pragma warning disable CS1998 // Async methos lacks 'await' operators and will run synchronously
[ApiController]
[SwaggerTag(description: "Controlador Genérico Entidad 3 Niveles")]
public class ControladorGenericoN3 : ControladorBaseGenerico
{
    private readonly ILogger<ControladorGenericoN3> _logger;
    /// <summary>
    /// Servicio para el CRUD de la entidad
    /// </summary>
    private IServicioEntidadAPI entidadAPI;


    /// <summary>
    /// Parametros de la petición que vienen desde el Middleware
    /// </summary>
    protected StringDictionary parametros;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="httpContextAccessor"></param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0290:Use primary constructor", Justification ="<Pending>")]
    public ControladorGenericoN3(ILogger<ControladorGenericoN3> logger, IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
    {
        _logger = logger;
        entidadAPI = (IServicioEntidadAPI)httpContextAccessor.HttpContext!.Items[EntidadAPIMiddleware.GenericAPIServiceKey]!;
        parametros = (StringDictionary?)httpContextAccessor.HttpContext!.Items[EntidadAPIMiddleware.DiccionarioNivelGenericoKey];
    }

    [HttpGet("/entidad/{n0}/{n0Id}/{n1}/{n1Id}/{n2}/metadatos", Name = "DefinicionEntidadN3")]
    [SwaggerOperation("Obtiene los Metadatos de una entidad del tipo especificado por el ruteo", OperationId = "DefinicionEntidadN3")]
    [SwaggerResponse(statusCode: 200, type: typeof(Entidad), description: "Metadatos de la entidad")]
    [SwaggerResponse(statusCode: 404, description: "Entidad no localizada o inexistente")]
    public async Task<IActionResult> Metadatos(string n0, string n0Id, string n1, string n1Id, string n2)
    {
        _logger.LogDebug($"Metadatos {n0}/{n0Id}/{n1}/{n1Id}/{n2}/metadatos");
        // calcula la Entidad desde el servico y s ni devuelve null devolver 404
        var resultado = await entidadAPI.Metadatos(n2, parametros);
        if (resultado == null)
        {
            return NotFound(n0);
        }
        return Ok(resultado);
    }

    [HttpGet("/entidad/{n0}/{n0Id}/{n1}/{n1Id}/{n2}/{n2Id}", Name = "UnicoPorIdN3")]
    [SwaggerOperation("Obtiene una entidad en base a su identificador único", OperationId = "UnicoPorIdN3")]
    [SwaggerResponse(statusCode: 200, type: typeof(object), description: "Entidad localizada")]
    [SwaggerResponse(statusCode: 404, description: "Entidad no localizada o inexistente")]
    [SwaggerResponse(statusCode: 403, description: "El usuario en sesión no tiene acceso a la operación")]
    [SwaggerResponse(statusCode: 401, description: "Usuario no autenticado")]
    [SwaggerResponse(statusCode: 405, description: "Método no implementado")]
    public async Task<IActionResult> PorId(string n0, string n0Id, string n1, string n1Id, string n2, string n2Id, 
        [FromHeader(Name = DOMINIOHEADER)] string dominioId, [FromHeader(Name = UORGHEADER)] string uOrgIDss, 
        [FromQuery(Name = "d")] bool? despliegue = true)
    {
        _logger.LogDebug($"PorId: {n0}/{n0Id}/{n1}/{n1Id}/{n2}/{n2Id} despliegue {despliegue}");
        RespuestaPayload<object> respuesta = new RespuestaPayload<object>();

        if (despliegue == true)
        {
            respuesta = await entidadAPI.UnicaPorIdDespliegueAPI((object)n2Id, parametros);
        }
        else
        {
            respuesta = await entidadAPI.UnicaPorIdAPI((object)n2Id, parametros);
        }

        if (respuesta.Ok)
        {
            return Ok(respuesta.Payload);
        }
        return StatusCode(respuesta.HttpCode.GetHashCode(), respuesta.Error);
    }

    [HttpPost("/entidad/{n0}/{n0Id}/{n1}/{n1Id}/{n2}", Name = "POSTGenericoN3")]
    [SwaggerOperation("Crea una entidad del tipo especificado por el ruteo", OperationId = "POSTGenericoN3")]
    [SwaggerResponse(statusCode: 200, type: typeof(object), description: "La entidad ha sido creada")]
    [SwaggerResponse(statusCode: 400, type: typeof(ErrorProceso), description: "Los datos proporcionados no son válidos")]
    [SwaggerResponse(statusCode: 409, type: typeof(ErrorProceso), description: "Los datos proporcionados causan conflictos en el repositorio")]
    [SwaggerResponse(statusCode: 403, description: "El usuario en sesión no tiene acceso a la operación")]
    [SwaggerResponse(statusCode: 401, description: "Usuario no autenticado")]
    [SwaggerResponse(statusCode: 405, description: "Método no implementado")]
    public async Task<IActionResult> Crea(string n0, string n0Id, string n1, string n1Id, string n2, [FromBody] JsonElement dtoInsert,
        [FromHeader(Name = DOMINIOHEADER)] string dominioId, [FromHeader(Name = UORGHEADER)] string uOrgID)
    {
        _logger.LogDebug($"Crea {n0}/{n0Id}/{n1}/{n1Id}/{n2}");
        var response = await entidadAPI.InsertarAPI(dtoInsert, parametros);
        if (response.Ok)
        {
            return Ok(response.Payload);
        }
        return StatusCode(response.HttpCode.GetHashCode(), response.Error);
    }

    [HttpPut("/entidad/{n0}/{n0Id}/{n1}/{n1Id}/{n2}/{n2Id}", Name = "PUTGenericoN3")]
    [SwaggerOperation("Actualiza una entidad del tipo especificado por el ruteo", OperationId = "PUTGenericoN3")]
    [SwaggerResponse(statusCode: 204, description: "La entidad ha sido actualizada")]
    [SwaggerResponse(statusCode: 400, type: typeof(ErrorProceso), description: "Los datos proporcionados no son válidos")]
    [SwaggerResponse(statusCode: 404, description: "Entidad no localizada o inexistente")]
    [SwaggerResponse(statusCode: 409, type: typeof(ErrorProceso), description: "Los datos proporcionados causan conflictos en el repositorio")]
    [SwaggerResponse(statusCode: 403, description: "El usuario en sesión no tiene acceso a la operación")]
    [SwaggerResponse(statusCode: 401, description: "Usuario no autenticado")]
    [SwaggerResponse(statusCode: 405, description: "Método no implementado")]
    public async Task<IActionResult> ActualizaPorId(string n0, string n0Id, string n1, string n1Id, string n2,string n2Id,
        [FromBody] JsonElement dtoUpdate,[FromHeader(Name = DOMINIOHEADER)] string dominioId, [FromHeader(Name = UORGHEADER)] string uOrgID)
    {
        _logger.LogDebug($"ActualizaPorId {n0}/{n0Id}/{n1}/{n1Id}/{n2}/{n2Id}");
        var response = await entidadAPI.ActualizarAPI((object)n2Id, dtoUpdate, parametros);
        if (response.Ok)
        {
            return NoContent();
        }
        return StatusCode(response.HttpCode.GetHashCode(), response.Error);
    }

    [HttpDelete("/entidad/{n0}/{n0Id}/{n1}/{n1Id}/{n2}/{n2Id}", Name = "EliminarUnicoN3")]
    [SwaggerOperation("Elimina una entidad en base a su identificador único", OperationId = "EliminarUnicoN3")]
    [SwaggerResponse(statusCode: 204, description: "Laentidad ha sido eliminada")]
    [SwaggerResponse(statusCode: 404, description: "Entidad inexistente o no localizada")]
    [SwaggerResponse(statusCode: 403, description: "El usuario en sesión no tiene acceso a la operación")]
    [SwaggerResponse(statusCode: 401, description: "Usuario no autenticado")]
    [SwaggerResponse(statusCode: 405, description: "Método no implementado")]
    public async Task<IActionResult> EliminarPorId(string n0, string n0Id, string n1, string n1Id, string n2, string n2Id,
        [FromHeader(Name = DOMINIOHEADER)] string dominioId, [FromHeader(Name = UORGHEADER)] string uOrgID)
    {
        _logger.LogDebug($"EliminaPorId {n0}/{n0Id}/{n1}/{n1Id}/{n2}/{n2Id}");
        var response = await entidadAPI.EliminarAPI((object)n2Id, parametros);
        if (response.Ok)
        {
            return NoContent();
        }
        return StatusCode(response.HttpCode.GetHashCode(), response.Error);
    }

    [HttpPost("/entidad/{n0}/{n0Id}/{n1}/{n1Id}/{n2}/{n2Id}", Name = "PaginaN3")]
    [SwaggerOperation("Obtiene una página de entidades tal como se almacena en el repositorio en base a la consulta", OperationId = "PaginaN3")]
    [SwaggerResponse(statusCode: 200, type: typeof(PaginaGenerica<object>), description: "Página de datos de entidad")]
    [SwaggerResponse(statusCode: 400, description: "Datos de consulta incorrectos")]
    [SwaggerResponse(statusCode: 403, description: "El usuario en sesión no tiene acceso a la operación")]
    [SwaggerResponse(statusCode: 401, description: "Usuario no autenticado")]
    [SwaggerResponse(statusCode: 405, description: "Método no implementado")]
    public async Task<IActionResult> Pagina(string n0, string n0Id, string n1, string n1Id, string n2, string n2Id,
        [FromBody] Consulta consulta, [FromQuery(Name = "d")] bool? despliegue = true)
    {
        _logger.LogDebug($"Pagina {n0}/{n0Id}/{n1}/{n1Id}/{n2}/{n2Id} despliegue {despliegue} consulta {JsonSerializer.Serialize(consulta)}");
        RespuestaPayload<PaginaGenerica<object>> response = new RespuestaPayload<PaginaGenerica<object>>();
        if (despliegue == true)
        {
            response = await entidadAPI.PaginaDespliegueAPI(consulta, parametros);
        }
        else
        {
            response = await entidadAPI.PaginaAPI(consulta);
        }

        if (response.Ok)
        {
            return Ok(response.Payload);
        }
        return StatusCode(response.HttpCode.GetHashCode(), response.Error);    
    }
}
#pragma warning restore CS1998 //Async methos lacks 'await' operators and will run synchronously