using MongoDB.Bson.Serialization.Attributes;
using System.Diagnostics.CodeAnalysis;

namespace controlescolar.modelo.persona;

/// <summary>
/// Especifica como una persona se vincula con los recursos de una escuela
/// </summary>
[ExcludeFromCodeCoverage]
public class VinculoPersonaEscuela
{
    /// <summary>
    /// Identificador único del vínculo
    /// </summary>
    [BsonElement("i")]
    public long Id { get; set; }
    // Para el desarrollo se tomará = DateTime.UtcNow.Ticks

    /// <summary>
    /// Cuando el vínculo sea realizado con un plantel esta propiedad indica cual de ellos
    /// </summary>
    [BsonElement("p")]
    public Guid? PlantelId { get; set; }
    // Indexar para busqiedas por plantel

    /// <summary>
    /// Rol de  la persona en la escuela a partir de la lista de roles disponibles
    /// </summary>
    [BsonElement("r")]
    public required long RolPersonaEscuelaId { get; set; }

    /// <summary>
    /// Motivo del movimiento asociado al vínculo, por ejemplo inscripción, contratación, baja definitiva, permiso
    /// </summary>
    [BsonElement("mm")]
    public required long MovimientoId { get; set; }

    /// <summary>
    /// Fecha de creación del vínculo
    /// </summary>
    [BsonElement("f")]
    public DateTime Fecha { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Fecha inicial del vínculo
    /// </summary>
    [BsonElement("fi")]
    public DateTime FechaInicial { get; set; }

    /// <summary>
    /// Fecha de finalizaión del vínculo
    /// </summary>
    [BsonElement("ff")]
    public DateTime? FechaFinal { get; set; }

    /// <summary>
    /// Especifica si el vínculo se encuentra activo
    /// </summary>
    [BsonElement("a")]
    public bool Activo { get; set; } = true;
    // Indexar para busquedas por plantel/activo

    /// <summary>
    /// Determina si el vínculo ha sido finalizado, por ejemplo para una relación laboral
    /// </summary>
    [BsonElement("x")]
    public bool Finalizado { get; set; } = false;
    // Indexar para busquedas por plantel/finalizado
}
