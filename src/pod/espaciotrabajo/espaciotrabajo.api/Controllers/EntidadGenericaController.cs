using apigenerica.primitivas;
using Microsoft.AspNetCore.Mvc;

namespace espaciotrabajo.api.Controllers;
[ApiController]
public class EntidadGenericaController : ControladorEntidadGenerico
{
    private ILogger<EntidadGenericaController> _logger;

    public EntidadGenericaController(ILogger<EntidadGenericaController> logger, IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
    {
        _logger = logger;
    }
}
