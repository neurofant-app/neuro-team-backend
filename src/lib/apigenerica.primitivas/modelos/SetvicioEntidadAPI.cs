namespace apigenerica.primitivas;

/// <summary>
/// Elemento en memoria para el mapeo de servicio en la API generica
/// </summary>
public class ServicioEntidadAPI
{
    /// <summary>
    /// Ruta del ensamblado
    /// </summary>
    public string Ruta { get; set; }

    /// <summary>
    /// Nombre de la propiead del ruteo en la API generica
    /// </summary>
    public string NombreRuteo { get; set; }

    /// <summary>
    /// Nombre commpleto del ensamblado
    /// </summary>
    public string NombreEnsamblado { get; set; }


    /// <summary>
    /// Nombre del contexto con el que trabaja el servico
    /// </summary>
    public string? Driver { get; set; }
}
