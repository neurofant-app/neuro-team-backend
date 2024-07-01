using comunes.primitivas.atributos;

namespace creador.contenido.model;

/// <summary>
/// DTO que muestra los metadatos recibidos de la base de datos.
/// </summary>
[CQRSConsulta]
public class ConsultaEspacioTrabajo
{
    /// <summary>
    /// Id único de la entidad.
    /// </summary>
    public Guid Id { get; set; }
    // Requerida // [A] [D]

    /// <summary>
    /// Nombre del espacio de trabajo.
    /// </summary>
    public string Nombre { get; set; }
    // Requerida // [I] [A] [D]

    /// <summary>
    /// Fecha de creación.
    /// </summary>
    public DateTime FechaCreacion { get; set; }
    // Requerida // [D]

    /// <summary>
    /// Enumeración del estado de la entidad.
    /// </summary>
    public EstadoEntidad Estado { get; set; }
    // Requerida // [A] [D]

    /// <summary>
    /// Fecha en que marcó para eliminación.
    /// </summary>
    public DateTime? FechaEliminacion { get; set; }
    // [D]
}
