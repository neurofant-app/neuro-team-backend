namespace controlescolar.modelo.rolesescolares;

/// <summary>
/// Tipo de movimientos para los roles
/// </summary>
public enum TipoMovimientoRol
{
    NoDefinido = 0,
    Alta = 1,
    BajaTemporal = 2,
    BajaDefinitiva = 3,
    Permiso = 4,
    Trapaso = 5,
}

/// <summary>
/// DEtermina que ocurre cuando se aplica un movimiento al vínculo del rol escolar
/// </summary>
public enum TipoActualizacionVinculo { 
    /// <summary>
    ///  No hay cambios en el estado del vínculo
    /// </summary>
    Ninguna = 0,
    /// <summary>
    /// Inactiva el vínculo para la persona
    /// </summary>
    Inactivar = 1,
    /// <summary>
    /// Activa el vínculo para la persona
    /// </summary>
    Activar = 2,
}