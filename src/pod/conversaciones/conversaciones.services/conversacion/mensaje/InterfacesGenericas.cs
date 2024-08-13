using apigenerica.model.servicios;
using conversaciones.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace conversaciones.services.conversacion.mensaje;

public interface IServicioMensaje : IServicioEntidadHijoGenerica<Mensaje, Mensaje, Mensaje, Mensaje, string, string>
{
}