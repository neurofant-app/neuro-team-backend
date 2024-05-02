using System.Threading.Tasks;

namespace gcf.conciliador.captcha.servicios;

public interface IServicioSatBot
{
    Task ProcesarRFC(string RFC, byte[] PfXbytes, string Contraseña);
    //Task<ResultadoTareaConciliacion> ProcesaTareaRFC(TareaConciliacion tarea, string captcha,string rutaSqlite);
}
