using System.Diagnostics.CodeAnalysis;

namespace organizacion.model.dominio;

/// <summary>
/// El dominio es el contenedor de todos los recursos de una cuenta
/// </summary>
[ExcludeFromCodeCoverage]
public class DominioActualizar
{
    /// <summary>
    /// Identificador único del dominio
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Nombre de dominio
    /// </summary>
    public string Nombre { get; set; }

    /// <summary>
    /// Indica si el dominio está activo
    /// </summary>
    public bool Activo { get; set; } = true;
}
