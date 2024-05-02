namespace aplicaciones.model;

/// <summary>
/// Estado de la invitación
/// </summary>
public enum EstadoInvitacion {
    Nueva=0,
    Enviada=1,
    Confirmada=2,
    ErrorDeEnvio=4
}
     
public enum TipoContenido
{
    Invitacion = 1,
    RecuperacionPassword = 2,
}

/// <summary>
/// Tipos de comunicaciones enviadas por la aplicacion
/// </summary>
public enum TipoComunicacion
{
    Registro = 0,
    RecuperacionContrasena = 1
}

public enum TipoLogo
{
    PrincipalCircular = 1,
    EncabezadoRectangular = 2 
}


public enum TipoConsentimiento { 
    Registro = 1
}