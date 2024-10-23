using System.Diagnostics.CodeAnalysis;

namespace evaluacion.model.impresos;

/// <summary>
/// Representación impresa de una evaluacion
/// </summary>
[ExcludeFromCodeCoverage]
public class Impreso
{
    /// <summary>
    /// Identificador único de la representación impresa
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Identificador único de la evaluación a la que pertenece el impreso
    /// </summary>
    public Guid EvaluacionId { get; set; }

    /// <summary>
    /// Formato físico de la página
    /// </summary>
    public FormatoPagina FormatoPagina { get; set; } = new FormatoPagina();


}
