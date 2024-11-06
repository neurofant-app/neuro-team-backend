using System.Diagnostics.CodeAnalysis;

namespace controlescolar.modelo.rolesescolares;

/// <summary>
/// DTO para la creación de un rol escolar para las personas
/// </summary>
[ExcludeFromCodeCoverage]
public class CreaRolPersonaEscuela
{
    /// <summary>
    /// Nombre único del rol
    /// </summary>
    public required string Nombre { get; set; }

    /// <summary>
    /// CLave del rol, puedes er un dentificador externo como del tipo de empleoado en el sistema de nómina
    /// </summary>
    public string? Clave { get; set; }

    /// <summary>
    /// Descripción del rol
    /// </summary>
    public string? Descripcion { get; set; }
}
