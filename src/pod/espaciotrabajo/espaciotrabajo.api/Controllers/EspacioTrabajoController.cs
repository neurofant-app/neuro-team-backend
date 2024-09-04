using espaciotrabajo.model.proxy;
using Microsoft.AspNetCore.Mvc;

namespace espaciotrabajo.api.Controllers
{
    [Route("espaciotrabajo")]
    [ApiController]
    public class EspacioTrabajoController : ControllerBase
    {
        /// <summary>
        /// 
        /// </summary>
        public EspacioTrabajoController() {

        }

        [HttpGet("usuario/{usuarioId}", Name = "ObtieneEspaciosUsuario")]
        public async Task<ActionResult<List<EspacioTrabajoUsuario>>> ObtieneEspaciosUsuario([FromRoute] string usuarioId) {

            List<EspacioTrabajoUsuario> l = [];
            
            // llamar al servicio para obtener la lista de espacios de trabajo del susuario
            
            return Ok(l);
        }


    }
}
