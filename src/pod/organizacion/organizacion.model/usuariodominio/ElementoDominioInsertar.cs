using System.Diagnostics.CodeAnalysis;

namespace organizacion.model.usuariodominio;

/// <summary>
/// Define los vinculos de un usuario con un dominio
/// </summary>
[ExcludeFromCodeCoverage]
public class ElementoDominioInsertar
{
    /// <summary>
    /// Identificador único usuario 
    /// </summary>
    public Guid UsurioId { get; set; }

    /// <summary>
    /// Indica si el usuario está activo en el dominio, si el usuario se encuentra inactivo también lo estará para todas las UO
    /// </summary>
    public bool? Activo { get; set; } = true;

}
