namespace extensibilidad.metadatos.validadores;

/// <summary>
/// Permite estabkecer valos mínimos ymáximos para las fechas, en el caso de fecha y hora sólo se toma la porción adecuada
/// En el cao de valores FechaHora se toma el valor completo especificado como UTC
/// </summary>
public class ValidadorFecha
{
    /// <summary>
    /// Valor mínimo aceptable, si es nulo no se valúa
    /// </summary>
    public DateTime? Minimo { get; set; }

    /// <summary>
    /// Valor máximo aceptable, si es nulo no se valúa
    /// </summary>
    public DateTime? Maximo { get; set; }
}