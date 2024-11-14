using System.Diagnostics.CodeAnalysis;

namespace organizacion.model.usuariodominio;

/// <summary>
/// DTO para la inserción de unidades organizacionales en el dominio
/// </summary>
[ExcludeFromCodeCoverage]
public class ElementoOUInsertar
{
    /// <summary>
    /// Identificador único de la unidad organizacional
    /// </summary>
    public Guid OUId { get; set; }

    /// <summary>
    /// IDentifica si la OU se encuentra activa
    /// </summary>
    public bool Activa { get; set; }
}
