namespace apigenerica.model.modelos;

/// <summary>
/// Clase base para la implementación de catálogos genéricos
/// </summary>
public class ElementoCatalogo: IElementoCatalogo
{
    /// <summary>
    /// Identificador único de la entrade del catálogo
    /// </summary>
    public virtual string Id { get; set; }

    /// <summary>
    /// Idioma default para el teto descriptivo
    /// </summary>
    public virtual string Idioma { get; set; }

    /// <summary>
    /// Texto default para la entrada de catálogo
    /// </summary>
    public virtual string Texto { get; set; }

    /// <summary>
    /// Identificador único del dominio al que pertenece el elemento
    /// </summary>
    public virtual string DominioId { get; set;}

    /// <summary>
    /// Identificador único de la unidad organizacional a la que pertenece el elemento
    /// </summary>
    public virtual string UnidadOrganizacionalId { get; set; }


    /// <summary>
    /// Identificador único del catálogo al que pertenece el elemento
    /// </summary>
    public virtual string CatalogoId { get; set; }


    /// <summary>
    /// Lista de traducciones del catálogo
    /// </summary>
    public virtual List<I18NCatalogo> Traducciones { get; set; }

}
