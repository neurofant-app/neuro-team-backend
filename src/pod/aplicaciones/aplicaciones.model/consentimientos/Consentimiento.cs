namespace aplicaciones.model;

public class Consentimiento
{
    /// <summary>
    /// Identificador único del logo
    /// </summary>
    public Guid Id { get; set; }
    // R

    /// <summary>
    /// Identificador único de la aplicación a la que pertenece el logo
    /// </summary>
    public Guid AplicacionId { get; set; }
    // R

    /// <summary>
    /// Tipo de logo
    /// </summary>
    public TipoConsentimiento Tipo { get; set; }

    /// <summary>
    /// Idioma del recurso
    /// </summary>
    public string Idioma { get; set; }
    // 10 R

    /// <summary>
    /// Determina si el logo debe considerarce el default para un idioma no reconocido
    /// </summary>
    public bool IdiomaDefault { get; set; }
    // R

    /// <summary>
    /// Url del logotipo o imagen compatible con navegadores en Base64 
    /// </summary>
    public string Texto { get; set; }
    // MAXIMO R

    /// <summary>
    /// Aplicacion relacionada
    /// </summary>
    public Aplicacion Aplicacion { get; set; }
}
