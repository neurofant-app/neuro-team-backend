using System.Diagnostics.CodeAnalysis;

namespace extensibilidad.metadatos.configuraciones;
/// <summary>
/// DEfine la configuración para el despliegue en un formilario
/// </summary>
[ExcludeFromCodeCoverage]
public class ConfiguracionFormulario
{
    /// <summary>
    /// Define si dos  o más elemento deben incluire en el mismo renglon
    /// </summary>
    public int Renglon { get; set; }

    /// <summary>
    /// Determina la posición relativa a otras propiedades
    /// </summary>
    public virtual TipoDespliegue TipoDespliegue { get; set; }

    /// <summary>
    /// Posicion relativa de la propiedad en el formulario en relación con otras propiedades
    /// </summary>
    public virtual int Indice { get; set; }

    /// <summary>
    /// Determina si la propiedad es visible o debe mantenerse oculta
    /// </summary>
    public virtual bool Visible { get; set; }

    /// <summary>
    /// Define el aencho relativo de la propiedad en porcentaje del área de despliegue
    /// y relativas a las otras propiedades a desplegar
    /// </summary>
    public virtual int Ancho { get; set; }
}
