namespace contabee.identity.api.models;

/// <summary>
/// DTO de actualización de contraseña
/// </summary>
public class ActualizarContrasena
{
    public required string Email { get; set; }
    public required string Password { get; set; }
    public required string Token { get; set; }
}
