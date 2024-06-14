using apigenerica.primitivas.aplicacion;
using comunes.primitivas.seguridad;
using controlescolar.servicios;

namespace controlescolar.api.seguridad;

public class ConfiguracionSeguridad : IProveedorAplicaciones
{

    public const string CE_CAMPUS_PERM_LIST = "ce-campus-perm-list";
    public const string CE_CAMPUS_PERM_ADMIN = "ce'campus-perm-admin";
    public const string CE_CAMPUS_ROL_ADMIN = "ce-campus-rol-admin";


    public Task<List<Aplicacion>> ObtieneApliaciones()
    {
        List<Aplicacion> apps = [];

        apps.Add(
            new Aplicacion
            {
                ApplicacionId = Guid.Parse(Constantes.AplicacionId),
                Nombre = "Control escolar",
                Descripcion = "Servicio de control escolar",
                Modulos = [
                 new Modulo()
                 {
                     Nombre = "Administrador de campus",
                     Descripcion = "Gestiona los campus de relacioandos con un dominio",
                     ModuloId = "campus-admin",
                     RolesPredefinidos = [
                             new()
                             {
                                 Nombre = "Administrador",
                                 Descripcion = "Tiene todos los permisos para administrar campus",
                                 Permisos = [Constantes.CE_CAMPUS_PERM_LIST, Constantes.CE_CAMPUS_PERM_ADMIN, Constantes.CE_CAMPUS_PERM_VIEW],
                                 Personalizado = false,
                                 RolId = Constantes.CE_CAMPUS_ROL_ADMIN
                             },
                             new()
                             {
                                 Nombre = "Visor de campus",
                                 Descripcion = "Tiene permisos para visualizar los campus y sus propieades",
                                 Permisos = [Constantes.CE_CAMPUS_PERM_LIST, Constantes.CE_CAMPUS_PERM_VIEW],
                                 Personalizado = false,
                                 RolId = Constantes.CE_CAMPUS_ROL_VISOR
                             }
                         ],
                     Permisos = [
                             new()
                             {
                                 Nombre = "Administrar campus",
                                 Descripcion = "Permite crear, editar y eliminar campus",
                                 Ambito = AmbitoPermiso.Global,
                                 PermisoId = Constantes.CE_CAMPUS_PERM_ADMIN
                             },
                             new()
                             {
                                 Nombre = "Listar campus",
                                 Descripcion = "Permite obtener la lista de campus",
                                 Ambito = AmbitoPermiso.Global,
                                 PermisoId = Constantes.CE_CAMPUS_PERM_LIST
                             },
                             new()
                             {
                                 Nombre = "Detalle campus",
                                 Descripcion = "Permite visualizar las propiedades del campus",
                                 Ambito = AmbitoPermiso.Global,
                                 PermisoId = Constantes.CE_CAMPUS_PERM_VIEW
                             }
                         ]
                 }
                ]
            });


        return Task.FromResult(apps);
    }
}
