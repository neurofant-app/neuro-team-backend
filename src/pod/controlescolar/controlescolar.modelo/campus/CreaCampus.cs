using comunes.primitivas.atributos;

namespace controlescolar.modelo.campi;

/// <summary>
/// Datos para la creación de un campus
/// </summary>
[CQRSCrear]
public class CreaCampus: CampusBase
{
    /// <summary>
    /// Referencia al Campus padre.
    /// </summary>
    public virtual Guid? CampusPadreId { get; set; }
}