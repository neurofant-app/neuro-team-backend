namespace apigenerica.primitivas;

public interface IConfiguracionAPIEntidades
{
    List<string> ObtieneRutasControladorGenerico();
    List<ServicioEntidadAPI> ObtienesServiciosIEntidadAPI();
    List<ServicioEntidadAPI> ObtienesServiciosICatalogoEntidadAPI();
    void RecargarServicios();
}

