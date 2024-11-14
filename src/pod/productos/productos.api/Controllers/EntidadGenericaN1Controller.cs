using apigenerica.primitivas;
using Microsoft.AspNetCore.Mvc;

namespace productos.api.Controllers;
[ApiController]
public class EntidadGenericaN1Controller : ControladorGenericoN1
{
    ILogger<EntidadGenericaN1Controller> _logger;
    public EntidadGenericaN1Controller(ILogger<EntidadGenericaN1Controller> logger, IHttpContextAccessor httpContextAccessor) : base(logger, httpContextAccessor)
    {
        _logger = logger;
    }
}
