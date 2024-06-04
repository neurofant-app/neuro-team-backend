using extensibilidad.metadatos.configuraciones;
using extensibilidad.metadatos.validadores;

namespace extensibilidad.metadatos;

public class Propiedad: PropiedadBase
{
    /// <summary>
    /// Identificador único de la propiedad, para reflexi{on es el nombre completo del objeto + nombre de la propiedad
    /// </summary>
    public string Id { get; set; }

    /// <summary>
    /// Determina si es posible realizar bpsuquedas sobre la propiedad
    /// </summary>
    public bool Buscable { get; set; }

    /// <summary>
    /// Determina si la propieda es visible
    /// </summary>
    public bool Visible { get; set; }

    /// <summary>
    /// Determina si el campo se muestra al crear
    /// </summary>
    public bool HabilitadoCrear { get; set; }

    /// <summary>
    /// Determina si el campo se muestra al editar
    /// </summary>
    public bool HabilitadoEditar { get; set; }

    /// <summary>
    /// Determina si el campo se muestra al desplegar
    /// </summary>
    public bool HabilitadoDespliegue { get; set; }

    /// <summary>
    /// Configuración de la propiedad en la vista tabular
    /// </summary>
    public ConfiguracionTabular? ConfiguracionTabular { get; set; }

    /// <summary>
    /// Define las propiedades de la configuración de la propiedad en un formulario
    /// </summary>
    public ConfiguracionFormulario? ConfiguracionFormulario { get; set; }

    /// <summary>
    /// Indica las operaciones donde la propuedad es requerida
    /// </summary>
    public List<RequeridaOperacion> Requerida { get; set; } = [];

    /// <summary>
    /// Validador opcional para propiedades texto
    /// </summary>
    public ValidadorTexto? ValidadorTexto { get; set; }

    /// <summary>
    /// Validador opcional para propiedades numericas enteras
    /// </summary>
    public ValidadorEntero? ValidadorEntero { get; set; }

    /// <summary>
    /// Validador opcional para propiedades numericas decimales
    /// </summary>
    public ValidadorDecimal? ValidadorDecimal { get; set; }

    /// <summary>
    /// Validador opcional para propiedades fecha, fechahora y hora
    /// </summary>
    public ValidadorFecha? ValidadorFecha { get; set; }
}


