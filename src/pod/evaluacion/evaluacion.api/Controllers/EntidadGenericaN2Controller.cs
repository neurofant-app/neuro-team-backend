using apigenerica.primitivas;

namespace evaluacion.api.Controllers;

public class EntidadGenericaN2Controller : ControladorGenericoN2
{
    ILogger<EntidadGenericaN2Controller> _logger;
    public EntidadGenericaN2Controller(ILogger<EntidadGenericaN2Controller> logger, IHttpContextAccessor httpContextAccessor) : base(logger, httpContextAccessor)
    {
        _logger = logger;
    }
}
