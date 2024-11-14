using System.Diagnostics.CodeAnalysis;

namespace usuario.model;

/// <summary>
/// Define un producto o servicio asociado al usuario
/// </summary>
[ExcludeFromCodeCoverage]
public class Producto
{
    /// <summary>
    /// Identificador único del producto
    /// </summary>
    public required string Id { get; set; }

    /// <summary>
    /// Nombre del producto
    /// </summary>
    public required string Nombre { get; set; }

    /// <summary>
    /// Especifica si el producto se encuentra activo
    /// </summary>
    public bool Activo { get; set; }
}
