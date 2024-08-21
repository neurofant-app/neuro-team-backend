using apigenerica.model.servicios;
using comunes.primitivas;
using comunicaciones.modelo;
using conversaciones.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace conversaciones.services.conversacion.mensaje;

public interface IServicioMensaje : IServicioEntidadHijoGenerica<Mensaje, Mensaje, Mensaje, Mensaje, string, string>
{
    public Task<Respuesta> ProcesoWhatsApp(Conversacion conversacion,Mensaje mensaje);
    public Task<Respuesta> ProcesoEmail(Conversacion conversacion, Mensaje mensaje);
    public Task<Respuesta> SeleccionCanal(Conversacion conversacion, Mensaje mensaje);
}