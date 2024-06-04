namespace extensibilidad.metadatos.atributos;

/// <summary>
/// Especifica si la propiedad debe pertenecer a un tipo de datos especifico
/// Por ejemplo en el caso donde hay subtipos fecha, hora, fechahora
/// 
/// Y determina también si al UI debe tener un tipo de despliegue específico, 
/// por ejemplo para tipos booleanos puedes ser un checkbox o un switch
/// </summary>
/// <remarks>
/// 
/// </remarks>
/// <param name="tipoDato">Tipo de datos asignado</param>
/// <param name="buscable">Especifica si la propiedad puede utilziare para búsquedas</param>
/// /// <param name="visible">Especifica si la propiedad es visuble para el usuario</param>
[AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
public class PropiedadAttribute(
    TipoDatos tipoDato = TipoDatos.SinAsignar, bool buscable = true, bool visible = true, string? valorDefault = null) : Attribute
{
    private readonly TipoDatos _tipoDatos = tipoDato;
    private readonly bool _buscable = buscable;
    private readonly bool _visible = visible;
    private readonly string? _valordefault = valorDefault;


    /// <summary>
    /// Especifica el tipo de datos
    /// </summary>
    public virtual TipoDatos TipoDatos
    {
        get { return _tipoDatos; }
    }

    /// <summary>git add 
    /// determina si la propiedad puede utilizarse para búsquedas
    /// </summary>
    public virtual bool Buscable
    {
        get { return _buscable; }
    }


    /// <summary>
    /// Determina si la propiedad es visible para el usuario
    /// </summary>
    public virtual bool Visible
    {
        get { return _visible; }
    }

    /// <summary>
    /// Valor defaultpara la propiedad serialziado como JSON
    /// </summary>
    public virtual string? ValorDefault
    {
        get { return _valordefault; }
    }
}
