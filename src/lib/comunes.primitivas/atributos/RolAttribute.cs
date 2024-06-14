namespace comunes.primitivas.atributos;

/// <summary>
/// Deermina si hay que verificar un rol para acceder al método
/// </summary>
/// <param name="AppId">Identificador de la apliación a la que pertenece el permiso</param>
/// <param name="PermisoId">Identificador único del rol</param>
[AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
public class RolAttribute(string AppId, string PermisoId) : Attribute
{
    private readonly string _AppId = AppId;
    private readonly string _PermisoId = PermisoId;

    /// <summary>
    /// Identificador de la apliación a la que pertenece el permiso
    /// </summary>
    public virtual string AppId { get { return _AppId; } }

    /// <summary>
    /// Identificador único del rol
    /// </summary>
    public virtual string RolId { get { return _PermisoId; } }

}
