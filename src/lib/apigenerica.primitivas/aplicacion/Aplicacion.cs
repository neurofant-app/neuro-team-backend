

namespace comunes.primitivas.seguridad;

/// <summary>
/// Especifica las propiedades de seguridad de una aplicación
/// </summary>
public class Aplicacion
{

    /// <summary>
    /// Identificador único de la aplicación, este Id será proporcionado por un sistema externo
    /// </summary>
    public virtual required Guid ApplicacionId { get; set; }


    /// <summary>
    /// Modulos de una aplicacion
    /// </summary>
    public List<Modulo> Modulos { get; set; } = [];


    /// <summary>
    /// Nombre del módulo para la UI, esto será calcolado en base al idioa
    /// </summary>
    public string? Nombre { get; set; }

    /// <summary>
    /// Descripción del módulo para la UI, esto será calcolado en base al idioa
    /// </summary>
    public string? Descripcion { get; set; }
}
