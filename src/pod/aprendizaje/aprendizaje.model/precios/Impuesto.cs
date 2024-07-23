namespace aprendizaje.model.precios;

/// <summary>
/// Impuesto asociado a un precio
/// </summary>
public class Impuesto
{
    public required string Clave { get; set; }

    public required decimal Valor { get; set; }
}
