namespace aplicaciones.model;

public class CreaAplicacion
{
    /// <summary>
    /// Nombre de la aplicación que emite la invitación
    /// </summary>
    public required string Nombre { get; set; }
    // Requerida 200
    // [I] [A] [D]

    /// <summary>
    /// Especifica si la aplicación se encuentra activa, solo es posible emitir notificaciones so lo está
    /// </summary>
    public bool Activa { get; set; }
    // Requerida
    // [I] [A] [D]
}
