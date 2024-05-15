using apigenerica.primitivas;
using aplicaciones.model;
using aplicaciones.services.aplicacion;
using aplicaciones.services.dbcontext;
using comunes.primitivas;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace aplicaciones.api.Controllers;
[ApiController]
public class AplicacionController : ControllerBase
{
    private readonly IHttpContextAccessor httpContextAccessor;
    private readonly IServicioAplicacion servicioAplicacion;

    public AplicacionController(IHttpContextAccessor httpContextAccessor, IServicioAplicacion  servicioAplicacion )
    {
        this.httpContextAccessor = httpContextAccessor;
        this.servicioAplicacion = servicioAplicacion;
    }
    [HttpGet("/api/aplicacion/identificar", Name = "ObtieneAplicacionPorIdentificador")]
    [SwaggerOperation("Obtiene la configuración de la apliciación", OperationId = "ObtieneAplicacionPorIdentificador")]
    [SwaggerResponse(statusCode: 200, type: typeof(ConsultaAplicacionAnonima), description: "Configuración de la Aplicación")]
    [AllowAnonymous]
    public async Task<ActionResult<ConsultaAplicacionAnonima>> ObtieneAplicacionPorIdentificador([FromQuery(Name = "key") ] string? key )
    {
        ConsultaAplicacionAnonima consultaAplicacionAnonima;
        var consultaAnonima = await servicioAplicacion.ConsultaAplicacion(httpContextAccessor.HttpContext.Request.Host.ToString(), key);
        if(consultaAnonima == null)
        {
            return Conflict("No se encontró la aplicación");
        }
        return Ok(consultaAnonima);
    }

    
}
