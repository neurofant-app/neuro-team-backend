using System.Diagnostics.CodeAnalysis;

namespace usuario.model;

/// <summary>
/// Nombre del usuario
/// </summary>
[ExcludeFromCodeCoverage]
public class Nombre
{
    /// <summary>
    /// Nombre o nombres de la persona
    /// </summary>
    public string? Nombres { get; set; }
    
    /// <summary>
    /// Inicial del nombre
    /// </summary>
    public string? Inicial { get; set; }
    
    /// <summary>
    /// Primer apellido del nombre
    /// </summary>
    public string? Apellido1 { get; set; }
    
    /// <summary>
    /// Segundo apellido del nombre
    /// </summary>
    public string? Apellido2 { get; set; }
}
