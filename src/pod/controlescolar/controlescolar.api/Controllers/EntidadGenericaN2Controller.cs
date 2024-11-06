using apigenerica.primitivas;
using Microsoft.AspNetCore.Mvc;

namespace controlescolar.api.Controllers;
[ApiController]
public class EntidadGenericaN2Controller : ControladorGenericoN2
{
    private ILogger<EntidadGenericaN2Controller> _logger;

    public EntidadGenericaN2Controller(ILogger<EntidadGenericaN2Controller> logger, IHttpContextAccessor httpContextAccessor) : base(logger,httpContextAccessor)
    {
        _logger = logger;
    }
}
