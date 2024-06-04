namespace extensibilidad.metadatos.validadores;

/// <summary>
/// Validador de propeidades decimales
/// </summary>
/// <param name="minimo">Valor mínimo, nulo para ignorar</param>
/// <param name="maximo">Valor máximo, nulo para ignorar</param>
[AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
public class ValidarDecimalAttribute(string minimo = "", string maximo = "") : Attribute
{
    private readonly decimal? _minimo = string.IsNullOrEmpty(minimo) ? null : int.Parse(minimo);
    private readonly decimal? _maximo = string.IsNullOrEmpty(maximo) ? null : int.Parse(maximo);


    /// <summary>
    /// Valor mínimo aceptable, si es nulo no se valúa
    /// </summary>
    public decimal? Minimo { get { return _minimo; } }

    /// <summary>
    /// Valor máximo aceptable, si es nulo no se valúa
    /// </summary>
    public decimal? Maximo { get { return _maximo; } }
}
