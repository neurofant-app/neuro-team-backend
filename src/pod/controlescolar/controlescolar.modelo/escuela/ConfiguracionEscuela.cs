using MongoDB.Bson.Serialization.Attributes;
using System.Diagnostics.CodeAnalysis;

namespace controlescolar.modelo.escuela;

/// <summary>
/// Parámetros de configuración de la escuela, estas propiedades se heredan a los planteles
/// </summary>
[ExcludeFromCodeCoverage]
public class ConfiguracionEscuela
{

    /// <summary>
    /// Idiomas para la internacionalización de contenido asociado a la escuela
    /// </summary>
    [BsonElement("lang")]
    public List<string>? IdiomaInternacionalizacion { get; set; }
}
