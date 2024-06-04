using System.ComponentModel.DataAnnotations;

namespace extensibilidad.metadatos.validadores;

/// <summary>
/// Configuración apr ala validación de una propiedad basada en fecha
/// </summary>
public class ValidadorFecha : IValidatableObject
{
    /// <summary>
    /// Valor mínimo aceptable, si es nulo no se valúa
    /// </summary>
    public DateTime? Minimo { get; set; }

    /// <summary>
    /// Valor máximo aceptable, si es nulo no se valúa
    /// </summary>
    public DateTime? Maximo { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        List<ValidationResult> result = [];
        DateTime? valor = null;
        TipoDatos paramTipo = TipoDatos.SinAsignar;

        if (!validationContext.Items.ContainsKey("valor"))
        {
            result.Add(new ValidationResult("No existe el valor para comparar"));
        }
        else
        {
            if ((validationContext.Items["valor"] is DateTime fecha))
            {
                valor = fecha;
            }
            else
            {
                result.Add(new ValidationResult("El valor  no es del tipo fecha/hora"));
            }
        }

        if (!validationContext.Items.ContainsKey("tipo"))
        {
            result.Add(new ValidationResult("No existe el tipo de dato para comparar"));
        }
        else
        {
            if ((validationContext.Items["tipo"] is TipoDatos tipo))
            {
                paramTipo = tipo;
            }
            else
            {
                result.Add(new ValidationResult("El tipo de datos no es válido"));
            }
        }

        if (result.Count == 0)
        {
            switch (paramTipo)
            {
                case TipoDatos.Hora:
                    ComparaHora(valor!.Value, result);
                    break;

                case TipoDatos.Fecha:
                    ComparaFecha(valor!.Value, result);
                    break;

                case TipoDatos.FechaHora:
                    ComparaFechaHora(valor!.Value, result);
                    break;

                default:
                    result.Add(new ValidationResult("Tipo incorrecto"));
                    break;
            }
        }

        return result;
    }

    private void ComparaFecha(DateTime valor, List<ValidationResult> results)
    {
        DateTime fechaTemp = new(valor.Year, valor.Month, valor.Day, 0, 0, 0);
        // Verificar que fechaTemp se encuentre en el rango dependiendo del caso
        if (Minimo.HasValue && Maximo.HasValue)
        {

        }
        else
        {

            if (Minimo.HasValue)
            {

            }

            if (Maximo.HasValue)
            {

            }

        }
    }

    private void ComparaHora(DateTime valor, List<ValidationResult> results)
    {
        DateTime fechaTemp = new(1900, 1, 1, valor.Hour, valor.Minute, valor.Second);
        // Verificar que fechaTemp se encuentre en el rango dependiendo del caso
        if (Minimo.HasValue && Maximo.HasValue)
        {

        }
        else
        {

            if (Minimo.HasValue)
            {

            }

            if (Maximo.HasValue)
            {

            }

        }
    }

    private void ComparaFechaHora(DateTime valor, List<ValidationResult> results)
    {
        DateTime fechaTemp = valor;
        // Verificar que fechaTemp se encuentre en el rango dependiendo del caso
        if (Minimo.HasValue && Maximo.HasValue)
        {

        }
        else
        {

            if (Minimo.HasValue)
            {

            }

            if (Maximo.HasValue)
            {

            }

        }
    }
}