using controlescolar.modelo.documentacion;
using controlescolar.modelo.plantel;
using controlescolar.modelo.rolesescolares;
using MongoDB.Bson.Serialization.Attributes;
using System.Diagnostics.CodeAnalysis;

namespace controlescolar.modelo.escuela;

/// <summary>
/// Define la entidad a almacenar en el repositorio de datos para la Escuela
/// Una escuela es un agrupador lógico de recursos para el control escolar
/// </summary>
[ExcludeFromCodeCoverage]
public class EntidadEscuela
{
    // <summary>
    /// Identificador único de la Escuela
    /// </summary>
    [BsonId]
    public Guid Id { get; set; }

    /// <summary>
    /// Nombre de la escuela
    /// </summary>
    [BsonElement("n")]
    public required string Nombre { get; set; }

    /// <summary>
    /// Determina si la escuela se encuentra activa o inactiva
    /// </summary>
    [BsonElement("a")]
    public required bool Activa { get; set; } = true;

    /// <summary>
    /// Fecha de creación del registro
    /// </summary>
    [BsonElement("f")]
    public required DateTime Creacion { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Identificador único de la cuenta que posee la escuela regularmente es el Id del usuario en sesión
    /// </summary>
    [BsonElement("t")]
    public required string CuentaId  { get; set; }
    /// Debe crearse un índice en la base de datos para facilitar las búsquedas

    /// <summary>
    /// Clave de la escuela para uso local por ejemplo del sistema escolar nacional
    /// </summary>
    [BsonElement("c")]
    public string? Clave { get; set; }
    /// Debe crearse un índice en la base de datos para facilitar las búsquedas


    /// <summary>
    /// Lista de planteles asociados a la escuela
    /// </summary>
    [BsonElement("rpe")]
    public List<EntidadPlantel> Planteles { get; set; } = [];

    /// <summary>
    /// Roles de una persona disponibles en la escuela durante la creacion deben asignarse los por defecto utilizando Defaults.RolesPersonaEscuelaBase. 
    /// La lista puede extenderse a;adiendo nuevo elementos locales
    /// </summary>
    [BsonElement("rpe")]
    public List<EntidadRolPersonaEscuela> RolesPersona { get; set; } = [];

    /// <summary>
    /// Expedientes aplicables a las personas miembros de la escuela y sus planteles
    /// </summary>
    [BsonElement("exp")]
    public List<EntidadExpediente> Expedientes { get; set; } = [];

    /// <summary>
    /// Parametrode de configuracion de la escuela
    /// </summary>
    [BsonElement("par")]
    public ConfiguracionEscuela? Configuracion { get; set; }

}
