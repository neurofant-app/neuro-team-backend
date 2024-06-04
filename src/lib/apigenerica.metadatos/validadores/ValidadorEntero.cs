using System.ComponentModel.DataAnnotations;

namespace extensibilidad.metadatos.validadores;


/// <summary>
/// Configuración apr ala validación de una propiedad entero
/// </summary>
public class ValidadorEntero : IValidatableObject
{
    /// <summary>
    /// Valor mínmo aceptable, si es nulo no se valúa
    /// </summary>

    public int? Minimo { get; set; }

    /// <summary>
    /// Valor máximo aceptable, si es nulo no se valúa
    /// </summary>
    public int? Maximo { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        // Implementar de manera similar al validador de fecha, es este caso no hay tipo
        // pero el valor de comparación vendrá en el diccioanrio con la clave 'valor'

        throw new NotImplementedException();
    }
}