using MongoDB.Bson.Serialization.Attributes;
using System.Diagnostics.CodeAnalysis;

namespace controlescolar.modelo.plantel;

/// <summary>
/// Define la entidad a almacenar en el repositorio de datos para la Escuela
/// Una escuela es un agrupador lógico de recursos para el control escolar
/// </summary>
[ExcludeFromCodeCoverage]
public class EntidadPlantel
{
    // <summary>
    /// Identificador único del plantel
    /// </summary>
    [BsonId]
    public Guid Id { get; set; }

    /// <summary>
    /// Nombre del plantel
    /// </summary>
    [BsonElement("n")]
    public required string Nombre { get; set; }

    /// <summary>
    /// Determina si el plantel se encuentra activo o inactivo
    /// </summary>
    [BsonElement("a")]
    public required bool Activo { get; set; } = true;

    /// <summary>
    /// Fecha de creación del registro
    /// </summary>
    [BsonElement("f")]
    public required DateTime Creacion { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Identificador único la escuela a la que pertenece el plantel
    /// </summary>
    [BsonElement("t")]
    public Guid EscuelaId  { get; set; }
    /// Debe crearse un índice en la base de datos para facilitar las búsquedas

    /// <summary>
    /// Clave del plantek para uso local por ejemplo del sistema escolar nacional
    /// </summary>
    [BsonElement("c")]
    public string? Clave { get; set; }

    /// <summary>
    /// El plantel es un plantel virtual
    /// </summary>
    [BsonElement("v")]
    public bool EsVirtual { get; set; }
}
