using System.Diagnostics.CodeAnalysis;

namespace aprendizaje.model.galeria;

/// <summary>
/// Define temas o tags para los elementos de la galería
/// </summary>
[ExcludeFromCodeCoverage]
public class TemaGaleria
{
    /// <summary>
    /// Identificador único del tema
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Nombre del tema
    /// </summary>
    public required string Nombre { get; set; }
}
