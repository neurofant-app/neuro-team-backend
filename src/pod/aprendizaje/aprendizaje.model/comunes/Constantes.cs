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
/// Estado de la neurona y su contenido asociado
/// </summary>
public enum EstadoContenido
{
    /// <summary>
    /// La neurona si ecuentra en edición
    /// </summary>
    Borraador = 0,
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

/// <summary>
/// Eventos de las neuronas
/// </summary>
public enum TipoEventoNeurona {
    /// <summary>
    /// Indica que la neurona ha sido actualizada
    /// </summary>
    Actualizar =0,
    /// <summary>
    /// Indica que la neurona ha sido publicada
    /// </summary>
    Publicar = 1
}



/// <summary>
/// 
/// </summary>
public enum TipoPrecio
{
    /// <summary>
    /// Sin costo
    /// </summary>
    Gratuita =0,
    /// <summary>
    /// Venta pago único
    /// </summary>
    Venta = 1,
    /// <summary>
    /// Renta por periodo sin límite de tiempo
    /// </summary>
    RentaInfinita = 2,
    /// <summary>
    /// Renta con pago único por un peridos de tiempo
    /// </summary>
    RentAcotada = 3

}


/// <summary>
/// Periodo asociadao cn la renta o venta
/// </summary>
public enum TipoPeriodoRenta
{
    /// <summary>
    /// Pago único para venta o para una RentAcotada
    /// </summary>
    Unico = 0,
    /// <summary>
    /// El cobro es por día
    /// </summary>
    Diario = 1,
    /// <summary>
    /// El cobro es semanal
    /// </summary>
    Semanal = 2,
    /// <summary>
    /// El cobro es mensual
    /// </summary>
    Mensual = 3,
    /// <summary>
    /// El cobro es anual
    /// </summary>
    Anual = 4
}

/// <summary>
/// Pasarela de pago asociada al precio
/// </summary>
public enum TipoPasarelaPago { 
    /// <summary>
    /// pago por transferencia bancaria
    /// </summary>
    TranferenciaBancaria = 0,
    /// <summary>
    /// Pago vía Paypal
    /// </summary>
    Paypal = 1,
    /// <summary>
    /// Pago vía MercadoPago
    /// </summary>
    MercadoPago = 2,
    /// <summary>
    /// Pago vía CoDI
    /// </summary>
    CoDI = 3
}
    