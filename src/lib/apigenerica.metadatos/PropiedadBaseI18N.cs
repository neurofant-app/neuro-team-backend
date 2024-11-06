namespace extensibilidad.metadatos;

/// <summary>
/// Elemento básicos de una propiedad
/// </summary>
public abstract class PropiedadBaseI18N
{
    /// <summary>
    /// Nombre de la propiedad o clave para internacionalización en la UI
    /// </summary>
    public List<DatoI18N<string>> Nombre { get; set; }

    /// <summary>
    /// Tipo de datos para la propiedad
    /// </summary>
    public TipoDatos Tipo { get; set; }

    /// <summary>
    /// Establace el tipo de control para el despligue en la UI
    /// </summary>
    public TipoDespliegue TipoDespliegue { get; set; }

    /// <summary>
    /// Valor default para la propiedad expresado como el valor serializado json
    /// </summary>
    public string? ValorDefault { get; set; }

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
