using apigenerica.model.servicios;
using controlescolar.modelo.campi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace controlescolar.servicios.entidadcampus;

public interface IServicioEntidadCampus : IServicioEntidadGenerica<EntidadCampus, CreaCampus,ActualizaCampus, ConsultaCampusCuenta,string>
{
}
