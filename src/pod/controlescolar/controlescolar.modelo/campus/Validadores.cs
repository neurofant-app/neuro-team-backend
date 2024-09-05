
namespace controlescolar.modelo.campi;

using FluentValidation;

/// <summary>
/// Validador para la creación de campus
/// </summary>
public class CreaCampusValidator : AbstractValidator<CreaCampus>
{
    public CreaCampusValidator()
    {
        RuleFor(x  => x.Nombre).NotNull().MaximumLength(500).MinimumLength(1);
    }
}


/// <summary>
/// Validador para la actualización de campus
/// </summary>
public class ActualizaCampusValidator : AbstractValidator<CreaCampus>
{
    public ActualizaCampusValidator()
    {
        RuleFor(x => x.Nombre).NotNull().MaximumLength(500).MinimumLength(1);
    }
}