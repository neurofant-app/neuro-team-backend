using modelo.repositorio.cfdi.busqqueda;
using modelo.repositorio.cfdi.busqueda;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace modelo.repositorio.cfdi
{
    public interface IServicioConsultaCFDI
    {
        Task<PaginaDatos<CFDI>> Buscar(Consulta consulta);
        Task CreaDatosDemo();
    }
}
