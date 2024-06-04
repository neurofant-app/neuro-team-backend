using System.Diagnostics.CodeAnalysis;

namespace seguridad.modelo;

/// <summary>
/// Define aun usuario perteneciente aun grupo
/// </summary>
[ExcludeFromCodeCoverage]
public class UsuarioGrupo
{
    /// <summary>
    /// Identificador único del usuario
    /// </summary>
    public required string UsuarioId { get; set; }
}
