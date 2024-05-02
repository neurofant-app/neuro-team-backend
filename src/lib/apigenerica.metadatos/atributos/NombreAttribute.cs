namespace extensibilidad.metadatos.atributos;


/// <summary>
/// Especifica la propiedad que debe utilizarse como nombre para el despliegue en la UI
/// </summary>
[AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
public class NombreAttribute : Attribute
{
    private readonly int _indice;
    private readonly bool _visible;
    private readonly int _ancho;

    /// <summary>
    /// 
    /// </summary>
    public NombreAttribute(int indice = 0, bool visible = false, int ancho = 1)
    {
        _indice = indice;
        _visible = visible;
        _ancho = ancho;
    }

    /// <summary>
    /// Determina la posición relativa a otras propiedades
    /// </summary>
    public virtual int Indice
    {
        get { return _indice; }
    }

    /// <summary>
    /// Determin si la propiedad es visible en el formulario o es información oculta
    /// </summary>
    public virtual bool Visible
    {
        get { return _visible; }
    }

    /// <summary>
    /// Especifica el ancho relativo de la propiedad
    /// Por ejemlo si el ancho de la UI es es divido en 16 una propiedad con ancho 1 será 1/16
    /// </summary>
    public virtual int Ancho
    {
        get { return _ancho; }
    }
}