using apigenerica.primitivas;

namespace organizacion.api.Controllers;

public class EntidadGenericaN3Controller : ControladorGenericoN3
{
    ILogger<EntidadGenericaN3Controller> _logger;
    public EntidadGenericaN3Controller(ILogger<EntidadGenericaN3Controller> logger, IHttpContextAccessor httpContextAccessor) : base(logger, httpContextAccessor)
    {
        _logger = logger;
    }
}
