using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace comunicaciones.modelo.email
{
    public class DatosPlantilla
    {
        public string? UrlBase { get; set; }
        public string? Activacion { get; set; }
        public string? Nombre { get; set; }
        public string? Email { get; set; }
        public string? FechaLimite { get; set; }
        public string? Remitente { get; set; }
        public string? Logo64 { get; set; }
        public string? Mensaje { get; set; }
    }
}
