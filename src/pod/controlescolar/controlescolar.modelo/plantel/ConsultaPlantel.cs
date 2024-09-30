using System.Diagnostics.CodeAnalysis;

namespace controlescolar.modelo.escuela;

/// <summary>
/// Elementos a obtener en la cosulta de planteles
/// </summary>
[ExcludeFromCodeCoverage]
public class ConsultaPlantel
{
    // <summary>
    /// Identificador único del plantel
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Nombre del plantel
    /// </summary>
    public required string Nombre { get; set; }

    /// <summary>
    /// Determina si el plantel se encuentra activo o inactivo
    /// </summary>
    public required bool Activo { get; set; } = true;

    /// <summary>
    /// Fecha de creación del registro
    /// </summary>
    public required DateTime Creacion { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Identificador único la escuela a la que pertenece el plantel
    /// </summary>
    public Guid EscuelaId { get; set; }

    /// <summary>
    /// Clave del plantek para uso local por ejemplo del sistema escolar nacional
    /// </summary>
    public string? Clave { get; set; }
}
