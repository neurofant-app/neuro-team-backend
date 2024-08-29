using apigenerica.primitivas.aplicacion;
using comunes.primitivas.seguridad;

namespace espaciotrabajo.api.seguridad;

public class ConfiguracionSeguridad : IProveedorAplicaciones
{

    public const string APP_MANAGER_PERM_LIST = "app-manager-perm-list";
    public const string APP_MANAGER_PERM_ADMIN = "app-manager-perm-admin";
    public const string APP_MANAGER_ROL_ADMIN = "app-manager-rol-admin";



    public Task<List<Aplicacion>> ObtieneApliaciones()
    {
        List<Aplicacion> apps = [];

        apps.Add(
            new Aplicacion
            {
                ApplicacionId = Guid.Parse("00000000-0000-0000-0000-000000000001"),
                Nombre = "Gestor de aplicaciones",
                Descripcion = "Gestor de aplicaciónes de la solución",
                Modulos = [
                 new Modulo()
                 {
                     Nombre = "Administrador de aplicaciones",
                     Descripcion = "Permite administrar las aplicaciones de la solución",
                     ModuloId = "app-manager",
                     RolesPredefinidos = [
                             new()
                             {
                                 Nombre = "Administrador",
                                 Descripcion = "Todos los permisos para la administración de aplicaciones",
                                 Permisos = [APP_MANAGER_PERM_ADMIN, APP_MANAGER_PERM_LIST],
                                 Personalizado = false,
                                 RolId = APP_MANAGER_ROL_ADMIN
                             }

                         ],
                     Permisos = [
                             new()
                             {
                                 Nombre = "Administrar aplicaciones",
                                 Descripcion = "Permite crear, editar y eliminar aplciaciones",
                                 Ambito = AmbitoPermiso.Global,
                                 PermisoId = APP_MANAGER_PERM_ADMIN
                             },
                         new()
                         {
                             Nombre = "Listar aplicaciones",
                             Descripcion = "Permite obtener la lista de aplicaciones",
                             Ambito = AmbitoPermiso.Global,
                             PermisoId = APP_MANAGER_PERM_LIST
                         }
                         ]
                 }
                ]
            });

        apps.Add(
           new Aplicacion
           {
               ApplicacionId = Guid.Parse("00000000-0000-0000-0000-000000000002"),
               Nombre = "Aplicacion 2 Demo",
               Descripcion = "Descripcion de la Aplicacion 2  Actualizado",
               Modulos = new List<Modulo>()
           });

        return Task.FromResult(apps);
    }
}

