
using seguridad.modelo.instancias;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace seguridad.modelo.relaciones;

public class RolUsuario
{
    public  string Id { get; set; }
    public required string RolId { get; set; }
    public required string UsuarioId { get; set; }
    
    [NotMapped]
    [JsonIgnore]
    public InstanciaAplicacion InstanciaAplicacion { get; set; }

    [NotMapped]
    [JsonIgnore]
    public Rol Rol { get; set; }
}
