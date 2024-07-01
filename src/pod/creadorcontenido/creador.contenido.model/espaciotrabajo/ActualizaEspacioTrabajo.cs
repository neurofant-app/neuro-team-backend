using comunes.primitivas.atributos;

namespace creador.contenido.model;
/// <summary>
/// Define el DTO de actualización un EspacioTrabajo
/// </summary>
[CQRSActualizar]
public class ActualizaEspacioTrabajo
{
    /// <summary>
    /// Nombre del espacio de trabajo.
    /// </summary>
    public string Nombre { get; set; }
    // Requerida // [I] [A] [D]

    /// <summary>
    /// Enumeración del estado de la entidad.
    /// </summary>
    public EstadoEntidad Estado { get; set; }
    // Requerida // [A] [D]
}
