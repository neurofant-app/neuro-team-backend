using sat.bot.modelo;

namespace sat.bot.captcha;

public class ResultadoHttp 
{
    public bool Ok { get; set; } = false;
    public int? HttpCode { get; set; } = null;
    public Dictionary<string, string> Propiedades { get; set; } = new Dictionary<string, string>();
    public List<string> Errores { get; set; } = new List<string>();
    public List<Exception> Exceptions { get; set; } = new List<Exception>();
    public List<string> PayLoad { get; set; } = new List<string>();
}
