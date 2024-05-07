using System.Text.Json.Serialization;

namespace comunes.primitivas.configuracion.mongo;

public sealed class ConexionEntidad
{
    [JsonPropertyName("entidad")]
    public required string Entidad { get; set; }

    [JsonPropertyName("conexion")]
    public string? Conexion { get; set; }

    [JsonPropertyName("esquema")]
    public required string Esquema { get; set; }

    [JsonPropertyName("coleccion")]
    public required string Coleccion { get; set; }

}
