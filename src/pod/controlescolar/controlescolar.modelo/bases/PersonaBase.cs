using controlescolar.modelo.persona;
using MongoDB.Bson.Serialization.Attributes;

namespace controlescolar.modelo;

/// <summary>
/// Representa los datos mínimos de un ser humano que interactura con el sistema o forma parte de la información
/// </summary>
public abstract class PersonaBase
{
    /// <summary>
    /// Nombre o nombres de la persona
    /// </summary>
    [BsonElement("n")]
    public string? Nombre { get; set; }

    /// <summary>
    /// Primer apellido de la persona
    /// </summary>
    [BsonElement("a1")]
    public string? Apellido1 { get; set; }

    /// <summary>
    /// Segundo apellido de la persona
    /// </summary>
    [BsonElement("a2")]
    public string? Apellido2 { get; set; }

    /// <summary>
    /// Fecha de nacimiento de la perosona
    /// </summary>
    [BsonElement("fn")]
    public DateTime? FechaNacimiento { get; set; }

    /// <summary>
    /// Identificador único nacional de la persona por ejemple CURP o número de DNI
    /// </summary>
    [BsonElement("idn")]
    public string? IdNacional { get; set; }

    /// <summary>
    /// Género de la persona
    /// </summary>
    [BsonElement("g")]
    public GeneroPersona Genero { get; set; } = GeneroPersona.NoDefinido;
   
}
