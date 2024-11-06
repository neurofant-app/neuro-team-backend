using System.Diagnostics.CodeAnalysis;

namespace controlescolar.modelo.rolesescolares;

/// <summary>
/// Define el catálogo de motivos de baja para el vínculo de un rol escolar
/// </summary>
[ExcludeFromCodeCoverage]
public class ConsultaMovimientoRolPersonaEscuela
{
    /// <summary>
    /// Identificador único del movimiento, este valor se calcula tuomaticamente
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// Identitificador del rol de la persona
    /// </summary>
    public long RolPersonaEscuelaId { get; set; }

    /// <summary>
    /// Texto asociado al movmiento para el rol
    /// </summary>
    public required string Nombre { get; set; }

    /// <summary>
    /// CLave de la operación, puedes er un dentificador externo como del movimiento en el sistema de nómina
    /// </summary>
    public string? Clave { get; set; }

    /// <summary>
    /// Marca el movimiento como eliminado para evitar su uso
    /// </summary>
    public bool Eliminado { get; set; } = false;

    /// <summary>
    /// Tipo de movimiento para el motivo
    /// </summary>
    public TipoMovimientoRol TipoMovimiento { get; set; } = TipoMovimientoRol.NoDefinido;

    /// <summary>
    /// Deterrmina lo que socede con el vínculo de la persona al establcer el movimiento
    /// </summary>
    public TipoActualizacionVinculo TipoActualizacion { get; set; }

}
