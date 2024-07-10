namespace apigenerica.model.reflectores;

/// <summary>
/// Define los atributos de API para la entidad
/// </summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
public class ServicioEntidadAPIAttribute: Attribute
{
    private Type _entidad;
    private string? _driver;

    /// <summary>
    /// Crea una instancia del atributo de entidades
    /// </summary>
    /// <param name="NombreEntidad">Tipo asociado a la entidad para el ruteo en el comtrolador</param>
    public ServicioEntidadAPIAttribute(Type entidad,string? driver=null) {
        _entidad = entidad;
        _driver = driver;
    }

    /// <summary>
    /// Nombre asociado a la entidad para el ruteo en el comtrolador
    /// </summary>
    public virtual Type Entidad
    {
        get { return _entidad; }
    }

 
    /// <summary>
    /// Nombre del contexto cpn el que trabaja
    /// </summary>
    public virtual string Driver
    {
        get { return _driver; }
    }
}
