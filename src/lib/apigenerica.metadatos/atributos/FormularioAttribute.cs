namespace extensibilidad.metadatos.atributos;

/// <summary>
/// Define la caonfiguración de despliegue de una propiedade dentro deun formulario
/// </summary>
[AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
public class FormularioAttribute : Attribute
{
    private readonly int _indice;
    private readonly bool _visible;
    private readonly int _ancho;
    private readonly int _renglon;
    private readonly TipoDespliegue _tipoDespliegue;

    /// <summary>
    /// <param name="indice">Posición relativa</param>
    /// <param name="visible">Visible u oculta</param>
    /// <param name="ancho">Ancho relateivo en porcentaje</param>
    /// <param name="tipoDespliegue">Posición relativa a otras propiedades</param>
    /// <param name="renglon">Define más de un elemento en el mismo renglon</param>
    /// </summary>
    public FormularioAttribute(int indice = 1, bool visible = true, int ancho = 100, TipoDespliegue tipoDespliegue = TipoDespliegue.Default, int renglon = 0)
    {
        _indice = indice;
        _visible = visible;
        _ancho = ancho;
        _tipoDespliegue = tipoDespliegue;
        _renglon = renglon;
    }

    /// <summary>
    /// Define si dos  o más elemento deben incluire en el mismo renglon
    /// </summary>
    public virtual int Renglon
    {
        get { return _renglon; }
    }

    /// <summary>
    /// Determina la posición relativa a otras propiedades
    /// </summary>
    public virtual TipoDespliegue TipoDespliegue
    {
        get { return _tipoDespliegue; }
    }

    /// <summary>
    /// Posicion relativa de la propiedad en el formulario en relación con otras propiedades
    /// </summary>
    public virtual int Indice
    {
        get { return _indice; }
    }

    /// <summary>
    /// Determina si la propiedad es visible o debe mantenerse oculta
    /// </summary>
    public virtual bool Visible
    {
        get { return _visible; }
    }

    /// <summary>
    /// Define el aencho relativo de la propiedad en porcentaje del área de despliegue
    /// y relativas a las otras propiedades a desplegar
    /// </summary>
    public virtual int Ancho
    {
        get { return _ancho; }
    }


}