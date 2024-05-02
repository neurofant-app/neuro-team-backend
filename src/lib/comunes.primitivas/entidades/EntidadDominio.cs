namespace comunes.primitivas;

/// <summary>
/// DEfine los datos que definen una entidad como perteneciente aun dominio
/// </summary>
public interface IEntidadDominio
{
    /// <summary>
    /// Identificador único del dominio
    /// </summary>
    Guid DominioId { get; set; }

    /// <summary>
    /// Identificador único de la unidad organziacional
    /// </summary>
    Guid? UnidadORganizacionalId { get; set; }
}
