using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace gcf.conciliador.captcha.servicios;

public class ServicioCaptchaAPI : sat.bot.comun.IServicioCaptcha
{    

    public async Task<Guid> EnviaCaptcha(string rfc, string imagenBase64,List<string> telefonos)
    {
        var url = Environment.GetEnvironmentVariable("urlCaptcha");
        var httpClient = new HttpClient() { BaseAddress = new Uri(url) };
        string path = Path.Combine( url,"/accesocaptcha/facturacion");
        
        var result = await httpClient.PostAsync(path, new StringContent(JsonConvert.SerializeObject(new AccesoCaptchaFacturacionDTO() { RFC = rfc, CaptchaBase64 = imagenBase64, Fecha = DateTime.UtcNow,Telefonos=telefonos }), Encoding.UTF8, "application/json"));
        if (result.IsSuccessStatusCode)
        {
            string payload = await result.Content.ReadAsStringAsync();
            var id = JsonConvert.DeserializeObject<Guid>(payload);
            return id;
        }
        return Guid.Empty;        
    }

    public async Task<string> ObtieneLecturaCaptcha(Guid Id)
    {
        var url = Environment.GetEnvironmentVariable("urlCaptcha");
        var httpClient = new HttpClient() { BaseAddress = new Uri(url)};
        string path = Path.Combine(url,$"/accesocaptcha/facturacion/respuesta/{Id}");   
        var result = await httpClient.GetAsync(path);
        if (result.IsSuccessStatusCode)
        {
            string payload = await result.Content.ReadAsStringAsync();
            return payload;
        }
        else
        {
            return null;
        }
    }
}
