using comunes.primitivas.atributos;

namespace creador.contenido.model;
/// <summary>
/// Datos para la creación de un EspacioTrabajo
/// </summary>
[CQRSCrear]
public class CreaEspacioTrabajo
{
    /// <summary>
    /// Nombre del espacio de trabajo.
    /// </summary>
    public string Nombre { get; set; }
    // Requerida // [I] [A] [D]
}
