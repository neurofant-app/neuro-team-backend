
using apigenerica.model.servicios;
using Microsoft.AspNetCore.Mvc;
using seguridad.modelo;
using seguridad.modelo.instancias;
using System.ComponentModel.DataAnnotations;
using System.Reflection.PortableExecutable;

namespace seguridad.servicios.mysql;

public interface IServicioInstanciaAplicacionMysql : IServicioEntidadGenerica<InstanciaAplicacionMysql, InstanciaAplicacionMysql, InstanciaAplicacionMysql, InstanciaAplicacionMysql, string>
{
    Task<List<Rol>> GetRolesUsuarioInterno(string aplicacionId,string usuarioId, string dominioId, string uOrgID);
    Task<List<Permiso>> GetPermisosAplicacionInterno(string aplicacionId,string usuarioId,string dominioId,string uOrgID);
}

