using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace comunes.primitivas.atributos;

/// <summary>
/// Deermina si hay que verificar un permiso para acceder al método
/// </summary>
/// <param name="AppId">Identificador de la apliación a la que pertenece el permiso</param>
/// <param name="PermisoId">Identificador único del permiso</param>
[AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
public class PermisoAttribute(string AppId, string PermisoId) : Attribute
{
    private readonly string _AppId = AppId;
    private readonly string _PermisoId = PermisoId;

    /// <summary>
    /// Identificador de la apliación a la que pertenece el permiso
    /// </summary>
    public virtual string AppId { get { return _AppId; } }

    /// <summary>
    /// Identificador único del permiso
    /// </summary>
    public virtual string PermisoId { get { return _PermisoId; } }

}
