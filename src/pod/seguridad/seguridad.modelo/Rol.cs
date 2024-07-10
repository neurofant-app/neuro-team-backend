using MongoDB.Bson.Serialization.Attributes;
using seguridad.modelo.instancias;
using seguridad.modelo.relaciones;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace seguridad.modelo;

/// <summary>
/// Rol de seguridad asociado a una apliación
/// </summary>
public class Rol
{
    /// <summary>
    /// Identificador único del rol, se utiliza como clave para los roles y para la i18N, debe ser único en la lista de permisos de una app
    /// </summary>
    [BsonElement("rid")]
    public required string RolId { get; set; }

    /// <summary>
    /// Lista de los identificadores de permisos asociados al rol
    /// </summary>
    [BsonElement("rp")]
    public List<string> Permisos { get; set; } = [];

    /// <summary>
    /// DEfine si un rol ha sido creado por el administrador de sistema
    /// </summary>
    [BsonElement("rpe")]
    public bool Personalizado { get; set; }

    /// <summary>
    /// Nombre del rol para la UI, esto será calcolado en base al idioma o bien al crear roles personalizados
    /// </summary>
    [BsonElement("rn")]
    public string? Nombre { get; set; }

    /// <summary>
    /// Descripción del rol para la UI, esto será calcolado en base al idioma o bien al crear roles personalizados
    /// </summary>
    [BsonElement("rd")]
    public string? Descripcion { get; set; }


    [BsonIgnore]
    public List<Permiso> RolPermisos { get; set; }

    [BsonIgnore]
    public string ModuloId { get; set; }

    [BsonIgnore]
    [JsonIgnore]
    public Modulo Modulo { get; set; }

    [BsonIgnore]
    public string? InstanciaAplicacionId { get; set; }

    [BsonIgnore]
    [JsonIgnore]
    public InstanciaAplicacion InstanciaAplicacion { get; set; }

    [BsonIgnore]
    public List<RolGrupo> RolGrupo { get; set; }

    [BsonIgnore]
    public List<RolUsuario> RolUsuario { get; set; }
}
