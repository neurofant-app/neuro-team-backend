using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.PortableExecutable;
using apigenerica.primitivas;
using seguridad.modelo;
using System.ComponentModel.DataAnnotations;

namespace seguridad.api.Controllers;

[Route("controlacceso")]
[ApiController]
public class ControlAccesoController : ControladorBaseGenerico
{

    private ILogger<ControlAccesoController> logger;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="contextAccessor"></param>
    public ControlAccesoController(ILogger<ControlAccesoController> logger, HttpContextAccessor contextAccessor ): base(contextAccessor ) {
        this.logger = logger;
    }


    [HttpGet("interno/roles/{usuarioId}")]
    public async Task<ActionResult<List<Rol>>> ObtieneRolesUsuarioInterno([Required]string usuarioId, [FromHeader(Name = DOMINIOHEADER)] string dominioId, [FromHeader(Name = UORGHEADER)] string uOrgID)
    {
        // DEbe devolver 
        throw new NotImplementedException();
    }

    [HttpGet("interno/permisos/{aplicacionId}/{usuarioId}")]
    public async Task<ActionResult<List<Permiso>>> ObtienePermisosAplicacionInterno([Required] string aplicacionId, [Required] string usuarioId, [FromHeader(Name = DOMINIOHEADER)] string dominioId, [FromHeader(Name = UORGHEADER)] string uOrgID)
    {
        // DEbe devolver 
        throw new NotImplementedException();
    }

}
