using System.Diagnostics.CodeAnalysis;

namespace aprendizaje.model.almacenamiento;

/// <summary>
/// Define los espacios de almacenamiento físicos tipo BLOB
/// </summary>

[ExcludeFromCodeCoverage]
public class Almacenamiento
{

    /// <summary>
    /// Identificador único del espacio de almacenamiento
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Tipo del almacenamiento
    /// </summary>
    public TipoAlmacenamiento Tipo { get; set; }

    /// <summary>
    /// Identificador dado por la plataforma de almacenamiento 
    /// </summary>
    public required string PlataformaId { get; set; }

    /// <summary>
    /// Especifica si la cadena de conexión al almacenamiento
    /// se almacena en un secreto en la nube o localmente en la propiedad
    /// SecretoCadenaConexion
    /// </summary>
    public TipoSecreto TipoSecreto { get; set; }

    /// <summary>
    /// Nombre cadena de conexión o secreto donde se almamacena este valos para el almacenamiento
    /// </summary>
    public string SecretoCadenaConexion { get; set; } = "";

    /// <summary>
    /// Esatdo del almacenamiento
    /// </summary>
    public EstadoAlmacenamiento Estado { get; set; }

}
