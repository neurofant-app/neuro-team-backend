using apigenerica.primitivas.aplicacion;
using comunes.primitivas.seguridad;

namespace espaciotrabajo.api.seguridad;

public partial class ConfiguracionSeguridad : IProveedorAplicaciones
{

    public Task<List<Aplicacion>> ObtieneApliaciones()
    {
        List<Aplicacion> apps = [];

        apps.Add(
            new Aplicacion
            {
                ApplicacionId = Guid.Parse(APP_ID_WS_NEUROPAD),
                Nombre = "NeuroPad",
                Descripcion = "Espacio de trabajo NeuroPad",
                Modulos = [ ModuloGalería(), ControlAcceso()]
            });

        apps.Add(
           new Aplicacion
           {
               ApplicacionId = Guid.Parse(APP_ID_WS_NEUROTEAM),
               Nombre = "NeuroTeam",
               Descripcion = "Espacio de trabajo NeuroTeam",
               Modulos = [ControlAcceso()]
           });

        return Task.FromResult(apps);

    }
}

