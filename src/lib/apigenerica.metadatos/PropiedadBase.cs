namespace extensibilidad.metadatos;

/// <summary>
/// Elemento básicos de una propiedad
/// </summary>
public abstract class PropiedadBase
{
    /// <summary>
    /// Nombre de la propiedad o clave para internacionalización en la UI
    /// </summary>
    public string Nombre { get; set; }

    /// <summary>
    /// Tipo de datos para la propiedad
    /// </summary>
    public TipoDatos Tipo { get; set; }

    /// <summary>
    /// Determina si el valor es requerido para su llenado
    /// </summary>
    public bool Requerida { get; set; }

    /// <summary>
    /// Determina si la propiedad acepta valores nulos
    /// </summary>
    public bool Nullable { get; set; }


    /// <summary>
    /// Define si la propiedad acepta valores en forma de arreglos
    /// </summary>
    public bool ValoresMultiples { get; set; } = false;

    /// <summary>
    /// Elementos de la lista para el tipo lista
    /// </summary>
    public Lista? Lista { get; set; }
}
