
using static comunicaciones.modelo.Constantes;

namespace comunicaciones.modelo.whatsapp;

public class MensajeWhatsapp
{
    /// <summary>
    /// Numero al que se envia el mensaje
    /// </summary>
    public string Telefono { get; set; }
    /// <summary>
    /// Contenido del mensaje
    /// </summary>
    public string Mensaje { get; set; }
    /// <summary>
    /// Tipo de mensaje archivo, texto, img, etc..
    /// </summary>
    public TipoMensaje Tipo { get; set; }
}
