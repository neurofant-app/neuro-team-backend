using comunes.primitivas.seguridad;
namespace espaciotrabajo.api.seguridad;

public partial class ConfiguracionSeguridad
{

    /// <summary>
    /// Configuración del módulo de galería de NeuroPad
    /// </summary>
    /// <returns></returns>
    public static Modulo ModuloGalería() {

        return new Modulo()
        {
            Nombre = "Galería",
            Descripcion = "Galería de medios de la solución",
            ModuloId = "galería",
            RolesPredefinidos = [
                             new()
                             {
                                 Nombre = "Administrador",
                                 Descripcion = "Todos los permisos para la administración de galerías",
                                 Permisos = [GAL_PERM_ADMIN],
                                 Personalizado = false,
                                 RolId = GALERIA_ROL_ADMIN
                             }

                         ],
            Permisos = [
                             new()
                             {
                                 Nombre = "Administrar galerías",
                                 Descripcion = "Permite crear, editar y eliminar galerías",
                                 Ambito = AmbitoPermiso.Global,
                                 PermisoId = GAL_PERM_ADMIN
                             },
                             new()
                             {
                                 Nombre = "Listar galerías",
                                 Descripcion = "Permite obtener la lista de galerías",
                                 Ambito = AmbitoPermiso.Global,
                                 PermisoId = GAL_PERM_LIST
                             },
                             new()
                             {
                                 Nombre = "Administrar contenido galería",
                                 Descripcion = "Permite administrar el contenido de una galería",
                                 Ambito = AmbitoPermiso.Contextual,
                                 PermisoId = GAL_PERM_CONTENIDO_ADMIN
                             },
                             new()
                             {
                                 Nombre = "Visualizar contenido galería",
                                 Descripcion = "Permite visualizar el contenido de una galería",
                                 Ambito = AmbitoPermiso.Contextual,
                                 PermisoId = GAL_PERM_CONTENIDO_VIEW
                             }
                         ]
        };
    }

    public static Modulo ControlAcceso()
    {
        return new Modulo()
        {
            Nombre = "Control de acceso",
            Descripcion = "Módulo de control de acceso del espacio de trabajo",
            ModuloId = "controla",
            RolesPredefinidos = [
                             new()
                             {
                                 Nombre = "Administrador",
                                 Descripcion = "Todos los permisos para la administración del control de acceso",
                                 Permisos = [ACL_PERM_ADMIN],
                                 Personalizado = false,
                                 RolId = ACL_ROL_ADMIN
                             }

                         ],
            Permisos = [
                             new()
                             {
                                 Nombre = "Administrar control de acceso",
                                 Descripcion = "Permite configurar la totalidad del control de acceso a la aplicación",
                                 Ambito = AmbitoPermiso.Global,
                                 PermisoId = ACL_PERM_ADMIN
                             },
                             new()
                             {
                                 Nombre = "Administrar miembros del espacio de trabajo",
                                 Descripcion = "Permite gestionar los miembros de espacio de trabajo",
                                 Ambito = AmbitoPermiso.Global,
                                 PermisoId = ACL_PERM_MIEMBROS
                             },
                             new()
                             {
                                 Nombre = "Administrar roles y miembors del espacio de trabajo",
                                 Descripcion = "Permite gestionar los roles de espacio de trabajo y sus miembros",
                                 Ambito = AmbitoPermiso.Global,
                                 PermisoId = ACL_PERM_ROLES
                             }
                         ]
        };
    }
}
