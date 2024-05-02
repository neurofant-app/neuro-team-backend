namespace apigenerica.model.modelos;

/// <summary>
/// DTO Para la creación de un elemento de catálog
/// </summary>
public class ElementoCatalogoInsertar
{    
    /// <summary>
    /// Idioma para el teto descriptivo
    /// </summary>
    public virtual string Idioma { get; set; }

    /// <summary>
    /// Texto para la entrada de catálogo
    /// </summary>
    public virtual string Texto { get; set; }
}
