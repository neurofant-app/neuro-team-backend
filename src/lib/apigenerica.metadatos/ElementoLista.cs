namespace extensibilidad.metadatos;
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

/// <summary>
/// DEfine un elmento para el tipo lista de metadatos
/// </summary>
public class ElementoLista
{
    /// <summary>
    /// Identificador único del elemento de la lista
    /// </summary>
    public string Id { get; set; }

    /// <summary>
    /// Texto para despliegue humano del elemento
    /// </summary>

    public string Nombre { get; set; }


    /// <summary>
    /// Valor crudo del elemento
    /// </summary>
    public string Valor { get; set; }

    /// <summary>
    /// Posición relativa el elemento en el despliegue
    /// </summary>
    public int Posicion { get; set; }
}

#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.