using apigenerica.primitivas.aplicacion;
using comunes.primitivas.seguridad;

namespace controlescolar.api.seguridad;

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
                                 Permisos = [CE_CAMPUS_PERM_LIST, CE_CAMPUS_PERM_ADMIN],
                                 Personalizado = false,
                                 RolId = CE_CAMPUS_ROL_ADMIN
                             }

                         ],
                     Permisos = [
                             new()
                             {
                                 Nombre = "Administrar campus",
                                 Descripcion = "Permite crear, editar y eliminar campus",
                                 Ambito = AmbitoPermiso.Global,
                                 PermisoId = CE_CAMPUS_PERM_ADMIN
                             },
                         new()
                         {
                             Nombre = "Listar aplicaciones",
                             Descripcion = "Permite obtener la lista de campuss",
                             Ambito = AmbitoPermiso.Global,
                             PermisoId = CE_CAMPUS_PERM_LIST
                         }
                         ]
                 }
                ]
            });


        return Task.FromResult(apps);
    }
}
