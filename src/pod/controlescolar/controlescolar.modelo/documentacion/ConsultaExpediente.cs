using controlescolar.modelo.comunes;
using System.Diagnostics.CodeAnalysis;

namespace controlescolar.modelo.documentacion;

/// <summary>
/// DTO para la creación de un expediente
/// </summary>
[ExcludeFromCodeCoverage]
public class ConsultaExpediente
{
    /// <summary>
    /// Identificador único del expediente
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// Nombre asigando al expediente
    /// </summary>
    public required List<ValorI18N<string>> Nombre { get; set; }

    /// <summary>
    /// Descripcion del expediente
    /// </summary>
    public required List<ValorI18N<string>>? Descripcion { get; set; } = [];

    /// <summary>
    /// Fecha de creación del expediente
    /// </summary>
    public DateTime FechaCreacion { get; set; }

    /// <summary>
    /// Determina si el expediente se encuentra activo para su uso
    /// </summary>
    public bool Activo { get; set; } = true;

    /// <summary>
    /// Identificador del rol a que pertenece el expediente
    /// </summary>
    public long RolEscolarId { get; set; }

}
