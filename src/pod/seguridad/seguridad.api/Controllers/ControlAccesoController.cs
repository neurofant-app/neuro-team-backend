using Microsoft.AspNetCore.Mvc;
using apigenerica.primitivas;
using seguridad.modelo;
using System.ComponentModel.DataAnnotations;
using seguridad.modelo.servicios;

namespace seguridad.api.Controllers;

[Route("controlacceso")]
[ApiController]
public class ControlAccesoController : ControladorBaseGenerico
{

    private ILogger<ControlAccesoController> logger;
    private readonly IServicioInstanciaAplicacion servicioInstanciaAplicacion;
    private readonly string? _driver;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="contextAccessor"></param>
    public ControlAccesoController(ILogger<ControlAccesoController> logger, IHttpContextAccessor httpContextAccessor, IServicioInstanciaAplicacion servicioInstanciaAplicacion, IConfiguration configuration) : base(httpContextAccessor) {
        this.logger = logger;
        this.servicioInstanciaAplicacion = servicioInstanciaAplicacion;
        this._driver = configuration.GetValue<string>("driver")!;
    }

    [HttpGet("interno/roles/{aplicacionId}/{usuarioId}")]
    public async Task<ActionResult<List<Rol>>> ObtieneRolesUsuarioInterno([Required] string aplicacionId, [Required]string usuarioId, [FromHeader(Name = DOMINIOHEADER)] string dominioId, [FromHeader(Name = UORGHEADER)] string uOrgID)
    {
        logger.LogDebug("ControlAccesoController-ObtieneRolesUsuarioInterno- {aplicacionId} {usuarioId}", aplicacionId, usuarioId);
        return await servicioInstanciaAplicacion.GetRolesUsuarioInterno(aplicacionId, usuarioId, dominioId, uOrgID);
    }

    [HttpGet("interno/permisos/{aplicacionId}/{usuarioId}")]
    public async Task<ActionResult<List<Permiso>>> ObtienePermisosAplicacionInterno([Required] string aplicacionId, [Required] string usuarioId, [FromHeader(Name = DOMINIOHEADER)] string dominioId, [FromHeader(Name = UORGHEADER)] string uOrgID)
    {
        logger.LogDebug("ControlAccesoController-ObtienePermisosAplicacionInterno- {aplicacionId} {usuarioId}", aplicacionId, usuarioId);
        return await servicioInstanciaAplicacion.GetPermisosAplicacionInterno(aplicacionId, usuarioId, dominioId, uOrgID);
    }
}
