using apigenerica.primitivas;
using Microsoft.AspNetCore.Http;

namespace aprendizaje.api.Controllers;

public class EntidadGenericaHijoController : ControladorEntidadHijoGenerico
{
    private ILogger<EntidadGenericaHijoController> _logger;

    public EntidadGenericaHijoController(ILogger<EntidadGenericaHijoController> logger, IHttpContextAccessor httpContextAccesor) : base(httpContextAccesor)
    {
        _logger = logger;
    }
}
