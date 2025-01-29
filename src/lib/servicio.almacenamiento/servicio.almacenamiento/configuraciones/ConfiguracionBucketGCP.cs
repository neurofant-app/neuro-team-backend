using System.Diagnostics.CodeAnalysis;

namespace servicio.almacenamiento.configuraciones;

/// <summary>
/// Define la configuracion necesario para el almacenamieno en un bucket de GCP
/// tal cual será almacenada en el repositorio
/// </summary>
[ExcludeFromCodeCoverage]
public class ConfiguracionBucketGCP
{
    /// <summary>
    /// Nombre del bucket de GCP
    /// </summary>
    public required string Bucket { get; set; }

    /// <summary>
    /// Identificador único del proyecto de GCP
    /// </summary>
    public string? ProyectoGCP { get; set; }

}
