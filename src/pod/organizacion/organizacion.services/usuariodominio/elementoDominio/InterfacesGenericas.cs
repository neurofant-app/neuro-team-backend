using apigenerica.model.servicios;
using organizacion.model.usuariodominio;

namespace organizacion.services.usuariodominio.elementoDominio;

public interface IServicioUsuarioDominio : IServicioEntidadGenerica<UsuarioDominio, ElementoDominioInsertar, ElementoDominioActualizar, UsuarioDominio, Guid>
{
}
