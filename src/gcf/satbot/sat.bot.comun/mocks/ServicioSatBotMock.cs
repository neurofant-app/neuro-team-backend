using modelo.repositorio.cfdi;
using procesador.sat.cfdi;
using sat.bot.captcha;
using sat.bot.modelo;

namespace sat.bot.comun.mocks;

public class ServicioSatBotMock : IServicioSatBot
{
    public Task ProcesarRFC(string RFC, byte[] PfXbytes, string Contraseña)
    {
        throw new NotImplementedException();
    }

    public async Task<ResultadoTareaConciliacion> ProcesaTareaRFC(TareaConciliacion tarea,string captcha,string rutaSqlite)
    {
        //SiatLogin login = new SiatLogin(tarea);
        ResultadoTareaConciliacion conciliacion = new(tarea) { OK = true};       
        return conciliacion;
    }
}
