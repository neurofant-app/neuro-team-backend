using organizacion.model.unidadorganizacional;
using System.Diagnostics.CodeAnalysis;

namespace organizacion.model.dominio;

/// <summary>
/// El dominio es el contenedor de todos los recursos de una cuenta
/// </summary>
[ExcludeFromCodeCoverage]
public class Dominio
{
    /// <summary>
    /// Identificador único del dominio
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Nombre de dominio
    /// </summary>
    public string Nombre { get; set; }

    /// <summary>
    /// Identificador único del origen del dominio
    /// </summary>
    public Guid OrigenId { get; set; }

    /// <summary>
    /// Tipo de origen del dominio
    /// </summary>
    public TipoOrigenDominio TipoOrigen { get; set; } = TipoOrigenDominio.Usuario;

    /// <summary>
    /// Indica si el dominio está activo
    /// </summary>
    public bool Activo { get; set; } = true;


    /// <summary>
    /// Unidades organizacionales asociadas al dominio
    /// </summary>
    public List<UnidadOrganizacional> UnidadesOrganizacionales { get; set; } = [];

 
}
