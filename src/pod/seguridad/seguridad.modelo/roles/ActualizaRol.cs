using MongoDB.Bson.Serialization.Attributes;
using seguridad.modelo.instancias;
using seguridad.modelo.relaciones;
using System.Diagnostics.CodeAnalysis;

namespace seguridad.modelo.roles;

[ExcludeFromCodeCoverage]
public class ActualizaRol
{
    /// <summary>
    /// Identificador único del rol, se utiliza como clave para los roles y para la i18N, debe ser único en la lista de permisos de una app
    /// </summary>
    public required string RolId { get; set; }

    /// <summary>
    /// Nombre del rol para la UI, esto será calcolado en base al idioma o bien al crear roles personalizados
    /// </summary>
    public required string Nombre { get; set; }

    /// <summary>
    /// Descripción del rol para la UI, esto será calcolado en base al idioma o bien al crear roles personalizados
    /// </summary>
    public string? Descripcion { get; set; }

    [BsonIgnore]
    public string ModuloId { get; set; }


    [BsonIgnore]
    public string? InstanciaAplicacionId { get; set; }

    [BsonIgnore]
    public List<RolGrupo> RolGrupo { get; set; }

    [BsonIgnore]
    public List<RolUsuario> RolUsuario { get; set; }
}
