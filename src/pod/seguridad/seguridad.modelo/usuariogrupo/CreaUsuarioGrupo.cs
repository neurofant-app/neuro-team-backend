using System.Diagnostics.CodeAnalysis;

namespace seguridad.modelo.roles;

[ExcludeFromCodeCoverage]
public class CreaUsuarioGrupo
{
    /// <summary>
    /// Identificador del usuario
    /// </summary>
    public required string UsuarioId { get; set; }
}
