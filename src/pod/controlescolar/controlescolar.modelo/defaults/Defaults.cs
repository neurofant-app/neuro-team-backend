using controlescolar.modelo.rolesescolares;

namespace controlescolar.modelo;
#pragma warning disable IDE0060 // Remove unused parameter

/// <summary>
/// Valores por default de los elemento de control escolar, esta clase es temporal y debe migrarse a un adminsitrador de catálogos 
/// que puedan mantenerse en la base de datos
/// </summary>
public static class Defaults
{
    /// <summary>
    /// Roles default de una escuela
    /// Esta lista de roles se adiciona a cada escuela durante su creacion y es posible editarlos
    /// </summary>
    /// <returns></returns>

    public static List<EntidadRolPersonaEscuela> RolesPersonaEscuelaBase(string Idioma)
    {
        return
        [
            new () { Id = 1, Nombre = "Alumno" },
            new () { Id = 2, Nombre = "Docente" },
            new () { Id = 3, Nombre = "Administrativo" }
            ];
    }


    public static List<EntidadMovimientoRolPersonaEscuela> MovimientosRolAlumnoPersonaEscuelaBase(string Idioma)
    {
        return
        [
            new () { Id = 1, Nombre = "Inscripión", RolPersonaEscuelaId =1, TipoMovimiento = TipoMovimientoRol.Alta },
            new () { Id = 2, Nombre = "Reinscripción", RolPersonaEscuelaId =1, TipoMovimiento = TipoMovimientoRol.Alta },
            new () { Id = 3, Nombre = "Baja reglamentaria", RolPersonaEscuelaId =1, TipoMovimiento = TipoMovimientoRol.BajaDefinitiva },
            new () { Id = 4, Nombre = "Solicitud baja temporal", RolPersonaEscuelaId =1, TipoMovimiento = TipoMovimientoRol.BajaTemporal },
            new () { Id = 5, Nombre = "Solicitud baja permanente", RolPersonaEscuelaId =1, TipoMovimiento = TipoMovimientoRol.BajaDefinitiva },
            ];
    }

    public static List<EntidadMovimientoRolPersonaEscuela> MovimientosRolDocentePersonaEscuelaBase(string Idioma)
    {
        return
        [
            new () { Id = 1, Nombre = "Contratación", RolPersonaEscuelaId = 2, TipoMovimiento = TipoMovimientoRol.Alta },
            new () { Id = 2, Nombre = "Fin de contrato", RolPersonaEscuelaId = 2, TipoMovimiento = TipoMovimientoRol.Alta },
            new () { Id = 3, Nombre = "Baja reglamentaria", RolPersonaEscuelaId =2, TipoMovimiento = TipoMovimientoRol.BajaDefinitiva },
            new () { Id = 4, Nombre = "Solicitud baja temporal", RolPersonaEscuelaId =2, TipoMovimiento = TipoMovimientoRol.BajaTemporal },
            new () { Id = 5, Nombre = "Solicitud baja permanente", RolPersonaEscuelaId =2, TipoMovimiento = TipoMovimientoRol.BajaDefinitiva },
            new () { Id = 6, Nombre = "Maternidad", RolPersonaEscuelaId =2, TipoMovimiento = TipoMovimientoRol.Permiso },
            ];
    }


    public static List<EntidadMovimientoRolPersonaEscuela> MovimientosRolAdministrativoPersonaEscuelaBase(string Idioma)
    {
        return
        [
            new () { Id = 1, Nombre = "Contratación", RolPersonaEscuelaId = 3, TipoMovimiento = TipoMovimientoRol.Alta },
            new () { Id = 2, Nombre = "Fin de contrato", RolPersonaEscuelaId = 3, TipoMovimiento = TipoMovimientoRol.Alta },
            new () { Id = 3, Nombre = "Baja reglamentaria", RolPersonaEscuelaId =3, TipoMovimiento = TipoMovimientoRol.BajaDefinitiva },
            new () { Id = 4, Nombre = "Solicitud baja temporal", RolPersonaEscuelaId =3, TipoMovimiento = TipoMovimientoRol.BajaTemporal },
            new () { Id = 5, Nombre = "Solicitud baja permanente", RolPersonaEscuelaId =3, TipoMovimiento = TipoMovimientoRol.BajaDefinitiva },
            new () { Id = 6, Nombre = "Maternidad", RolPersonaEscuelaId =3, TipoMovimiento = TipoMovimientoRol.Permiso },
            ];
    }

}
#pragma warning restore IDE0060 // Remove unused parameter