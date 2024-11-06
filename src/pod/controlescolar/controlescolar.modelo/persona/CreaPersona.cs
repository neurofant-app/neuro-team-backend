using System.Diagnostics.CodeAnalysis;

namespace controlescolar.modelo.persona;

/// <summary>
/// DTO para la creación de una persona en la API
/// </summary>
[ExcludeFromCodeCoverage]
public class CreaPersona
{

    /// <summary>
    /// Identificador único la escuela a la que pertenece el resistro de la persona
    /// </summary>
    public required Guid EscuelaId { get; set; }

    /// <summary>
    /// Nombre de la persona
    /// </summary>
    public required string Nombre { get; set; }

    /// <summary>
    /// Primer apellido de la persona
    /// </summary>
    public required string Apellido1 { get; set; }

    /// <summary>
    /// Segundo apellido de la persona
    /// </summary>
    public string? Apellido2 { get; set; }

    /// <summary>
    /// Fecha de nacimiento de la persona, la parte de hora es ignorada y debe llenarse con 00:00:00
    /// </summary>
    public DateTime? Nacimiento { get; set; }

    /// <summary>
    /// Genero asociado a la persona
    /// </summary>
    public GeneroPersona? Genero { get; set; }

    /// <summary>
    /// Clave de única de identidad de la persona a nivel nacional, pro ejemplo la CURP en México
    /// </summary>
    public string? IdNacional { get; set; }

}
