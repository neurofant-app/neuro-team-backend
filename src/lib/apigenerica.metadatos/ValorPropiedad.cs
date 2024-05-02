namespace extensibilidad.metadatos;

/// <summary>
/// Velor asignado a una propiedad, si la propiedad puede ten
/// </summary>
public class ValorPropiedad
{
    /// <summary>
    /// IDentificador único como referencia CRUD, se calcula manualmente en base al contexto
    /// Por ejemplo para base NoSQL puede se un entero convertido a string en un arreglo de valores
    /// </summary>
    public string Id { get; set; }

    /// <summary>
    /// Identificador de la propiedad en el modelo
    /// </summary>
    public required string PropiedaId { get; set; }

    /// <summary>
    /// Valor de la propiedad cuando el tipo de dato es texto
    /// </summary>
    public string? ValorString { get; set; }

    /// <summary>
    /// Valor de la propiedad cuando el tipo de dato es entero
    /// </summary>
    public int? ValorEntero { get; set; }

    /// <summary>
    /// Valor de la propiedad cuando el tipo de dato es entero largo
    /// </summary>
    public long? ValorEnteroLargo { get; set; }

    /// <summary>
    /// Valor de la propiedad cuando el tipo de dato es booleano
    /// </summary>
    public bool? ValorBooleano { get; set; }


    /// <summary>
    /// Valor de la propiedad cuando el tipo de dato es fecha, hora o fechahora
    /// </summary>
    public DateTime? ValorFechaHora { get; set; }
}
