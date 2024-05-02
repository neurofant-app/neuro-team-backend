namespace apigenerica.model.modelos;

/// <summary>
/// DTO para la actualización de un elemento de catálogo
/// </summary>
public class ElementoCatalogoActualizar
{    /// <summary>
     /// Identificador único de la entrade del catálogo
     /// </summary>
    public virtual string Id { get; set; }

    /// <summary>
    /// Idioma para el teto descriptivo
    /// </summary>
    public virtual string Idioma { get; set; }

    /// <summary>
    /// Texto para la entrada de catálogo
    /// </summary>
    public virtual string Texto { get; set; }
}
