using System.Diagnostics.CodeAnalysis;

namespace evaluacion.model.impresos;

/// <summary>
/// Plantilla de impresión 
/// </summary>
[ExcludeFromCodeCoverage]
public class Plantilla
{
    /// <summary>
    /// Identificador único de la plantilla
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Nombre de la plantilla
    /// </summary>
    public required string Nombre { get; set; }

    /// <summary>
    /// IDentificador único del almacenamiento para la plantilla
    /// </summary>
    public Guid? AlmacenamientoId { get; set; }

    /// <summary>
    /// Ruta del objeto en el almacenamiento
    /// </summary>
    public string? Ruta { get; set; }

}
