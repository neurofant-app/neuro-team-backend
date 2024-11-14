using System.Diagnostics.CodeAnalysis;

namespace organizacion.model.dominio;

/// <summary>
/// El dominio es el contenedor de todos los recursos de una cuenta
/// </summary>
[ExcludeFromCodeCoverage]
public class DominioInsertar
{
    /// <summary>
    /// Nombre de dominio
    /// </summary>
    public string Nombre { get; set; }

}
