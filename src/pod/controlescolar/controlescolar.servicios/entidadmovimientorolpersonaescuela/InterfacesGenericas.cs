using apigenerica.model.servicios;
using comunes.primitivas;
using controlescolar.modelo.rolesescolares;
using System.Collections.Specialized;

namespace controlescolar.servicios.entidadmovimientorolpersonaescuela;

public interface IServicioMovimientoEntidadRolPersonaEscuela : IServicioEntidadGenerica<EntidadMovimientoRolPersonaEscuela, CreaMovimientoRolPersonaEscuela, ActualizaMovimientoRolPersonaEscuela, ConsultaMovimientoRolPersonaEscuela, string>
{
    Task<RespuestaPayload<EntidadMovimientoRolPersonaEscuela>> CalculandoIdMax(EntidadMovimientoRolPersonaEscuela entidadDocumentoBase, StringDictionary parametros);
}
