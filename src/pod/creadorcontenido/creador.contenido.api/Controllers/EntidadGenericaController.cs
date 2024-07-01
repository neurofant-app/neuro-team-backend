using apigenerica.primitivas;

namespace creador.contenido.api.Controllers;


public class EntidadGenericaController : ControladorEntidadGenerico
{
    private ILogger<EntidadGenericaController> _logger;

    public EntidadGenericaController(ILogger<EntidadGenericaController> logger, IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
    {
        _logger = logger;
    }

}