using System.Diagnostics.CodeAnalysis;

namespace usuario.model;

/// <summary>
/// Datos de los dominios en los que participa el usuario
/// </summary>
[ExcludeFromCodeCoverage]
public class DominioUsuario
{
    /// <summary>
    /// Identificador único del dominio
    /// </summary>
    public required string DominioId { get; set; }

    /// <summary>
    /// Indica si el usuario se encuentra activo en el dominio
    /// </summary>
    public bool Activo { get; set; }

    /// <summary>
    /// Lista de las OU a las que pertenece el usuario y se encuentra activo en el dominio 
    /// </summary>
    public List<string> UnidadesOrganizacionalesId { get; set; } = [];

    /// <summary>
    /// Productos asociados al perfil en el dominio
    /// </summary>
    public List<Producto> Productos { get; set; } = [];

}
