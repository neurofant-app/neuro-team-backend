using apigenerica.model.modelos;
using apigenerica.model.reflectores;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Annotations;
using System.Text.Json;

namespace apigenerica.primitivas;
#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously

[ApiController]
[SwaggerTag(description: "Controlador Genérico Entidad 2 Niveles")]
public class ControladorGenericoN2: ControladorBaseGenerico
{
    private readonly ILogger<ControladorGenericoN2> _logger;

    /// <summary>
    /// Servicio para el CRUD de la entidad
    /// </summary>
    protected IServicioEntidadAPI entidadAPI;

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
    }

    [HttpGet("/entidad/{n0}/{n0Id}/{n1}/metadatos")]
    public async Task<IActionResult> Metadatos(string n0, string n0Id, string n1)
    {
        _logger.LogDebug($"Metadatos {n0}/{n0Id}/{n1}");
        return Ok();
    }

    [HttpGet("/entidad/{n0}/{n0Id}/{n1}/{n1Id}")]
    public async Task<IActionResult> PorId(string n0, string n0Id, string n1, string n1Id,
        [FromQuery(Name = "d")] bool? despliegue = true)
    {
        _logger.LogDebug($"PorId {n0}/{n0Id}/{n1}/{n1Id} despliegue {despliegue}");
        return Ok();
    }

    [HttpPost("/entidad/{n0}/{n0Id}/{n1}")]
    public async Task<IActionResult> Crea(string n0, string n0Id, string n1)
    {
        _logger.LogDebug($"Crea {n0}/{n0Id}/{n1}");
        return Ok();
    }

    [HttpPut("/entidad/{n0}/{n0Id}/{n1}/{n1Id}")]
    public async Task<IActionResult> ActualizaPorId(string n0, string n0Id, string n1, string n1Id)
    {
        _logger.LogDebug($"ActualizaPorId {n0}/{n0Id}/{n1}/{n1Id}");
        return Ok();
    }

    [HttpDelete("/entidad/{n0}/{n0Id}/{n1}/{n1Id}")]
    public async Task<IActionResult> EliminaPorId(string n0, string n0Id, string n1, string n1Id)
    {
        _logger.LogDebug($"EliminaPorId {n0}/{n0Id}/{n1}/{n1Id}");
        return Ok();
    }

    [HttpPost("/entidad/{n0}/{n0Id}/{n1}/pagina")]
    public async Task<IActionResult> Pagina(string n0, string n0Id, string n1, string n1Id, 
        [FromBody] Consulta consulta, [FromQuery(Name = "d")] bool? despliegue = true)
    {
        _logger.LogDebug($"Pagina {n0}/{n0Id}/{n1}/{n1Id} despliegue {despliegue} consulta {JsonSerializer.Serialize(consulta)}");
        return Ok();
    }
}

#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
