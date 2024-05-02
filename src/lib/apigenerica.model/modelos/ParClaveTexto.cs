namespace apigenerica.model.modelos;

/// <summary>
/// Estructura genérica para devolver pares valore texto por ejemplo para catálogos
/// </summary>
public class ParClaveTexto
{
    /// <summary>
    /// Identificador único en un conjunto de pares
    /// </summary>
    public string Clave { get; set; }

    /// <summary>
    /// Texto descriptivo asociado a la clave
    /// </summary>
    public string Texto { get; set; }
}
