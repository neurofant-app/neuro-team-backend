using apigenerica.primitivas;
using Microsoft.AspNetCore.Mvc;

namespace disenocurricular.api.Controllers;
[ApiController]
public class EntidadGenericaHijoController : ControladorEntidadHijoGenerico
{
    private ILogger<EntidadGenericaHijoController> _logger;

    public EntidadGenericaHijoController(ILogger<EntidadGenericaHijoController> logger, IHttpContextAccessor httpContextAccesor) : base(httpContextAccesor)
    {
        _logger = logger;
    }
}
