using apigenerica.model.servicios;
using organizacion.model.dominio;

namespace organizacion.services.dominio;

public interface IServicioDominio : IServicioEntidadGenerica<Dominio, DominioInsertar, DominioActualizar, DominioDespliegue, Guid>
{
}
