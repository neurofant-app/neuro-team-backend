namespace comunicaciones.model;

/// <summary>
/// Canales de comunicación por el que se envían los mensajes
/// </summary>
public enum TipoCanal
{
    CorreoElectronico = 0,
    WhatsApp= 1,
    SMS = 2,
    Telegram = 3,
    Web = 4,
    API = 5
}

/// <summary>
/// Describe quién recibirá el mensaje.
/// </summary>
public enum TipoParticipante
{
    /// <summary>
    /// Corresponde a una persona
    /// </summary>
    Persona = 0,
    /// <summary>
    /// Es una aplicación de la plataforma o externa
    /// </summary>
    Aplicacion = 1,
    /// <summary>
    /// Robot de conversación
    /// </summary>
    Bot = 2,
}

public enum TipoVigencia
{
    Vigencia = 0,
    SinVigencia = 1,
    Mensual = 2,
    Trimestral = 3
}

