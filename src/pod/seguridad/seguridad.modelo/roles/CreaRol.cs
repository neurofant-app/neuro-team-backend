using System.Diagnostics.CodeAnalysis;

namespace seguridad.modelo.roles;

[ExcludeFromCodeCoverage]
public class CreaRol
{
    /// <summary>
    /// Nombre del rol para la UI, esto será calcolado en base al idioma o bien al crear roles personalizados
    /// </summary>
    public required string Nombre { get; set; }

    /// <summary>
    /// Descripción del rol para la UI, esto será calcolado en base al idioma o bien al crear roles personalizados
    /// </summary>
    public string? Descripcion { get; set; }
}
