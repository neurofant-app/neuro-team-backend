using comunes.primitivas;
using comunicaciones.modelo;
using comunicaciones.modelo.whatsapp;
using conversaciones.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace conversaciones.services.proxy.abstractions;

public interface IProxyConversacionComunicaciones
{
    public Task<Respuesta> EnvioCorreo(MensajeEmail data);
    public Task<Respuesta> EnvioWhatsApp(MensajeWhatsapp mensajeWhatsapp);

}
