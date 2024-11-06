using apigenerica.model.servicios;
using comunes.primitivas;
using controlescolar.modelo.documentacion;
using System.Collections.Specialized;

namespace controlescolar.servicios.entidaddocumentobase;

public interface IServicioEntidadDocumentoBase : IServicioEntidadGenerica<EntidadDocumentoBase, CreaDocumentoBase, ActualizaDocumentoBase, ConsultaDocumentoBase, string>
{
    Task<RespuestaPayload<EntidadDocumentoBase>> CalculandoIdMax(EntidadDocumentoBase entidadDocumentoBase, StringDictionary parametros);
}
