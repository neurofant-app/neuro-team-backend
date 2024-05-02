using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aplicaciones.services.extensiones;

public static class ExtensionesInvitacion
{
    public static string LeeUrlBase(this IConfiguration _configuration)
    {
        return _configuration.GetSection("emailing")["url-base"];
    }

    public static string LeeTemaRegistro(this IConfiguration _configuration)
    {
        return _configuration.GetSection("emailing")["tema-email-registro"];
    }
}
