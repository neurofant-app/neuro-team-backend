using apigenerica.primitivas.aplicacion;
using comunes.primitivas.seguridad;

namespace seguridad.api;

public class ConfiguracionSeguridad : IProveedorAplicaciones
{

    public const string CE_CAMPUS_PERM_LIST= "ce-campus-perm-list";
    public const string CE_CAMPUS_PERM_ADMIN = "ce'campus-perm-admin";
    public const string CE_CAMPUS_ROL_ADMIN = "ce-campus-rol-admin";
    

    public Task<List<Aplicacion>> ObtieneApliaciones()
    {
        List<Aplicacion> apps = [];

        apps.Add(
            new Aplicacion
            {
                ApplicacionId = Guid.Parse("00000000-0000-0000-1000-000000000001"),
                Nombre = "Seguridad",
                Descripcion = "Servicio de Seguridad",
                Modulos = []
            });


        return Task.FromResult(apps);
    }
}
