using Microsoft.Extensions.Logging;

namespace comunes.primitivas.extensiones;

/// <summary>
/// Define metodos para generar eventos de sistema comunes
/// </summary>
public static class ExtensionesEventos
{

    public static string ERROR_CONFIG="ERROR-CONFIG";

    public static EventId EventoConfiguracionNoValida()
    {
        return new EventId(1000, ERROR_CONFIG);
    }

}
