using System.Diagnostics.CodeAnalysis;

namespace controlescolar.modelo.escuela;

/// <summary>
/// DTO de API para la creación de una escuela
/// </summary>
[ExcludeFromCodeCoverage]
public class CreaEscuela
{
    /// <summary>
    /// Nombre de la escuela
    /// </summary>
    public required string Nombre { get; set; }

    /// <summary>
    /// Clave de la escuela para uso local por ejemplo del sistema escolar nacional
    /// </summary>
    public string? Clave { get; set; }
}
