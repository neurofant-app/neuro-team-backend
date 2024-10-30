using comunes.primitivas.atributos;
using MongoDB.Bson.Serialization.Attributes;
using System.Diagnostics.CodeAnalysis;

namespace controlescolar.modelo.rolesescolares;

/// <summary>
/// Rol de vinculación de personas a escuelas
/// </summary>
[ExcludeFromCodeCoverage]
[EntidadDB]
public class EntidadRolPersonaEscuela

{
    /// <summary>
    /// Identificador único del rol, este valor se calcula automaticamente al crear el rol
    /// </summary>
    [BsonElement("i")]
    public long Id { get; set; }

    /// <summary>
    /// Nombre único del rol
    /// </summary>
    [BsonElement("n")]
    public required string Nombre { get; set; }

    /// <summary>
    /// CLave del rol, puedes er un dentificador externo como del tipo de empleoado en el sistema de nómina
    /// </summary>
    [BsonElement("c")]
    public string? Clave { get; set; }

    /// <summary>
    /// Descripción del rol
    /// </summary>
    [BsonElement("d")]
    public string? Descripcion { get; set; }

    /// <summary>
    /// Marca el rol como eliminado para evitar su uso
    /// </summary>
    [BsonElement("x")]
    public bool Eliminado { get; set; } = false;

    /// <summary>
    /// Motivos de para los movimientos asociados al rol, esta lista debe prellenarse para cada escuela con los defaults para el tipo
    /// </summary>
    [BsonElement("m")]
    public List<EntidadMovimientoRolPersonaEscuela> Movimientos { get; set; } = [];
}
