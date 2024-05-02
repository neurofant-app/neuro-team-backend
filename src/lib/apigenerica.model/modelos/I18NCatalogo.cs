namespace apigenerica.model.modelos;

/// <summary>
/// Entrada de internacionalizaicón del catálogo
/// </summary>
public class I18NCatalogo
{
    /// <summary>
    /// IDentificador del id de catalaogo
    /// </summary>
    public string Id { get; set; }

    /// <summary>
    /// Idioma para el teto descriptivo
    /// </summary>
    public virtual string Idioma { get; set; }

    /// <summary>
    /// Texto para la entrada de catálogo
    /// </summary>
    public virtual string Texto { get; set; }

    /// <summary>
    /// Identificador único del dominio al que pertenece el elemento
    /// </summary>
    public virtual string DominioId { get; set; }

    /// <summary>
    /// Identificador único de la unidad organizacional a la que pertenece el elemento
    /// </summary>
    public virtual string UnidadOrganizacionalId { get; set; }

    /// <summary>
    /// Identificador único del catálogo al que pertenece el elemento
    /// </summary>
    public virtual string CatalogoId { get; set; }
}
