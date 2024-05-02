using sat.bot.modelo;

namespace sat.bot.comun;

public interface IServicioSatBot
{
    Task ProcesarRFC(string RFC, byte[] PfXbytes, string Contraseña);
    Task<ResultadoTareaConciliacion> ProcesaTareaRFC(TareaConciliacion tarea, string captcha,string rutaSqlite);
}
