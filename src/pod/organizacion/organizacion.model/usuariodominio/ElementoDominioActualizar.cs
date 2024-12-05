using System.Diagnostics.CodeAnalysis;

namespace organizacion.model.usuariodominio;

/// <summary>
/// Define los vinculos de un usuario con un dominio
/// </summary>
[ExcludeFromCodeCoverage]
public class ElementoDominioActualizar
{
    /// <summary>
    /// Identificador único del usuario
    /// </summary>
    public Guid UsuarioId { get; set; }
    /// <summary>
    /// Identificador único del dominio al que pertenece el usuario
    /// </summary>
    public bool? Activo { get; set; } = true;

}
