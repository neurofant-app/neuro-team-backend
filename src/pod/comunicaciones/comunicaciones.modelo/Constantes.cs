
namespace comunicaciones.modelo;

public class Constantes
{
    /// <summary>
    /// Este enum es necesario para saber que tipo de información será enviada y comunicarse con
    /// el endpoint correspondiente en el controlador de WhatsApp del proyecto de comunicaciones.api
    /// </summary>
    public enum TipoMensaje
    {
        archivo = 0,
        texto = 1,
        img = 2
    }
}
