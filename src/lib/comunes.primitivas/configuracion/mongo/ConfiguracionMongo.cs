using System.Text.Json.Serialization;

namespace comunes.primitivas.configuracion.mongo;

public sealed class ConfiguracionMongo
{
    [JsonPropertyName("conexion-default")]
    public string? ConexionDefault { get; set; }

    [JsonPropertyName("conexiones-entidad")]
    public List<ConexionEntidad>? ConexionesEntidad { get; set; }
}
