namespace extensibilidad.metadatos.atributos;

/// <summary>
/// DEterina si la propiead require de un catálogo para su llenado
/// </summary>
[AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
public class UsoCatalogoAttribute : Attribute
{

    private readonly bool _local;
    private readonly string _idCatalogo;
    private readonly string? _idMicroServicio;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="idCatalogo"Identificador de la entidad catálogo></param>
    /// <param name="local">Especifica si el catálogo se encuentra en el propio microservicio</param>
    /// <param name="idMicroServicio">Identificador del microservicio que aloja el catálogo en caso remoto</param>
    public UsoCatalogoAttribute(string idCatalogo, bool local = true, string? idMicroServicio = null)
    {
        _local = local;
        _idCatalogo = idCatalogo;
        _idMicroServicio = idMicroServicio;
    }

    /// <summary>
    /// Especifica el identificador del catálogo en la API para obtener los datos
    /// </summary>
    public virtual string IdCatalogo
    {
        get { return _idCatalogo; }
    }

    /// <summary>
    /// Identificador del microservicio para obtener el catálogo si no es local,
    /// este valor se utiliza zomo clave para la localización del la URL downstream
    /// en la configuración ya se en el backend o el frontend
    /// </summary>
    public virtual string? IdMicroservicio
    {
        get { return _idMicroServicio; }
    }


    /// <summary>
    /// Determina si el catálogo se obtiene del mismo microservicio
    /// </summary>
    public virtual bool Local
    {
        get { return _local; }
    }
}
