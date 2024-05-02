namespace extensibilidad.metadatos.atributos;


/// <summary>
/// Determina si una propiedad es un Id para la entidad
/// </summary>
[AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
public class IdAttribute : Attribute
{
    private readonly int _indice;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="indice">Posición en el caso de que la entidad tenga un ID compuesto</param>
    public IdAttribute(int indice = 0)
    {
        _indice = indice;
    }

    /// <summary>
    /// Posicion del identificador para las operaciones de CRUD de la entidad
    /// </summary>
    public virtual int Indice
    {
        get { return _indice; }
    }
}

