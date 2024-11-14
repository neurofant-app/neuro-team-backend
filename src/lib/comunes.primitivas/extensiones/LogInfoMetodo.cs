namespace comunes.primitivas.extensiones;

/// <summary>
/// Parámetros para la ejecución de la bitácora
/// </summary>
public struct PrefixLogParams
{
    /// <summary>
    /// Texto base para logs
    /// </summary>
    public string Texto { get; set; }

    /// <summary>
    /// PArámetros del log
    /// </summary>
    public object?[] parametros { get; set; }
}

/// <summary>
/// Información del método ejecutado para bitácora
/// </summary>
public struct LogInfoMetodo
{
    
    public LogInfoMetodo(string? clase, string? nombre)
    {
        Clase = clase;
        Nombre = nombre;
    }

    /// <summary>
    /// Clase a la que pertenece el método
    /// </summary>
    public string? Clase { get; set; }

    /// <summary>
    /// Nombre del método
    /// </summary>
    public string? Nombre { get; set; }
}
