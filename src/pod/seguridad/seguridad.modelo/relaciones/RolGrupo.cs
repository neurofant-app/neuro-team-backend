using seguridad.modelo.instancias;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace seguridad.modelo.relaciones;

public class RolGrupo
{
    public string Id { get; set; }
    public required string RolId { get; set; }
    public required Guid GrupoId { get; set; }
    [NotMapped]
    [JsonIgnore]
    public InstanciaAplicacionMysql InstanciaAplicacion { get; set; }
    [NotMapped]
    [JsonIgnore]
    public GrupoUsuarios Grupo { get; set; }
    [NotMapped]
    [JsonIgnore]
    public Rol Rol { get; set; }
}
