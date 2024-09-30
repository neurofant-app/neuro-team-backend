using System.Diagnostics.CodeAnalysis;

namespace controlescolar.modelo.rolesescolares;

/// <summary>
/// DTO para la consulta de roles escolares
/// </summary>
[ExcludeFromCodeCoverage]
public class ConsultaRolPersonaEscuela
{
    /// <summary>
    /// Identificador único del rol, este valor se calcula automaticamtne al crear el rol
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// Nombre único del rol
    /// </summary>
    public required string Nombre { get; set; }

    /// <summary>
    /// CLave del rol, puedes er un dentificador externo como del tipo de empleoado en el sistema de nómina
    /// </summary>
    public string? Clave { get; set; }

    /// <summary>
    /// Descripción del rol
    /// </summary>
    public string? Descripcion { get; set; }

    /// <summary>
    /// MOvimientos permitos en un rol escolar
    /// </summary>
    public List<EntidadMovimientoRolPersonaEscuela> Movimientos { get; set; } = [];
}
