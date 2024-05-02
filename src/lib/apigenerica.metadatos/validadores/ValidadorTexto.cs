namespace extensibilidad.metadatos.validadores;

public class ValidadorTexto
{
    /// <summary>
    /// Longitud mínima del texto de la propiedad, si es nulo no se evalúa
    /// </summary>
    public int? LongitudMinima { get; set; }

    /// <summary>
    /// Longitud máxima del texto de la propiedad, si es nulo no se evalúa
    /// </summary>
    public int? LongitudMaxima { get; set; }

    /// <summary>
    /// Expresión regular para validar, si es nulo no se evalúa
    /// </summary>
    public string? RegExp { get; set; }
}