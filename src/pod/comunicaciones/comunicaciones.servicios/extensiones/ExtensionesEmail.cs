using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace comunicaciones.servicios.extensiones;

public static class ExtensionesEmail
{
    public static string LeeUrlBase(this IConfiguration _configuration)
    {
        return _configuration.GetSection("emailing")["url-base"];
    }

    public static string LeePlantillaRegistro(this IConfiguration _configuration)
    {
        string plantilla = "";
        plantilla = _configuration.GetSection("emailing")["plantilla-email-registro"];
        string ruta = Path.Combine(Environment.CurrentDirectory, plantilla);
        string contenido = "";
        if (File.Exists(ruta))
        {
            contenido = File.ReadAllText(ruta);
        }
        return contenido;
    }

    public static string LeeTemaRegistro(this IConfiguration _configuration)
    {
        return _configuration.GetSection("emailing")["tema-email-registro"];
    }
}
