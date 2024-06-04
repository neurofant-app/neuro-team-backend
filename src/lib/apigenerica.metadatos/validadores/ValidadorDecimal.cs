using System.ComponentModel.DataAnnotations;

namespace extensibilidad.metadatos.validadores;

/// <summary>
/// Configuración apr ala validación de una propiedad decimal
/// </summary>
public class ValidadorDecimal : IValidatableObject
{
    /// <summary>
    /// Valor mínmo aceptable, si es nulo no se valúa
    /// </summary>
    public decimal? Minimo { get; set; }

    /// <summary>
    /// Valor máximo aceptable, si es nulo no se valúa
    /// </summary>
    public decimal? Maximo { get; set; }


    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        // Implementar de manera similar al validador de fecha, es este caso no hay tipo
        // pero el valor de comparación vendrá en el diccioanrio con la clave 'valor'

        throw new NotImplementedException();
    }
}
