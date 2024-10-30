using apigenerica.model.servicios;
using comunes.primitivas;
using controlescolar.modelo.documentacion;
using System.Collections.Specialized;

namespace controlescolar.servicios.entidadexpediente;

public interface IServicioEntidadExpediente : IServicioEntidadGenerica<EntidadExpediente, CreaExpediente, ActualizaExpediente, ConsultaExpediente, string>
{
    Task<RespuestaPayload<EntidadExpediente>> CalculandoIdMax(EntidadExpediente entidadExpediente, StringDictionary parametros);
}
