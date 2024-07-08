
using seguridad.modelo.instancias;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace seguridad.modelo.relaciones;

public class PermisoUsuario
{
    public string Id { get; set; }
    public required string PermisoId { get; set; }
    public required string UsuarioId { get; set; }
    
    [NotMapped]
    [JsonIgnore]
    public InstanciaAplicacion InstanciaAplicacion { get; set; }
    
    [NotMapped]
    [JsonIgnore]
    public Permiso Permiso { get; set; }
}
