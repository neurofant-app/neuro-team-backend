namespace extensibilidad.metadatos.validadores;

/// <summary>
/// Validador de datos enteros
/// </summary>
/// <param name="minimo">Valor mínimo, nulo para ignorar</param>
/// <param name="maximo">Valor máxicmo, nulo para ignorar</param>
[AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
public class ValidarEnteroAttribute(string minimo = "", string maximo = "") : Attribute
{
    private readonly int? _minimo = string.IsNullOrEmpty(minimo) ? null : int.Parse(minimo);
    private readonly int? _maximo = string.IsNullOrEmpty(maximo) ? null : int.Parse(maximo);

    /// <summary>
    /// Valor mínimo aceptable, si es nulo no se valúa
    /// </summary>
    public int? Minimo { get { return _minimo; } }

    /// <summary>
    /// Valor máximo aceptable, si es nulo no se valúa
    /// </summary>
    public int? Maximo { get { return _maximo; } }
}