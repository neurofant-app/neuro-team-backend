using extensibilidad.metadatos;

namespace identidad.modelo;


/// <summary>
/// Datos de una persona en la apliación
/// </summary>
public class Persona : PersonaBase
{
    /// <summary>
    /// Identificador  único de la persona
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Identificadores de las plantillas a partir de la cuales se interpretan las propiedades extendidas
    /// </summary>
    public List<string>? PantillasId { get; set; }

    /// <summary>
    /// Lista de valores en el caso de que ecistan propieades extendidas
    /// </summary>
    public List<ValorPropiedad>? PropiedadesExtendidas { get; set; }

}
