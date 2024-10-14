using controlescolar.modelo.documentacion;
using controlescolar.modelo.plantel;
using controlescolar.modelo.rolesescolares;
using System.Diagnostics.CodeAnalysis;

namespace controlescolar.modelo.escuela;

/// <summary>
/// DTO de cunsulta para la entidad escuela
/// </summary>
[ExcludeFromCodeCoverage]
public class ConsultaEscuela 
{
    // <summary>
    /// Identificador único de la Escuela
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Nombre de la escuela
    /// </summary>
    public required string Nombre { get; set; }

    /// <summary>
    /// Determina si la escuela se encuentra activa o inactiva
    /// </summary>
    public required bool Activa { get; set; } = true;

    /// <summary>
    /// Fecha de creación del registro
    /// </summary>
    public required DateTime Creacion { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Clave de la escuela para uso local por ejemplo del sistema escolar nacional
    /// </summary>
    public string? Clave { get; set; }

    /// <summary>
    /// Roles de una persona disponibles en la escuela durante la creacion deben asignarse los por defecto utilizando Defaults.RolesPersonaEscuelaBase. 
    /// La lista puede extenderse a;adiendo nuevo elementos locales
    /// </summary>
    public List<EntidadRolPersonaEscuela> RolesPersona { get; set; } = [];

    /// <summary>
    /// Lista de planteles asociados a la escuela
    /// </summary>
    public List<EntidadPlantel> Planteles { get; set; } = [];

    /// <summary>
    /// Expedientes aplicables a las personas miembros de la escuela y sus planteles
    /// </summary>
    public List<EntidadExpediente> Expedientes { get; set; } = [];

    /// <summary>
    /// Parametrode de configuracion de la escuela
    /// </summary>
    public ConfiguracionEscuela? Configuracion { get; set; }
}
