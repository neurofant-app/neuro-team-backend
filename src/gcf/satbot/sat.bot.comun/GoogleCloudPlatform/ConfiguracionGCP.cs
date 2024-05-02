namespace sat.bot.comun.GoogleCloudPlatform;

public class ConfiguracionGCP
{
    /// <summary>
    /// Identificador único del proyecto de GCP
    /// </summary>
    public required string ProyectoId { get; set; }


    /// <summary>
    /// Nombre del bucket para el almacenamiento de archivos
    /// </summary>
    public required string Bucket { get; set; }
}
