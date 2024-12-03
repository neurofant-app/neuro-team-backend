using apigenerica.primitivas.aplicacion;
using comunes.primitivas.seguridad;

namespace organizacion.api.seguridad;

public class ConfiguracionSeguridad : IProveedorAplicaciones
{
    public Task<List<Aplicacion>> ObtieneApliaciones()
    {
        List<Aplicacion> apps = [];

        apps.Add(
            new Aplicacion
            {
                ApplicacionId = Guid.Parse(ConfiguracionSeguridadConstantes.AplicacionId),
                Nombre = "Gestor de Organizaciones",
                Descripcion = "Gestor de Organizaciones de la solución",
                Modulos = [
                 new Modulo()
                 {
                     Nombre = "Administrador de Organizaciones",
                     Descripcion = "Permite administrar las Organizaciones de la solución",
                     ModuloId = "org-manager",
                     RolesPredefinidos = [
                             new()
                             {
                                 Nombre = "Administrador",
                                 Descripcion = "Todos los permisos para la administración de organizaciones",
                                 Permisos = [ConfiguracionSeguridadConstantes.ORG_DOMINIO_PERM_ADMIN],
                                 Personalizado = false,
                                 RolId = ConfiguracionSeguridadConstantes.ORG_DOMINIO_ROL_ADMIN
                             }

                         ],
                     Permisos = [
                             new()
                             {
                                 Nombre = "Administrar Organizaciones",
                                 Descripcion = "Permite crear, editar y eliminar Organizaciones",
                                 Ambito = AmbitoPermiso.Global,
                                 PermisoId = ConfiguracionSeguridadConstantes.ORG_DOMINIO_PERM_ADMIN
                             },
                         new()
                         {
                             Nombre = "Listar dominios",
                             Descripcion = "Permite obtener la lista de dominios",
                             Ambito = AmbitoPermiso.Global,
                             PermisoId = ConfiguracionSeguridadConstantes.ORG_DOMINIO_PERM_LIST
                         }
                         ]
                 },
                    new Modulo()
                    {
                        Nombre = "Administrador de UnidadesOrganizacionales",
                        Descripcion = "Permite administrar las UnidadesOrganizacionales de la solución",
                        ModuloId = "unid-manager",
                        RolesPredefinidos = [
                            new()
                            {
                                Nombre = "Administrador",
                                Descripcion = "Todos los permisos para la administración de unidades organizacionales",
                                Permisos = [ConfiguracionSeguridadConstantes.ORG_UNIDADORGANIZACIONAL_PERM_ADMIN],
                                Personalizado = false,
                                RolId = ConfiguracionSeguridadConstantes.ORG_UNIDADORGANIZACIONAL_ROL_ADMIN
                            }

                        ],
                        Permisos = [
                            new()
                            {
                                Nombre = "Administrar UnidadesOrganizacionales",
                                Descripcion = "Permite crear, editar y eliminar UnidadesOrganizacionales",
                                Ambito = AmbitoPermiso.Global,
                                PermisoId = ConfiguracionSeguridadConstantes.ORG_UNIDADORGANIZACIONAL_PERM_ADMIN
                            }
                        ]
                    },
                    new Modulo()
                    {
                        Nombre = "Administrador de UsuariosGrupo",
                        Descripcion = "Permite administrar los UsuariosGrupo de la solución",
                        ModuloId = "usg-manager",
                        RolesPredefinidos = [
                            new()
                            {
                                Nombre = "Administrador",
                                Descripcion = "Todos los permisos para la administración de unidades UsuariosGrupo",
                                Permisos = [ConfiguracionSeguridadConstantes.ORG_USUARIOGRUPO_PERM_ADMIN],
                                Personalizado = false,
                                RolId = ConfiguracionSeguridadConstantes.ORG_usuariogrupo_ROL_ADMIN
                            }

                        ],
                        Permisos = [
                            new()
                            {
                                Nombre = "Administrar UsuariosGrupo",
                                Descripcion = "Permite crear, editar y eliminar UsuariosGrupo",
                                Ambito = AmbitoPermiso.Global,
                                PermisoId = ConfiguracionSeguridadConstantes.ORG_USUARIOGRUPO_PERM_ADMIN
                            }
                        ]
                    },
                ]

            });
        return Task.FromResult(apps);
    }
}
