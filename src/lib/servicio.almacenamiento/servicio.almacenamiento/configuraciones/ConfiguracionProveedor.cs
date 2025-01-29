using System.Diagnostics.CodeAnalysis;

namespace servicio.almacenamiento.configuraciones;

/// <summary>
/// Propiedades básicas de un proveedor de configuración, esta entiad base se utiliza
/// para la persistencia y recuperación de los datos del proveedor
/// </summary>
[ExcludeFromCodeCoverage]
public class ConfiguracionProveedor
{
    /// <summary>
    /// Nombre del servicio al que pertenece la configuración
    /// </summary>
    public required string Servicio { get; set; }

    /// <summary>
    /// Identificador del servicio al que pertenece la configuración cuando puedan existir varias
    /// </summary>
    public string? ServicioId { get; set; }

    /// <summary>
    /// Espeficica si la configuracion JSON se encuentra cifrada
    /// </summary>
    public bool PayloadCifrado { get; set; } 

    /// <summary>
    /// Determina si la configuración se encuentra activa
    /// </summary>
    public bool Activa { get; set; }

    /// <summary>
    /// DEtermina si la configración ea la de default
    /// </summary>
    public bool Default { get; set; }

    /// <summary>
    /// Tipo de proveedor de almacenamiento
    /// </summary>
    public TipoProveedorAlmacenamiento Tipo { get; set; }

    /// <summary>
    /// Almacena la configuración específica para el tipo de proveedor de almacenamiento
    /// Esta deberá ser convertida vía la fábrica para crear una instancia útil de proveedor
    /// </summary>
    public  string? ConfiguracionJSON { get; set; }
}
