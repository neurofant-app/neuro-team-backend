
using System.Net;

namespace sat.bot.modelo.conciliacion;

public class ConsultaConciliacion
{
    public bool Ok { get; set; } = false;
    public int? HttpCode { get; set; } = null;
    public Dictionary<string, string> Propiedades { get; set; } = new Dictionary<string, string>();
    public List<string> Errores { get; set; } = new List<string>();
    public List<string> PayLoad { get; set; } = new List<string>();
    public DateTime FechaInicio { get; set; }
    public DateTime FechaFin { get; set; }

}

