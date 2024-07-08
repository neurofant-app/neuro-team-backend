using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.PortableExecutable;
using apigenerica.primitivas;
using seguridad.modelo;
using System.ComponentModel.DataAnnotations;
using seguridad.servicios;
using seguridad.servicios.mysql;

namespace seguridad.api.Controllers;

[Route("controlacceso")]
[ApiController]
public class ControlAccesoController : ControladorBaseGenerico
{

    private ILogger<ControlAccesoController> logger;
    private readonly IServicioInstanciaAplicacion servicioInstanciaAplicacion;
    private readonly IServicioInstanciaAplicacionMysql instanciaAplicacionMysql;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="contextAccessor"></param>
    public ControlAccesoController(ILogger<ControlAccesoController> logger, IHttpContextAccessor httpContextAccessor, IServicioInstanciaAplicacion servicioInstanciaAplicacion, IServicioInstanciaAplicacionMysql instanciaAplicacionMysql) : base(httpContextAccessor) {
        this.logger = logger;
        this.servicioInstanciaAplicacion = servicioInstanciaAplicacion;
        this.instanciaAplicacionMysql = instanciaAplicacionMysql;
    }

    [HttpGet("interno/roles/{aplicacionId}/{usuarioId}")]
    public async Task<ActionResult<List<Rol>>> ObtieneRolesUsuarioInterno([Required] string aplicacionId, [Required]string usuarioId, [FromHeader(Name = DOMINIOHEADER)] string dominioId, [FromHeader(Name = UORGHEADER)] string uOrgID)
    {
        //return await servicioInstanciaAplicacion.GetRolesUsuarioInterno(aplicacionId, usuarioId, dominioId, uOrgID);
        return await instanciaAplicacionMysql.GetRolesUsuarioInterno(aplicacionId, usuarioId, dominioId, uOrgID);
    }

    [HttpGet("interno/permisos/{aplicacionId}/{usuarioId}")]
    public async Task<ActionResult<List<Permiso>>> ObtienePermisosAplicacionInterno([Required] string aplicacionId, [Required] string usuarioId, [FromHeader(Name = DOMINIOHEADER)] string dominioId, [FromHeader(Name = UORGHEADER)] string uOrgID)
    {
        //return await servicioInstanciaAplicacion.GetPermisosAplicacionInterno(aplicacionId, usuarioId, dominioId, uOrgID);
        return await instanciaAplicacionMysql.GetPermisosAplicacionInterno(aplicacionId, usuarioId, dominioId, uOrgID);
    }

}
