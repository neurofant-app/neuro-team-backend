using System.Diagnostics.CodeAnalysis;

namespace servicio.almacenamiento.configuraciones;

/// <summary>
/// Define la configuracion necesario para el almacenamieno en filesystem local 
/// tal cual será almacenada en el repositorio
/// </summary>
[ExcludeFromCodeCoverage]
public class ConfiguracionFilesystemLocal
{
    /// <summary>
    /// Ruta base del almacenamiento local
    /// </summary>
    public required string Ruta { get; set; }


}
