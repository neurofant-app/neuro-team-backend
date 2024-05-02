namespace apigenerica.primitivas;

/// <summary>
/// Proporiciona los servicios para la configuración de entidades de API
/// </summary>
public class ConfiguracionAPIEntidades : IConfiguracionAPIEntidades
{

    private List<ServicioEntidadAPI>? serviciosEntidad = null;
    private List<ServicioEntidadAPI>? serviciosCatalogoEntidad = null;
    private List<string>? rutasGenericas = null;


    public List<string> ObtieneRutasControladorGenerico()
    {
        if (rutasGenericas == null)
        {
            rutasGenericas = IntrospeccionEnsamblados.OntieneRutasControladorGenrico();
        }
        return rutasGenericas;
    }

    /// <summary>
    /// Devuelve una lista de los servicios que implementan la interfaz del catalogo de entidades genéricas
    /// </summary>
    /// <returns></returns>
    public List<ServicioEntidadAPI> ObtienesServiciosICatalogoEntidadAPI()
    {
        if (serviciosCatalogoEntidad == null)
        {
            serviciosCatalogoEntidad = IntrospeccionEnsamblados.ObtienesServiciosICatalogoAPI();
        }
        return serviciosCatalogoEntidad;
    }

    /// <summary>
    /// Devuelve una lista de servicios de entidad genérica con datos de ruteo
    /// </summary>
    /// <returns></returns>
    public List<ServicioEntidadAPI> ObtienesServiciosIEntidadAPI()
    {
        if (serviciosEntidad == null)
        {
            serviciosEntidad = IntrospeccionEnsamblados.ObtienesServiciosIEntidadAPI();
        }
        return serviciosEntidad;
    }

    /// <summary>
    /// Recarga la lista de servicios de entidad para la API genérica
    /// </summary>
    public void RecargarServicios()
    {
        serviciosCatalogoEntidad = IntrospeccionEnsamblados.ObtienesServiciosICatalogoAPI();
        serviciosEntidad = IntrospeccionEnsamblados.ObtienesServiciosIEntidadAPI();
    }
}
