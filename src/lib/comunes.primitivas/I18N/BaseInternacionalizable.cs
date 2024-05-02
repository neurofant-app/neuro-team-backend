namespace comunes.primitivas.I18N;

/// <summary>
/// Propuiedades bases para la internacionalizacion
/// </summary>
public interface IInternacionalizable
{
    /// <summary>
    /// Idioma del rescurso
    /// </summary>
    string Idioma { get; set; }

    /// <summary>
    /// Determina si el recurso debe considerarce el default para un idioma no reconocido
    /// </summary>
    bool IdiomaDefault { get; set; }
}
