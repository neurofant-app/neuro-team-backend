namespace comunes.almacenamiento;

/// <summary>
/// Tipo de almacenamiento 
/// </summary>
public enum TipoAlmacenamiento {
    BucketGCP = 0
}

/// <summary>
/// Estados de lectura del almacenamiento
/// </summary>
public enum EstadoAlmacenamiento { 
    /// <summary>
    /// Es posible realizar operaciones de lectura y escritoa 
    /// </summary>
    LectoEscritura = 0,
    /// <summary>
    /// Sólo es posible realizar operaciones de lectura 
    /// </summary>
    SoloLectura = 1
}


/// <summary>
/// Clase de almacenamiento 
/// </summary>
public enum ClaseAlmacenamiento
{
    /// <summary>
    /// Acceso continuo
    /// </summary>
    Estandar= 0,
    /// <summary>
    /// Acceso a 30 días
    /// </summary>
    Nearline=1,
    /// <summary>
    /// Acceso a 90 días
    /// </summary>
    Coldline = 2,
    /// <summary>
    /// Acceso a 365 días
    /// </summary>
    Archive = 3
}


/// <summary>
/// Distingue si alguna propiedad se almacena como secreto de plataforma
/// </summary>
public enum TipoSecreto
{
    /// <summary>
    /// No se almacena como secreto
    /// </summary>
    Ninguno = 0,
    /// <summary>
    /// El secreto se hospeda en GCP
    /// </summary>
    SecretoGCP = 1
}
    