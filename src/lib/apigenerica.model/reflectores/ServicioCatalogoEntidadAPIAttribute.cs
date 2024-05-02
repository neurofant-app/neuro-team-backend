namespace apigenerica.model.reflectores;

/// <summary>
/// Define los atributos de API para la entidad de catálogo
/// </summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
public class ServicioCatalogoEntidadAPIAttribute : Attribute
{
    private Type _entidad;

    /// <summary>
    /// Crea una instancia del atributo de entidades
    /// </summary>
    /// <param name="NombreEntidad">Tipo asociado a la entidad para el ruteo en el comtrolador</param>
    public ServicioCatalogoEntidadAPIAttribute(Type entidad) {
        _entidad = entidad;
    }

    /// <summary>
    /// Nombre asociado a la entidad para el ruteo en el comtrolador
    /// </summary>
    public virtual Type Entidad
    {
        get { return _entidad; }
    }

}
