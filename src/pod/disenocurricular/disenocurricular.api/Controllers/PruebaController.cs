using comunes.interservicio.primitivas;
using disenocurricular.services.curso;
using Microsoft.AspNetCore.Mvc;

namespace disenocurricular.api.Controllers;
/// <summary>
/// Controlador Prueba para ser usado con el ProxyEspacioTrabajo
/// </summary>
[Route("pruebaEspacioTrabajo")]
[ApiController]
public class PruebaController : ControllerBase
{
    private readonly ILogger<PruebaController> _logger;
    private readonly IServicioCurso servicioCurso;

    public PruebaController(ILogger<PruebaController> logger, IServicioCurso servicioCurso)
    {
        this._logger = logger;
        this.servicioCurso = servicioCurso;
    }


    [HttpGet("obtieneEspacios/{usuarioId}", Name = "ObtieneEspaciosUsuario")]
    public async Task<ActionResult<List<EspacioTrabajoUsuario>>> ObtieneEspaciosUsuario([FromRoute] string usuarioId)
    {
        _logger.LogDebug("PruebaController - ObtieneEspaciosUsuario - {usuarioId}", usuarioId);
        List<EspacioTrabajoUsuario> l = [];
        var espaciosTrabajo = await this.servicioCurso.ObtieneEspacios(usuarioId);
        if (espaciosTrabajo.Ok == true)
        {
            _logger.LogDebug("PruebaController - ObtieneEspacioUsuario - resultado {ok} {code} {error}", espaciosTrabajo!.Ok, espaciosTrabajo!.HttpCode, espaciosTrabajo.Error);
            return Ok(espaciosTrabajo.Payload);
        }
        _logger.LogDebug("PruebaController - ObtieneEspacioUsuario - resultado {ok} {code} {error}", espaciosTrabajo!.Ok, espaciosTrabajo!.HttpCode, espaciosTrabajo.Error);
        return NotFound();
    }
}
