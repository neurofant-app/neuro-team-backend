using System.Security.Claims;

namespace apigenerica.model.modelos;

/// <summary>
/// Almacena los identificadores relacionados con el contexto del usuario en sesión
/// </summary>
public class ContextoUsuario
{
    /// <summary>
    /// Identificador único del usuario
    /// </summary>
    public string? UsuarioId { get; set; }
    
    /// <summary>
    /// Dominio del usuario en sesión
    /// </summary>
    public string? DominioId { get; set; }
    
    /// <summary>
    /// Unidad organizacional del usuario en sesión
    /// </summary>
    public string? UOrgId { get; set; }


    /// <summary>
    /// Idioma del request desde Accept-Language
    /// </summary>
    public string? Idioma { get; set; }
    
    /// <summary>
    /// Token de autenticación para usuarios autenticados
    /// </summary>
    public string? TokenAutenticacion { get; set; }

    /// <summary>
    /// Claims de seguridad asociados al usuario
    /// </summary>
    public List<Claim>?  Clains { get; set; }


    /// <summary>
    /// Define los permisos del usuario en la aplicacion
    /// </summary>
    public List<string>? PermisosAplicacion { get; set;  }

    /// <summary>
    /// Define los roles del usuario en la aplicacion
    /// </summary>
    public List<string>? RolesAplicacion { get; set; }
}
