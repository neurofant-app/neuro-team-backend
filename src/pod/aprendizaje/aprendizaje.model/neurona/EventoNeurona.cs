using System.Diagnostics.CodeAnalysis;

namespace aprendizaje.model.neurona;

/// <summary>
/// Define un evento de actualización de una neurona publicada previamente
/// </summary>
[ExcludeFromCodeCoverage]
public class EventoNeurona
{
    /// <summary>
    /// Identificador único del evento
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Fecha del evento de actualización UTC
    /// </summary>
    public DateTime Fecha { get; set; }

    /// <summary>
    /// Tipo de evento ocurrido para la neurona
    /// </summary>
    public TipoEventoNeurona TipoEvento { get; set; }


    /// <summary>
    /// Comentarios de la actualización
    /// </summary>
    public List<ValorI18N<string>> Comentarios { get; set; } = [];
}
