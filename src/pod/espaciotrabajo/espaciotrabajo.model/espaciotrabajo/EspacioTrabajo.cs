using comunes.primitivas.atributos;
using System.Diagnostics.CodeAnalysis;

namespace espaciotrabajo.model.espaciotrabajo;

/// <summary>
/// Define un espacio de trabajo para los creadores de aprendizaje
/// </summary>
[ExcludeFromCodeCoverage]
[EntidadDB]
public class EspacioTrabajo
{
    /// <summary>
    /// Identificador único del espacio de trabajo
    /// </summary>
    public Guid? Id { get; set; }

    /// <summary>
    /// Nombre del espacio de trabajo
    /// </summary>
    public required string Nombre { get; set; }
}
