using apigenerica.model.modelos;
using comunes.primitivas;
using System.Text.Json;

namespace apigenerica.model.servicios
{
    public static class Extensiones
    {
        public static T ReserializeCamelCase<T>(this object o)
        {
            var options = new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
            string s = JsonSerializer.Serialize(o, options);
            return JsonSerializer.Deserialize<T>(s, options)!;
        }


        public static RespuestaPayload<object> ReserializePayloadCamelCase(this object o)
        {
            var options = new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
            string s = JsonSerializer.Serialize(o, options);
            return JsonSerializer.Deserialize<RespuestaPayload<object>>(s, options)!;
        }

        public static RespuestaPayload<PaginaGenerica<object>> ReserializePaginaCamelCase(this object o)
        {
            var options = new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
            string s = JsonSerializer.Serialize(o, options);
            return JsonSerializer.Deserialize<RespuestaPayload<PaginaGenerica<object>>>(s, options)!;
        }
    }
}
