using comunes.primitivas.I18N;

namespace aplicaciones.model;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

/// <summary>
/// Logo perteneciente a una aplciación
/// </summary>
public class LogoAplicacion : IInternacionalizable
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
    public TipoLogo Tipo { get; set; }
    // R


    /// <summary>
    /// Idioma del recurso
    /// </summary>
    public string Idioma { get; set; }
    // R 10


    /// <summary>
    /// Determina si el logo debe considerarce el default para un idioma no reconocido
    /// </summary>
    public bool IdiomaDefault { get; set; }
    // R 

    /// <summary>tua
    /// Url del logotipo o imagen compatible con navegadores en Base64 
    /// </summary>
    public string? LogoURLBase64{ get; set; }
    // R MAX

    /// <summary>
    /// Determina si el logo almacenaodo se encuentra en formato vectorial
    /// </summary>
    public bool EsSVG { get; set; } = false;    
    // R

    /// <summary>
    /// DEtermina si el logo es una URL pública
    /// </summary>
    public bool EsUrl { get; set; } = false;
    // R

    /// <summary>
    /// Aplicacion relacionada
    /// </summary>
    public Aplicacion Aplicacion { get; set; }

}

#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
