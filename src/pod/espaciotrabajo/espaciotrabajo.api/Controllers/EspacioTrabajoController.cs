using apigenerica.primitivas;
using comunes.interservicio.primitivas;
using comunes.interservicio.primitivas.identidad;
using comunes.primitivas;
using espaciotrabajo.model.espaciotrabajo;
using espaciotrabajo.services;
using espaciotrabajo.services.espaciotrabajo;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace espaciotrabajo.api.Controllers
{
    [Route("espaciotrabajo")]
    [ApiController]
    public class EspacioTrabajoController : ControladorBaseGenerico
    {
        private readonly ILogger<EspacioTrabajoController> _logger;
        private readonly IHttpContextAccessor _httpContext;
        private readonly IServicioEspacioTrabajo servicioEspacioTrabajo;
        private readonly IProxyIdentidad _proxyIdentidad;

        public EspacioTrabajoController(ILogger<EspacioTrabajoController> logger, 
                                        IHttpContextAccessor httpContextAccessor, 
                                        IServicioEspacioTrabajo servicioEspacioTrabajo,
                                        IProxyIdentidad proxyIdentidad) : base(httpContextAccessor)
        {
            this._logger = logger;
            this._httpContext = httpContextAccessor;
            this.servicioEspacioTrabajo = servicioEspacioTrabajo;
            this._proxyIdentidad = proxyIdentidad;
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

        [HttpDelete("{id}/usuario/{usuarioId}")]

        public async Task<ActionResult> EliminarUsuarioEspacioTrabajo(string id, string usuarioId)
        {
            _logger.LogDebug("EspacioTrabajoController - EliminarUsuarioEspacioTrabajo - {id} {usuarioId}", id, usuarioId);
            var existeEspacioTrabajo = await this.servicioEspacioTrabajo.UnicaPorId(id);
            var espacioTrabajo = (EspacioTrabajo)existeEspacioTrabajo.Payload;
            if(espacioTrabajo == null)
            {
                return NotFound(CodigosError.ESPACIOTRABAJO_NO_EXISTE);
            }

            var miembroEspacio = espacioTrabajo.Miembros.Find(m => m.UsuarioId.Equals(usuarioId));

            if(miembroEspacio != null)
            {
                var usuarioProxy = await this._proxyIdentidad.ExisteUsuarioId(usuarioId);
                if (usuarioProxy == false)
                {
                    return NotFound(CodigosError.ESPACIOTRABAJO_USUARIO_NO_EXISTE);
                }
                else
                {
                    espacioTrabajo.Miembros.Remove(miembroEspacio);
                    var eliminadoUsuario = await this.servicioEspacioTrabajo.ActualizaDbSetEspacioTrabajo(espacioTrabajo);
                    if (eliminadoUsuario.Ok == false)
                    {
                        return BadRequest(CodigosError.ESPACIOTRABAJO_ERROR_AL_ELIMINAR_USUARIO_EN_ESPACIOTRABAJO);
                    }
                }
            }

            return Ok();
        }

        [HttpPost("{id}/usuario/{usuarioId}")]
        public async Task<ActionResult> InsertarUsuarioEspacioTrabajo(string id, string usuarioId)
        {
            _logger.LogDebug("EspacioTrabajoController - InsertarUsuarioEspacioTrabajo - {id} {usuarioId}", id, usuarioId);
            var existeEspacioTrabajo = await this.servicioEspacioTrabajo.UnicaPorId(id);
            var espacioTrabajo = (EspacioTrabajo)existeEspacioTrabajo.Payload;
            if(espacioTrabajo == null)
            {
                return NotFound(CodigosError.ESPACIOTRABAJO_NO_EXISTE);
            }

            var miembroEspacio = espacioTrabajo.Miembros.Find(m => m.UsuarioId.Equals(usuarioId));
            if(miembroEspacio == null)
            {

                var usuarioProxy = await this._proxyIdentidad.ExisteUsuarioId(usuarioId);
                if(usuarioProxy == false)
                {
                    return NotFound(CodigosError.ESPACIOTRABAJO_USUARIO_NO_EXISTE);
                }
                else
                {
                    espacioTrabajo.Miembros.Add(new Miembro() { UsuarioId= usuarioId});
                    var insertandoUsuario = await this.servicioEspacioTrabajo.ActualizaDbSetEspacioTrabajo(espacioTrabajo);
                    if (insertandoUsuario.Ok == false)
                    {
                        return BadRequest(CodigosError.ESPACIOTRABAJO_ERROR_AL_INSERTAR_USUARIO_EN_ESPACIOTRABAJO);
                    }
                }

            }
            return Ok();
        }

    }
}
