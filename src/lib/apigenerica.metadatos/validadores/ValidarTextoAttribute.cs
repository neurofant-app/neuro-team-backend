namespace extensibilidad.metadatos.validadores;

/// <summary>
/// Validador de propiedades de texto
/// </summary>
/// <param name="longitudMinima">Longitus mínima, nulo para ignorar</param>
/// <param name="longitudMaxima">Longitus maxima, nulo para ignorar</param>
/// <param name="regexp">Expresión regular, nulo para ignorar</param>
[AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
public class ValidarTextoAttribute(string longitudMinima = "", string longitudMaxima = "", string? regexp = "") : Attribute
{

    private readonly int? _longitudMinima = string.IsNullOrEmpty(longitudMinima) ? null : int.Parse(longitudMinima);

    private readonly int? _longitudMaxima = string.IsNullOrEmpty(longitudMaxima) ? null : int.Parse(longitudMaxima);

    private readonly string? _regExp = regexp;

    /// <summary>
    /// Longitud mínima del texto de la propiedad, si es nulo no se evalúa
    /// </summary>
    public virtual int? LongitudMinima
    {
        get { return _longitudMinima; }
    }

    /// <summary>
    /// Longitud máxima del texto de la propiedad, si es nulo no se evalúa
    /// </summary>
    public virtual int? LongitudMaxima
    {
        get { return _longitudMaxima; }
    }

    /// <summary>
    /// Expresión regular para validar, si es nulo no se evalúa
    /// </summary>
    public virtual string? RegExp
    {
        get { return _regExp; }
    }

    public int? NullableParameter { get; set; }
}
