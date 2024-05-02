namespace extensibilidad.metadatos.configuraciones;

/// <summary>
/// Especifica las la configuración tabulre de una propiedad de metadatos
/// </summary>
public class ConfiguracionTabular
{
    /// <summary>
    /// Posición o número de columna en el despliegue tabular si MostrarEnTabla es true 
    /// </summary>
    public int Indice { get; set; }

    /// <summary>
    /// Determina si la propiedad se muestra en el despliegue tabular
    /// </summary>
    public bool Visible { get; set; }

    /// <summary>
    /// Determina si la propiedad puede alternar su visibilidad en el despliegue tabular
    /// </summary>
    public bool Alternable { get; set; }
    /// <summary>
    /// Determina si la propiedad puede ordenar su visibilidad en el despliegue tabular
    /// </summary>
    public bool Ordenable { get; set; }
    /// <summary>
    /// Define el aencho relativo de la propiedad un unidades de división de la UI
    /// y relativas a las otras propiedades a desplegar
    /// </summary>
    public int Ancho { get; set; }
}

