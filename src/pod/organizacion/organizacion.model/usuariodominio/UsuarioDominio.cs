using System.Diagnostics.CodeAnalysis;

namespace organizacion.model.usuariodominio;

/// <summary>
/// Entidad de los usuarios asociados al dominio
/// </summary>
[ExcludeFromCodeCoverage]
public class UsuarioDominio
{
    /// <summary>
    /// Identificador único del usuario en el dominio
    /// </summary>
    public Guid UsuarioId { get; set; }

    /// <summary>
    /// Lista de identificadores de los dominios a los que el usuario pertenece
    /// </summary>
    public List<Guid> DominiosId { get; set; } = [];
    // Indexar

    /// <summary>
    /// Lista de dominios a los que el usuario está asociado 
    /// </summary>
    public List<ElementoDominio> Dominios { get; set; } = [];

}
