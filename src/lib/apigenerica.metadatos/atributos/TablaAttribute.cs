namespace extensibilidad.metadatos.atributos;

/// <summary>
/// Define las configuración de la propiedad en un entorno tabular
/// </summary>
[AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
public class TablaAttribute : Attribute
{
    private readonly int _indice;
    private readonly bool _visible;
    private readonly bool _ordenable;
    private readonly bool _alternable;
    private readonly int _ancho;


    /// <summary>
    /// <param name="indice">Posición relativa</param>
    /// <param name="visible">Visible u oculta</param>
    /// <param name="ancho">Ancho relateivo en unidades de UI</param>
    /// <param name="ordenable">Determina si la columna es ordenable</param>
    /// <param name="alternable">Determina si la columna puede ser utilizada para búsqueda</param>
    /// </summary>
    public TablaAttribute(int indice = 1, bool visible = true, int ancho = 1, bool ordenable = true, bool alternable = true)
    {
        _indice = indice;
        _visible = visible;
        _ancho = ancho;
        _ordenable = ordenable;
        _alternable = alternable;
    }

    /// <summary>
    /// Posicion relativa de la propiedad en el formulario en relación con otras propiedades
    /// </summary>
    public virtual int Indice
    {
        get { return _indice; }
    }

    /// <summary>
    /// Determina si la columna es ordenable
    /// </summary>
    public virtual bool Ordenable
    {
        get { return _ordenable; }
    }

    /// <summary>
    /// Determina si la columna puede alternar su visibilidad
    /// </summary>
    public virtual bool Alternable
    {
        get { return _alternable; }
    }

    /// <summary>
    /// Determina si la propiedad es visible o debe mantenerse oculta
    /// </summary>
    public virtual bool Visible
    {
        get { return _visible; }
    }

    /// <summary>
    /// Define el ancho relativo de la propiedad un unidades de división de la UI
    /// y relativas a las otras propiedades a desplegar
    /// </summary>
    public virtual int Ancho
    {
        get { return _ancho; }
    }

}
