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
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IServicioEspacioTrabajo servicioEspacioTrabajo;

        /// <summary>
        /// 
        /// </summary>
        public EspacioTrabajoController(ILogger<EspacioTrabajoController> logger, IHttpContextAccessor httpContextAccessor, IServicioEspacioTrabajo servicioEspacioTrabajo) : base(httpContextAccessor)
        {
            this._logger = logger;
            this.httpContextAccessor = httpContextAccessor;
            this.servicioEspacioTrabajo = servicioEspacioTrabajo;
        }

        [HttpGet("usuario/{usuarioId}")]
        public async Task<ActionResult<List<EspacioTrabajoUsuario>>> ObtieneEspaciosUsuario([FromRoute] string usuarioId) {
            _logger.LogDebug("EspacioTrabajoController - ObtieneEspaciosUsuario - {usuarioId}", usuarioId);
            List<EspacioTrabajoUsuario> l = [];

            // llamar al servicio para obtener la lista de espacios de trabajo del susuario
            var espaciosTrabajo = await this.servicioEspacioTrabajo.ObtieneEspaciosUsuario(usuarioId);
            l = (List<EspacioTrabajoUsuario>)espaciosTrabajo.Payload;

            if(l.Count>0) 
            {
                return Ok(l);
            }
            _logger.LogDebug("EspacioTrabajoController - ObtieneEspacioUsuario - resultado {ok} {code} {error}", espaciosTrabajo!.Ok, espaciosTrabajo!.HttpCode, espaciosTrabajo.Error);
            return NotFound();
        }


    }
}
