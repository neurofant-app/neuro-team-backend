using System.Diagnostics.CodeAnalysis;

namespace controlescolar.modelo.plantel;

/// <summary>
/// DTO de API para la actualización de una escuela
/// </summary>
[ExcludeFromCodeCoverage]
public class ActualizaPlantel
{
    /// <summary>
    /// Id del plantel
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Id de la escuela a la que pertenece el plantel
    /// </summary>
    public Guid EscuelaId { get; set; }

    /// <summary>
    /// Nombre del plantel
    /// </summary>
    public required string Nombre { get; set; }

    /// <summary>
    /// Clave del plantel para uso local por ejemplo del sistema escolar nacional
    /// </summary>
    public string? Clave { get; set; }
}
