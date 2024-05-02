namespace comunes.primitivas;

/// <summary>
/// Determina si una operación de validación es exitosa
/// </summary>
public class ResultadoValidacion
{
    /// <summary>
    /// Validación exitosa, se estbalce por defecto como false
    /// </summary>
    public bool Valido { get; set; } = false;

    /// <summary>
    /// Error de validación principal
    /// </summary>
    public ErrorProceso? Error { get; set; }
}
