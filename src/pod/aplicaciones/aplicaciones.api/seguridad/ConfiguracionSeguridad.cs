using comunes.primitivas.seguridad;

namespace aplicaciones.api.seguridad;

public static class ConfiguracionSeguridad
{
    public static List<Aplicacion> ObtieneAplicaciones()
    {
        List<Aplicacion> apps = [];

        apps.Add(
            new Aplicacion
            {
                ApplicacionId = Guid.Parse("00000000-0000-0000-0000-000000000001"),
                Nombre = "Aplicacion 1 Demo",
                Descripcion = "Descripcion de la Aplicacion 1",
                Modulos = new List<Modulo>()
            }) ;

        apps.Add(
           new Aplicacion
           {
               ApplicacionId = Guid.Parse("00000000-0000-0000-0000-000000000002"),
               Nombre = "Aplicacion 2 Demo",
               Descripcion = "Descripcion de la Aplicacion 2  Actualizado",
               Modulos = new List<Modulo>()
           });
        return apps;
    }
}
