using apigenerica.model.servicios;
using comunes.interservicio.primitivas;
using comunes.primitivas;
using espaciotrabajo.model.espaciotrabajo;

namespace espaciotrabajo.services.espaciotrabajo;

public interface IServicioEspacioTrabajo : IServicioEntidadGenerica<EspacioTrabajo, EspacioTrabajo, EspacioTrabajo, EspacioTrabajo, string>
{
    Task<RespuestaPayload<List<EspacioTrabajoUsuario>>> ObtieneEspaciosUsuario(string usuarioId, string dominioId, string unidadOrgId);
}
