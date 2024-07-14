namespace aprendizaje.model;

/// <summary>
/// Estado de publicación del temario
/// </summary>
public enum EstadoTemario
{
    /// <summary>
    /// El temario si ecuentra en edición
    /// </summary>
    EnEdicion = 0,
    /// <summary>
    /// El temario ha sido publicado y no puede ser modificado
    /// </summary>
    Publicado = 1,
    /// <summary>
    /// El temario ha sido marcado como obsoleto y no debe utilizarse
    /// </summary>
    Obsoleto = 2
}


/// <summary>
/// Estado de publicación de la neurona
/// </summary>
public enum EstadoNeurona
{
    /// <summary>
    /// La neurona si ecuentra en edición
    /// </summary>
    EnEdicion = 0,
    /// <summary>
    /// La neurona ha sido publicado y no puede ser modificado
    /// </summary>
    Publicado = 1,
    /// <summary>
    /// La neurona ha sido marcado como obsoleto y no debe utilizarse
    /// </summary>
    Obsoleto = 2
}


/// <summary>
/// Tipo de licencia de neurona
/// </summary>
public enum TipoLiceciaNeurona
{
    /// <summary>
    /// Produceida por NeuroPad o alguna aplicación 
    /// con fines de ser publicada en el mercado
    /// </summary>
    Comercial = 0,
    /// <summary>
    /// Producida por un usuario final y para ser compartida
    /// con otros usuarios
    /// </summary>
    Usuario = 1,
    /// <summary>
    /// La neurona fue creada como una actividad escolar para uso exclusivo
    /// de un grupo de usuarios
    /// </summary>
    ActividadEscolar = 2
}

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


/// <summary>
/// Tipos de medios para los elementos de aprendizaje
/// </summary>
public enum TipoContenido
{
    /// <summary>
    /// Imagen compatible con web, ya se jpg, gif o png
    /// </summary>
    Imagen = 0,
    /// <summary>
    /// Video compatible con web, ya se mpg, mp4 o oog
    /// </summary>
    Video = 1,
    /// <summary>
    /// Audio compatible con web, ya se mp3 o oog
    /// </summary>
    Audio = 2,
    /// <summary>
    /// Documento PDF
    /// </summary>
    PDF = 3
}

