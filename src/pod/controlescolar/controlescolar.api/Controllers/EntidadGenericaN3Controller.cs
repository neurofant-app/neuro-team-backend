using apigenerica.primitivas;

namespace controlescolar.api.Controllers;

public class EntidadGenericaN3Controller : ControladorGenericoN3
{
    ILogger<EntidadGenericaN3Controller> _logger;
    public EntidadGenericaN3Controller(ILogger<EntidadGenericaN3Controller> logger, IHttpContextAccessor httpContextAccessor) : base(logger, httpContextAccessor)
    {
        this._logger = logger;
    }
}
