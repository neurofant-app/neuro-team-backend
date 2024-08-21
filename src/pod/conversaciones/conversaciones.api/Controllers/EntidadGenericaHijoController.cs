using apigenerica.primitivas;

namespace conversaciones.api.Controllers;
public class EntidadGenericaHijoController : ControladorEntidadHijoGenerico
{
    private ILogger<EntidadGenericaHijoController> _logger;

    public EntidadGenericaHijoController(ILogger<EntidadGenericaHijoController> logger, IHttpContextAccessor httpContextAccesor) : base(httpContextAccesor)
    {
        _logger = logger;
    }
}
