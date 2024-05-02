using procesador.sat.cfdi;
using sat.bot.modelo;
using sat.bot.modelo.conciliacion;
using System.Net;
using System.Text.Json;

namespace sat.bot.captcha;


/// <summary>
/// GEnera el acecso a las aplciaciones dependientes del login SIAT
/// </summary>
public partial class SiatLogin
{

    public const string PARAM_CAPTCHA = "captcha";
    public const string PARAM_ARCHIVO_CAPTCHA = "acaptcha";
    private readonly TareaConciliacion tarea;
    private readonly IProcesadorCFDI _procesador;

    public CookieContainer Cookies { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public SiatLogin(TareaConciliacion tarea, IProcesadorCFDI procesador)
    {
        Cookies = new CookieContainer();
        this.tarea = tarea;
        this._procesador = procesador;
    }

    public async Task<string> LoginCaptcha(string URLDestino)
    {
        ResultadoHttp resultado = new ResultadoHttp () {  Ok = true };
        string img = null;
        resultado = (await resultado.PaginaInicial(URLDestino, Cookies))
           .PostPaginaInicial(Cookies).Result;

        if(resultado.Ok)
        {
            img = resultado.Propiedades[PARAM_CAPTCHA];
            if (img.IndexOf(',') > 0)
            {
                img = img.Split(',')[1];
            }
        } else
        {
            Console.WriteLine("LoginCaptcha Error");
            foreach (var item in resultado.Errores)
            {
                Console.WriteLine(item);
            }
        }
        return img;
    }

    public async Task<ResultadoHttp> ContinuaLoginCaptchaCancelados(ResultadoHttp resultado)
    {
        if(tarea.ConciliarCancelados)
        {
            resultado = await resultado.GetConsultaCancelacion(Cookies);    
            if (resultado.Ok)
            {
                Console.WriteLine("Conciliacion Cancelados  OK");
                resultado.HttpCode = 200;
            }
            else
            {
                Console.WriteLine("ContinuaLoginCaptcha Error");
                foreach (var item in resultado.Errores)
                {
                    Console.WriteLine(item);
                }
            }
        }
        else {
            resultado.Ok = true; 
            resultado.HttpCode = 204; }
        return resultado;
    }

    public async Task<ResultadoHttp> ContinuaLoginCaptchaEmitidos( ResultadoHttp resultado)
    {
        if (tarea.ConciliarEmitidos)
        {
            resultado = await resultado.GetConsultaEmisor(Cookies);

            if (resultado.Ok)
            {
                resultado.HttpCode = 200;
                Console.WriteLine("Consulta default Emitidos OK");
            }
            else
            {
                Console.WriteLine("Consulta default Emitidos Error");
                foreach (var item in resultado.Errores)
                {
                    Console.WriteLine(item);
                }
            }
        }
        else {
            resultado.Ok = true;
            resultado.HttpCode = 204; }
        return resultado;
    }
    public async Task<ResultadoHttp> ConciliaEmitidos(ResultadoHttp resultado)
    {
        var consulta = new ConsultaConciliacion()
        {
            Propiedades = resultado.Propiedades,
            FechaFin = tarea.FechaFinal,
            FechaInicio = tarea.FechaInicio,
        };

       while(consulta.HttpCode!=200)
        {
            await consulta.PostConsultaEmisor(Cookies);
            if (consulta.Ok && consulta.PayLoad.Any())
            {
                foreach (var c in consulta.PayLoad)
                {
                    var emitido = JsonSerializer.Deserialize<CfdiEmitido>(c);
                    await _procesador.guardarCfdiUiEmitido(emitido);

                    if (tarea.DescargarXML)
                    {
                        await emitido.GuardarXMLEmitido(await emitido.UrlDescarga.GetDescargaCFDI(Cookies));
                        await _procesador.ProcesarEmitido(emitido);
                    }
                };

            }
            await _procesador.SaveChanges();
            if (tarea.FechaFinal - consulta.FechaFin > TimeSpan.FromMilliseconds(1))
            {
                consulta.FechaInicio = consulta.FechaFin;
                consulta.FechaFin = tarea.FechaFinal;
            }
            else
            {
                consulta.HttpCode = 200;
            }
        }      


            if (consulta.HttpCode==200)
            {
                    resultado.Ok = true;
                    resultado.HttpCode = 200;           
                Console.WriteLine("Consulta Emitidos OK");
            }
            else
            {
                Console.WriteLine("Consulta Emitidos Error");
                foreach (var item in resultado.Errores)
                {
                    Console.WriteLine(item);
                }
            }  
        return resultado;
    }



    public async Task<ResultadoHttp> ContinuaLoginCaptchaRecibidos(ResultadoHttp resultado)
    {

        if (tarea.ConciliarRecibidos)
        {
            resultado = await resultado.GetConsultaReceptor(Cookies);
            if (resultado.Ok)
            {
                resultado.HttpCode = 200;
                Console.WriteLine("Consulta default Recibidos OK");
            }
            else
            {
                Console.WriteLine("Consulta default Recibidos Error");
                foreach (var item in resultado.Errores)
                {
                    Console.WriteLine(item);
                }
            }
        }
        else
        {
            resultado.Ok = true;
            resultado.HttpCode = 204;
        }
        return resultado;


    }

    public async Task<ResultadoHttp> ConciliaRecibidos(ResultadoHttp resultado)
    {
        var consulta = new ConsultaConciliacion()
        {
            Propiedades = resultado.Propiedades,
            FechaFin = tarea.FechaFinal,
            FechaInicio = tarea.FechaInicio,
        };


        while (consulta.HttpCode != 200)
        {
            if ((consulta.FechaFin - consulta.FechaInicio) < TimeSpan.FromHours(24))
            {
                await consulta.PostConsultaReceptor(Cookies);
            }
            else
            {
                consulta.FechaFin = consulta.FechaInicio.AddSeconds(86399);
                await consulta.PostConsultaReceptor(Cookies);
            }

            if (consulta.Ok && consulta.PayLoad.Any())
            {

                foreach (var c in consulta.PayLoad)
                {
                    var recibido = JsonSerializer.Deserialize<CfdiRecibido>(c);
                    await _procesador.guardarCfdiUiRecibido(recibido);

                    if (tarea.DescargarXML)
                    {
                        var xml = await recibido.UrlDescarga.GetDescargaCFDI(Cookies);

                        if (xml != null)
                        {
                            await recibido.GuardarXMLRecibido(xml);
                            await _procesador.ProcesarRecibido(recibido);
                        }

                    }

                }

            }
            await _procesador.SaveChanges();
            if (tarea.FechaFinal - consulta.FechaFin > TimeSpan.FromMilliseconds(1))
            {
                consulta.FechaInicio = consulta.FechaFin+TimeSpan.FromSeconds(1);
                consulta.FechaFin = tarea.FechaFinal;
            }
            else
            {
                consulta.HttpCode = 200;
            }

        }


        if (consulta.HttpCode == 200)
        {
            resultado.Ok = true;
            resultado.HttpCode = 200;
            Console.WriteLine("Consulta Emitidos OK");
        }
        else
        {
            Console.WriteLine("Consulta Emitidos Error");
            foreach (var item in resultado.Errores)
            {
                Console.WriteLine(item);
            }
        }
        return resultado;
    }


        public async Task<ResultadoHttp> ContinuaLoginCaptcha(string Captcha)
    {
        ResultadoHttp resultado = new ResultadoHttp { Ok = true };
        resultado = (await resultado.LoginConCaptcha(Cookies, tarea.RFC, tarea.ContrasenaRFC, Captcha))
   .GetNdipWsfed(Cookies).Result.PostHome(Cookies).Result;
        if (resultado.Ok)
        {
            Console.WriteLine("Login Exitoso");
        }
        else
        {
            Console.WriteLine("Login Error");
            foreach (var item in resultado.Errores)
            {
                Console.WriteLine(item);
            }
        }
        return resultado;
    }
    public async Task LogOut()
    { 
        await ExtensionesLogin.LogOut(Cookies);
    }

}
