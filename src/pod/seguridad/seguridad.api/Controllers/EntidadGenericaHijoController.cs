using apigenerica.primitivas;
using Microsoft.AspNetCore.Mvc;

namespace seguridad.api.Controllers;

public class EntidadGenericaHijoController : ControladorEntidadHijoGenerico
{
    private ILogger<EntidadGenericaHijoController> _logger;

    public EntidadGenericaHijoController(ILogger<EntidadGenericaHijoController> logger, IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
    {
        _logger = logger;
    }
}
