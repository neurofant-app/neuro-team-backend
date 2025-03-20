using System.Text.Json;

namespace apigenerica.model.servicios
{
    public static class Extensiones
    {
        public static T ReserializeCamelCase<T>(this object o)
        {
            var options = new JsonSerializerOptions() { PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase };
            string s = JsonSerializer.Serialize(o, options);
            return JsonSerializer.Deserialize<T>(s, options)!;
        }
    }
}
