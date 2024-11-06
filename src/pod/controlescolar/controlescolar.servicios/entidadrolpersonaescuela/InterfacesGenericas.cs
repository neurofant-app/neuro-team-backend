using apigenerica.model.servicios;
using comunes.primitivas;
using controlescolar.modelo.rolesescolares;
using System.Collections.Specialized;

namespace controlescolar.servicios.entidadrolpersonaescuela;

public interface IServicioEntidadRolPersonaEscuela : IServicioEntidadGenerica<EntidadRolPersonaEscuela, CreaRolPersonaEscuela, ActualizaRolPersonaEscuela, ConsultaRolPersonaEscuela, string>
{
    Task<RespuestaPayload<EntidadRolPersonaEscuela>> CalculandoIdMax(EntidadRolPersonaEscuela entidadDocumentoBase, StringDictionary parametros);
}
