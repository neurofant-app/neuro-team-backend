namespace controlescolar.modelo;

/// <summary>
/// Representa los datos mínimos de un ser humano que interactura con el sistema o forma parte de la información
/// </summary>
public abstract class PersonaBase
{
    /// <summary>
    /// Nombre o nombres de la persona
    /// </summary>
    public string? Nombre { get; set; }

    /// <summary>
    /// Primer apellido de la persona
    /// </summary>
    public string? Apellido1 { get; set; }

    /// <summary>
    /// Segundo apellido de la persona
    /// </summary>
    public string? Apellido2 { get; set; }

    /// <summary>
    /// Fecha de nacimiento de la perosona
    /// </summary>
    public DateTime? FechaNacimiento { get; set; }

    /// <summary>
    /// Género de la persona
    /// </summary>
    public GeneroPersona Genero { get; set; } = GeneroPersona.NoDefinido;
   
}
