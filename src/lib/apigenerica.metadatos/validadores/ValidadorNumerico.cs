namespace extensibilidad.metadatos.validadores;
public class ValidadorNumerico
{
    /// <summary>
    /// Valor mínimo aceptable, si es nulo no se valúa
    /// </summary>
    public decimal? Minimo { get; set; }

    /// <summary>
    /// Valor máximo aceptable, si es nulo no se valúa
    /// </summary>
    public decimal? Maximo { get; set; }
}

