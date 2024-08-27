using comunes.primitivas;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using RestSharp;
using System.Drawing;
using System.Net.Http.Headers;

namespace comunicaciones.servicios.whatsapp
{
    public class ServicioWhatsapp : IServicioWhatsapp
    {
        private readonly ILogger<ServicioWhatsapp> logger;
        private readonly IConfiguration configuration;

        public ServicioWhatsapp(ILogger<ServicioWhatsapp> logger, IConfiguration configuration)
        {
            this.logger = logger;
            this.configuration = configuration;
        }





        public string Base64ToImage(string base64String)
        {
            logger.LogDebug($"Convirtiendo imagenBase64 - memoryStream");
            var pathCache = configuration.GetSection("WhatsappConfig:pathCache").Value;
            byte[] imageBytes = Convert.FromBase64String(base64String);

            using (MemoryStream ms = new MemoryStream(imageBytes))
            {
                Image image = Image.FromStream(ms);
                image.Save(pathCache);
                return pathCache;
            }
        }


        public string SubirImagen(string UrlBase, string Token, string RutaImg)
        {
            string id = "";
            dynamic data;
            try
            {
                string urlMedia = UrlBase + "media";
                using (var client = new RestClient())
                {
                    var request = new RestRequest(urlMedia, Method.Post);
                    // Configurar encabezados
                    request.AddHeader("Authorization", $"Bearer {Token}");
                    // Agregar archivo
                    request.AddFile("file", RutaImg, "image/jpeg");
                    // Parámetro específico
                    request.AddParameter("messaging_product", "whatsapp");
                    // Realizar la solicitud
                    var response = client.Execute(request);
                    // Imprimir el contenido de la respuesta
                    data = JObject.Parse(response.Content);
                    id = data.id;
                }
            }
            catch (Exception ex)
            {
                logger.LogDebug($"ServicioWhatsapp - Error al subir la imagen: {ex.Message}");
            }

            return id;
        }

        public async Task<Respuesta> EnviarImagen(string idImg,string UrlBase, string Token, string TelefonoDestino)
        {
            Respuesta r = new Respuesta();
            RestClient client = new RestClient();
            var urlEnvioImagen = UrlBase + "messages";
            RestRequest request = new RestRequest(urlEnvioImagen, Method.Post);
            request.AddHeader("Authorization", "Bearer " + Token);
            request.AddHeader("Content-Type", "application/json");

            // Construir el cuerpo del mensaje en formato JSON
            string jsonBody = $@"
            {{
                ""messaging_product"": ""whatsapp"",
                ""recipient_type"": ""individual"",
                ""to"": ""{TelefonoDestino}"",
                ""type"": ""image"",
                ""image"": {{
                    ""id"": ""{idImg}""
                }}
            }}";

            request.AddParameter("application/json", jsonBody, ParameterType.RequestBody);

            RestResponse response = client.Execute(request);

            if (response.IsSuccessful)
            {
                r.Ok = true;
                logger.LogDebug("Mensaje enviado con éxito.");

            }
            else
            {
                r.Error = new ErrorProceso()
                {
                    Codigo = CodigosError.COMUNICACIONES_WHATSAPP_ERROR_ENVIO_MENSAJE_IMG,
                    Mensaje = "No se pudo enviar la imagen a WhatsApp",
                    HttpCode = HttpCode.BadRequest
                };
                r.Ok = false;
                r.HttpCode = HttpCode.BadRequest;
                logger.LogDebug("ServicioWhatsapp resultado {ok} {code} {error}", r!.Ok, r!.HttpCode, r.Error);

            }

            return r;
        }

        public async Task<Respuesta> EnviarTxt(string UrlBase, 
                                               string Token, 
                                               string TelefonoDestino, 
                                               string mensaje)
        {
            Respuesta r = new Respuesta();
            RestClient client = new RestClient();
            var url = UrlBase + "messages";
            RestRequest request = new RestRequest(url, Method.Post);
            request.AddHeader("Authorization", "Bearer " + Token);
            request.AddHeader("Content-Type", "application/json");

            // Construir el cuerpo del mensaje en formato JSON
            string jsonBody = $@"
            {{
                ""messaging_product"": ""whatsapp"",
                ""recipient_type"": ""individual"",
                ""to"": ""{TelefonoDestino}"",
                ""type"": ""text"",
                ""text"": {{
                    ""body"": ""{mensaje}""
                }}
            }}";

            request.AddParameter("application/json", jsonBody, ParameterType.RequestBody);

            RestResponse response = client.Execute(request);

            if (response.IsSuccessful)
            {
                logger.LogDebug("Mensaje enviado con éxito.");
                r.Ok = true;
            }
            else
            {

                r.Error = new ErrorProceso()
                {
                    Codigo = CodigosError.COMUNICACIONES_WHATSAPP_ERROR_ENVIO_MENSAJE,
                    Mensaje = "No se pudo enviar el mensaje de WhatsApp",
                    HttpCode = HttpCode.BadRequest
                };
                r.Ok = false;
                r.HttpCode = HttpCode.BadRequest;
                logger.LogDebug("ServicioWhatsapp resultado {ok} {code} {error}", r!.Ok, r!.HttpCode, r.Error);
            }

            return r;
        }
        
    }
}
