using System.Diagnostics.CodeAnalysis;

namespace servicio.almacenamiento.configuraciones;

/// <summary>
/// Define la configuracion de la fabrica de configuraciones de almaenamiento
/// </summary>
[ExcludeFromCodeCoverage]
public class ConfiguracionRepositorioProveedorAlmacenamiento
{
    public const string TIPO_REPO_MONGO = "mongo";
    public const string TIPO_REPO_MYSQL = "mysql";
    public const string TIPO_REPO_APPSETTINGS = "appsettings";


    /// <summary>
    /// Tipo del repositorio, por ejemplo MONGO o MYSQL etc
    /// </summary>
    public string? TipoRepositorio { get; set; }

    /// <summary>
    /// Cadena de conexión al repositorio
    /// </summary>
    public string? CadenaConexion { get; set; }

    /// <summary>
    /// Nombre de la base de datos si es requerido
    /// </summary>
    public string? NombreDb { get; set; }

    /// <summary>
    /// Nombre de la coleccion o esquema de la base de datos si es requerido
    /// </summary>
    public string? Coleccion { get; set; }
}
