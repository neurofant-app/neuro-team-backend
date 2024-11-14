using System.Diagnostics.CodeAnalysis;

namespace usuario.model;

/// <summary>
/// Datos del perfil de un usuario registrado
/// </summary>
[ExcludeFromCodeCoverage]
public class Perfil
{
    /// <summary>
    /// Identificador único del perfil
    /// </summary>
    public string? Id { get; set; }

    /// <summary>
    /// Datos del nombre para el perfil del usuario
    /// </summary>
    public Nombre? Nombre { get; set; }
    
    /// <summary>
    /// Correo electrónico asociado al perfil
    /// </summary>
    public string? Email { get; set; }

    /// <summary>
    /// Imagen de avatar codificada en base64
    /// </summary>
    public string? AvatarB64 { get; set; }

    /// <summary>
    /// Especifica si el perfil se encuentra activo
    /// </summary>
    public bool Activo { get; set; }

    /// <summary>
    /// Dominiso a los que pertenece el usuarios
    /// </summary>
    public List<DominioUsuario> Dominios { get; set; } = [];

}
