using apigenerica.primitivas;
using comunes.interservicio.primitivas;
using espaciotrabajo.services.espaciotrabajo;
using Microsoft.AspNetCore.Mvc;

namespace espaciotrabajo.api.Controllers
{
    [Route("espaciotrabajo")]
    [ApiController]
    public class EspacioTrabajoController : ControladorBaseGenerico
    {
        private readonly ILogger<EspacioTrabajoController> _logger;
        private readonly IHttpContextAccessor _httpContext;
        private readonly IServicioEspacioTrabajo servicioEspacioTrabajo;

        public EspacioTrabajoController(ILogger<EspacioTrabajoController> logger, IHttpContextAccessor httpContextAccessor, IServicioEspacioTrabajo servicioEspacioTrabajo) : base(httpContextAccessor)
        {
            this._logger = logger;
            this._httpContext = httpContextAccessor;
            this.servicioEspacioTrabajo = servicioEspacioTrabajo;
        }

        [HttpGet("usuario/{usuarioId}")]
        public async Task<ActionResult<List<EspacioTrabajoUsuario>>> ObtieneEspaciosUsuario([FromRoute] string usuarioId) {
            _logger.LogDebug("EspacioTrabajoController - ObtieneEspaciosUsuario - {usuarioId}", usuarioId);
            var espaciosTrabajo = await this.servicioEspacioTrabajo.ObtieneEspaciosUsuario(usuarioId, this._httpContext.HttpContext.Request.Headers["x-d-id"], this._httpContext.HttpContext.Request.Headers["x-uo-id"]);
            if(espaciosTrabajo.Ok == true)
            {
                List<EspacioTrabajoUsuario> l = [];
                l = (List<EspacioTrabajoUsuario>)espaciosTrabajo.Payload;
                if (!l.Any())
                {
                    _logger.LogDebug("EspacioTrabajoController - ObtieneEspacioUsuario - resultado {ok} {code} {error}", espaciosTrabajo!.Ok, espaciosTrabajo!.HttpCode, espaciosTrabajo.Error);
                    return NotFound(espaciosTrabajo.Error);
                }
                return Ok(l);
            }
            _logger.LogDebug("EspacioTrabajoController - ObtieneEspacioUsuario - resultado {ok} {code} {error}", espaciosTrabajo!.Ok, espaciosTrabajo!.HttpCode, espaciosTrabajo.Error);
            return NotFound(espaciosTrabajo.Error);
        }


    }
}
