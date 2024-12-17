using apigenerica.primitivas;
using comunes.primitivas;
using evaluacion.model;
using evaluacion.model.evaluacion;
using evaluacion.model.reactivos;
using evaluacion.services.evaluacion;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace evaluacion.api.Controllers;

[ApiController]
public class EvaluacionController : ControladorBaseGenerico
{
    private readonly ILogger<EvaluacionController> _logger;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IServicioEvaluacion _servicioEvaluacion;


    public EvaluacionController(ILogger<EvaluacionController> logger, IHttpContextAccessor httpContextAccessor, IServicioEvaluacion servicioEvaluacion) : base(httpContextAccessor)
    {
        _logger = logger;
        _httpContextAccessor = httpContextAccessor;
        _servicioEvaluacion = servicioEvaluacion;
    }

    [HttpPut("evaluacion/{evaluacionId}/estado/{nuevoEstado}")]
    public async Task<IActionResult> CambiarEstado(Guid evaluacionId, EstadoEvaluacion nuevoEstado) 
    {
        _logger.LogDebug("EvaluacionController - CambiarEstado");

        var response = await this._servicioEvaluacion.CambiarEstado(evaluacionId, nuevoEstado);
        if (response.Ok)
        {
            return NoContent();
        } 

        return StatusCode(response.HttpCode.GetHashCode(), response.Error);
    }

    [HttpPost("evaluacion/{evaluacionId}/reactivo/multiplecrear")]
    public async Task<IActionResult> ReactivoMultipleCrear(Guid evaluacionId, [FromBody] JsonElement data)
    {
        _logger.LogDebug("EvaluacionController - ReactivoMultipleCrear");
        var reactivos = data.Deserialize<ReactivoMultipleCrear>(new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
        var response = await this._servicioEvaluacion.ReactivoMultipleCrear(evaluacionId, reactivos);

        if (response.Ok)
        {
            return NoContent();
        }

        return StatusCode(response.HttpCode.GetHashCode(), response.Error);
    }

    [HttpDelete("evaluacion/{evaluacionId}/reactivo/multipleliminar")]
    public async Task<IActionResult> ReactivoMultipleEliminar(Guid evaluacionId, [FromBody] JsonElement data)
    {
        _logger.LogDebug("EvaluacionController - ReactivoMultipleEliminar");
        var reactivos = data.Deserialize<ReactivoMultipleEliminar>(new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
        var response = await this._servicioEvaluacion.ReactivoMultipleEliminar(evaluacionId, reactivos);

        if (response.Ok)
        {
            return NoContent();
        }

        return StatusCode(response.HttpCode.GetHashCode(), response.Error);
    }

}
