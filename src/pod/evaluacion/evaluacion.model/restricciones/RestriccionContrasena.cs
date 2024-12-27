using System.Diagnostics.CodeAnalysis;

namespace evaluacion.model.restricciones;

/// <summary>
/// Define una restricción de acceso por cotnrasena
/// </summary>
[ExcludeFromCodeCoverage]
public class RestriccionContrasena
{
    /// <summary>
    /// Hash de la contraseña de acceso
    /// </summary>
    public required string Contrasena { get; set; }
}
