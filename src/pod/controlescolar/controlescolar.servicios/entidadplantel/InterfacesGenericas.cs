using apigenerica.model.servicios;
using controlescolar.modelo.escuela;
using controlescolar.modelo.plantel;

namespace controlescolar.servicios.entidadplantel;

public interface IServicioEntidadPlantel : IServicioEntidadGenerica<EntidadPlantel, CreaPlantel, ActualizaPlantel, ConsultaPlantel, string>
{
}
