using System.Diagnostics.CodeAnalysis;

namespace seguridad.modelo.roles;

[ExcludeFromCodeCoverage]
public class ConsultaGrupoUsuarios
{

    /// <summary>
    /// Lista de los ID de usuarios asociados al grupo
    /// </summary>
    public List<string> UsuarioIds { get; set; } = [];

}
