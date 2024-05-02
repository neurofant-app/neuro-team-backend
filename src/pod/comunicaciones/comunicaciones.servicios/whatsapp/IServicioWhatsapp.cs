using comunes.primitivas;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace comunicaciones.servicios.whatsapp
{
    public interface IServicioWhatsapp
    {
        string SubirImagen(string UrlBase, string Token, string RutaImg);
        string Base64ToImage(string base64String);
        Task<Respuesta> EnviarImagen(string UrlBase, string IdImg, string Token, string TelefonoDestino);


    }
}
