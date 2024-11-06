using apigenerica.primitivas;
using Microsoft.AspNetCore.Mvc;

namespace disenocurricular.api.Controllers;
[ApiController]
public class EntidadGenericaN2Controller : ControladorGenericoN2
{
    ILogger<EntidadGenericaN2Controller> _logger;
    public EntidadGenericaN2Controller(ILogger<EntidadGenericaN2Controller> logger, IHttpContextAccessor httpContextAccessor) : base(logger, httpContextAccessor)
    {
        this._logger = logger;
    }   
}
