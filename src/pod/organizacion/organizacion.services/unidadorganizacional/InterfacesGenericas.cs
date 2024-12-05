using apigenerica.model.servicios;
using organizacion.model.unidadorganizacional;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace organizacion.services.unidadorganizacional;

public interface IServicioUnidadOrganizacional : IServicioEntidadGenerica<UnidadOrganizacional, UnidadOrganizacionalInsertar, UnidadOrganizacionalActualizar, UnidadOrganizacionalDespliegue, Guid>
{

}
