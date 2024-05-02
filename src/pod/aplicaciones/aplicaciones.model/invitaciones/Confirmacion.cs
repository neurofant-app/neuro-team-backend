namespace aplicaciones.model.invitaciones;

/// <summary>
/// REaliza la confirmación de subscripción para un usuario que recibió una invitación
/// </summary>
public class Confirmacion
{
    /// <summary>
    /// Identificador de la invitación
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Contraseña de acceso a la cuenta
    /// </summary>
    public string Password { get; set; }
}
