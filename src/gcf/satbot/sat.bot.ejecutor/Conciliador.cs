using Microsoft.EntityFrameworkCore;
using modelo.repositorio.cfdi;
using procesador.sat.cfdi;
using sat.bot.captcha;
using sat.bot.comun;
using sat.bot.modelo;
using sat.bot.modelo.conciliacion;
using sat.bot.services;




//using sat.bot.comun;
using System.Net;

namespace sat.bot.ejecutor;

// Ejecuta una llamada de conciliación por fechas para un RFC
public class Conciliador
{

    public async Task<ResultadoTareaConciliacion> Conciliar(TareaConciliacion tarea, 
        IGestorArchivos gestorArchivos, 
        IServicioSatBot  servicioSatBot, 
        IAdministradorSecretos administradorSecretos,
        IServicioCaptcha servicioCaptcha,
        string urlCaptcha) {

        ResultadoTareaConciliacion conciliacion = new(tarea) { OK = false };

        string? rutaSQlite = await gestorArchivos.RutaRWDBSqlite(tarea.RFC, tarea.SubscripcionId, $"{tarea.SubscripcionId}/{tarea.RFC}");
        if (!string.IsNullOrEmpty(rutaSQlite)) {
            SiatLogin login = new SiatLogin(tarea, new ProcesadorCFDI(new DbContextSqLite(rutaSQlite)));
            string captcha = await login.LoginCaptcha("https://portalcfdi.facturaelectronica.sat.gob.mx");
            if(!string.IsNullOrEmpty(captcha))
             {
                byte[] bytes = Convert.FromBase64String(captcha);
                string file = Path.Combine(@"C:\tempC", $"{Guid.NewGuid()}.jpg");
                File.WriteAllBytes(file, bytes);
                //var idCaptcha = await servicioCaptcha.EnviaCaptcha(tarea.RFC,captcha,tarea.Telefonos);
                var idCaptcha = Guid.Empty;
                var timer = new PeriodicTimer(TimeSpan.FromSeconds(10));
                int count = 0;
                string? textoCaptcha =  Console.ReadLine();
                //while (await timer.WaitForNextTickAsync())
                //{
                //    textoCaptcha = await servicioCaptcha.ObtieneLecturaCaptcha(idCaptcha);
                //    count += 10;
                //    if (count >= tarea.EsperaCaptcha || !string.IsNullOrEmpty(textoCaptcha))
                //    {
                //        timer.Dispose();
                //    }
                //}
                if (!string.IsNullOrEmpty(textoCaptcha))
                {
                   
                    ResultadoHttp resultado = await login.ContinuaLoginCaptcha(textoCaptcha);

                    if (resultado.Ok)
                    {                   
                        resultado = await login.ContinuaLoginCaptchaEmitidos(resultado);


                        if(resultado.Ok && resultado.HttpCode==200)
                        {
                            resultado = await login.ConciliaEmitidos(resultado);

                        }

                        if (resultado.Ok && (resultado.HttpCode == 200 || resultado.HttpCode == 204))
                        {
                            resultado = await login.ContinuaLoginCaptchaRecibidos(resultado);

                            if (resultado.Ok && resultado.HttpCode == 200)
                            {
                                resultado = await login.ConciliaRecibidos(resultado);

                            }
                            if(resultado.Ok)
                            {
                                conciliacion.OK = true;
                            }
                          
                        }
                    }
                    else
                    {
                        conciliacion.Errores.AddRange(resultado.Errores);
                    }    
                
            }
                else
                {
                    conciliacion.Errores.Add("Termino el tiempo de espera para obtener captcha");
                }
            }
            else
            {
                conciliacion.Errores.Add("No se pudo obtener captcha");
            }
            await login.LogOut();
                     
        } else
        {
            conciliacion.Errores.Add("No se obtuvo el acceso a la DB de SQlite");
        }
        conciliacion.FechaConclusionTarea = DateTime.UtcNow;
        return conciliacion;
    }
}