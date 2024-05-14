using apigenerica.primitivas;
using aplicaciones.model;
using aplicaciones.services.aplicacion;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace aplicaciones.api.Controllers;

public class AplicacionController : ControladorEntidadGenerico
{

    public AplicacionController(IHttpContextAccessor httpContextAccessor, IServicioAplicacion  servicioAplicacion ) : base(httpContextAccessor)
    {

    }

    [AllowAnonymous]
    [HttpGet("/api/aplicacion/identificar", Name = "ObtieneAplicacionPorIdentificador")]
    [SwaggerOperation("Obtiene la configuración de la apliciación", OperationId = "ObtieneAplicacionPorIdentificador")]
    [SwaggerResponse(statusCode: 200, type: typeof(ConsultaAplicacionAnonima), description: "Configuración de la Aplicación")]
    public async Task<ActionResult<ConsultaAplicacionAnonima>> ObtieneAplicacionPorIdentificador([FromQuery(Name = "key") ] string? key )
    {
        // 1. Revisar si AppId !=null y si es asi buscar la aplicación por id
        // 2. si appid == null entonces buscar la aplicación utilziando el host que viene de this._httpContextAccessor.HttpContext.Request.Host  en la lista de URLS
        // 3. si no se encuentra una aplicaicon en el punto 1 o 2  devolver la aplicación Default  = true (siempre va a existir )


        return Ok();
    }

    
}
