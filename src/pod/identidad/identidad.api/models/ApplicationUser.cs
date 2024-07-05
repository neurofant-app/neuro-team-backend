using AspNetCore.Identity.MongoDbCore.Models;
using Microsoft.AspNetCore.Identity;

namespace contabee.identity.api.models;

public enum EstadoCuenta { 
    PendienteConfirmacion = 0,
    Activo = 1,
    BajaCliente = 2,
    InactivoPago = 3,
}


// Add profile data for application users by adding properties to the ApplicationUser class
public class ApplicationUser : IdentityUser
{

    /// <summary>
    /// Estado de la cuenta
    /// </summary>
    public EstadoCuenta Estado { get; set; } = EstadoCuenta.PendienteConfirmacion;
    
    /// <summary>
    /// Fecha en que el usuario realizó la solicitud de inscripción
    /// </summary>
    public DateTime FechaRegistro { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Fecha de confirmación de la solicitud de inscripcion
    /// </summary>
    public DateTime? FechaActivacion { get; set; }
}
