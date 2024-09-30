using MongoDB.Bson.Serialization.Attributes;
using System.Diagnostics.CodeAnalysis;

namespace controlescolar.modelo.rolesescolares;

/// <summary>
/// Define el catálogo de motivos de baja para el vínculo de un rol escolar
/// </summary>
[ExcludeFromCodeCoverage]
public class EntidadMovimientoRolPersonaEscuela
{
    /// <summary>
    /// Identificador único del movimiento, este valor se calcula tuomaticamente
    /// </summary>
    [BsonElement("i")]
    public long Id { get; set; }

    /// <summary>
    /// Identitificador del rol de la persona
    /// </summary>
    [BsonElement("r")]
    public long RolPersonaEscuelaId { get; set; }

    /// <summary>
    /// Texto asociado al movmiento para el rol
    /// </summary>
    [BsonElement("m")]
    public required string Nombre { get; set; }

    /// <summary>
    /// CLave de la operación, puedes er un dentificador externo como del movimiento en el sistema de nómina
    /// </summary>
    [BsonElement("c")]
    public string? Clave { get; set; }

    /// <summary>
    /// Marca el movimiento como eliminado para evitar su uso
    /// </summary>
    [BsonElement("x")]
    public bool Eliminado { get; set; } = false;

    /// <summary>
    /// Tipo de movimiento para el motivo
    /// </summary>
    [BsonElement("t")]
    public TipoMovimientoRol TipoMovimiento { get; set; } = TipoMovimientoRol.NoDefinido;

    /// <summary>
    /// Deterrmina lo que socede con el vínculo de la persona al establcer el movimiento
    /// </summary>
    [BsonElement("a")]
    public TipoActualizacionVinculo TipoActualizacion { get; set; }

}
