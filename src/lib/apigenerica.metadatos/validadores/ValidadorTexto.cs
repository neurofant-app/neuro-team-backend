using System.ComponentModel.DataAnnotations;

namespace extensibilidad.metadatos.validadores;

/// <summary>
/// PAraámetros para la validación de un apropiedad de texto
/// </summary>
public class ValidadorTexto : IValidatableObject
{
    /// <summary>
    /// Longitud mínima del texto de la propiedad, si es nulo no se evalúa
    /// </summary>
    public virtual int? LongitudMinima { get; set; }

    /// <summary>
    /// Longitud máxima del texto de la propiedad, si es nulo no se evalúa
    /// </summary>
    public virtual int? LongitudMaxima { get; set; }

    /// <summary>
    /// Expresión regular para validar, si es nulo no se evalúa
    /// </summary>
    public virtual string? RegExp { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        // Implementar de manera similar al validador de fecha, es este caso no hay tipo
        // pero el valor de comparación vendrá en el diccioanrio con la clave 'valor'
        // En el caso de que la propiedad RegExp tenga un valor deberá probarseq eu el valor se ajuste a las regexp
        throw new NotImplementedException();
    }
}