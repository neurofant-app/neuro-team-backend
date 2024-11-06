using System.Diagnostics.CodeAnalysis;

namespace controlescolar.modelo.plantel;

/// <summary>
/// DTO de API para la creación del plantel
/// </summary>
[ExcludeFromCodeCoverage]
public class CreaPlantel
{

    /// <summary>
    /// Id de la escuela a la que pertenece el plantel
    /// </summary>
    public Guid EscuelaId { get; set; }

    /// <summary>
    /// Nombre del plantel
    /// </summary>
    public required string Nombre { get; set; }

    /// <summary>
    /// Clave del plantek para uso local por ejemplo del sistema escolar nacional
    /// </summary>
    public string? Clave { get; set; }
}
