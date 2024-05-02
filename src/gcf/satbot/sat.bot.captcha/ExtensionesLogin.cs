using HtmlAgilityPack;
using modelo.repositorio.cfdi.busqueda;
using sat.bot.modelo;
using sat.bot.modelo.conciliacion;
using System;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Reflection;
using System.Runtime.CompilerServices;
//using System.Runtime.InteropServices.JavaScript;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web;
namespace sat.bot.captcha;

public static class ExtensionesLogin
{
    /// <summary>
    /// 001 GET inisial para obtener las cookies de la sesión
    /// </summary>
    /// <param name="resultado"></param>
    /// <param name="URLDestino"></param>
    /// <param name="Cookies"></param>
    /// <param name="HttpCodigoEsperado"></param>
    /// <returns></returns>
    public async static Task<ResultadoHttp> PaginaInicial(this ResultadoHttp resultado, string URLDestino, CookieContainer Cookies)
    {
        Console.WriteLine("Ejecuando PaginaInicial");

        if (!resultado.Ok)
        {
            return resultado;
        }

        string url = HttpUtility.UrlEncode(URLDestino);
        string wct = HttpUtility.UrlEncode(ToUniversalIso8601(DateTime.Now));
        var URL = $"https://cfdiau.sat.gob.mx/nidp/wsfed_portalCFDI.jsp?wa=wsignin1.0&wtrealm={url}&wctx=rm%3d0%26id%3dpassive%26ru%3d%252f&wct={wct}&wreply={url}";
    https://cfdiau.sat.gob.mx/nidp/wsfed_portalCFDI.jsp?wa=wsignin1.0&wtrealm=https%3a%2f%2fportalcfdi.facturaelectronica.sat.gob.mx&wctx=rm%3d0%26id%3dpassive%26ru%3d%252f&wct=2023-11-27T22%3a26%3a10Z&wreply=https%3a%2f%2fportalcfdi.facturaelectronica.sat.gob.mx%2f
        try
        {
            using var handler = new HttpClientHandler() { CookieContainer = Cookies, AllowAutoRedirect = false, AutomaticDecompression = DecompressionMethods.All };
            using var client = new HttpClient(handler);

            client.ClearHeaders().WithTimeOut(5).WithKeepAlive().MockBrowserHeaders().AccepTextHTML();

            var result = await client.GetAsync(new Uri(URL));

            if (result.StatusCode == HttpStatusCode.OK)
            {
                resultado.Ok = true;
                await HumanDelay();
                // las cookies son almacenadas automaticamente
            }
        }
        catch (Exception ex)
        {
            resultado.Ok = false;
            resultado.Errores.Add(ex.Message);
            resultado.Exceptions.Add(ex);
        }
        return resultado;
    }

    /// <summary>
    /// POst inicial para obtner el captcha
    /// </summary>
    /// <param name="resultado"></param>
    /// <param name="URLDestino"></param>
    /// <param name="Cookies"></param>
    /// <param name="HttpCodigoEsperado"></param>
    /// <returns></returns>
    public async static Task<ResultadoHttp> PostPaginaInicial(this ResultadoHttp resultado, CookieContainer Cookies)
    {

        if (!resultado.Ok)
        {
            Console.WriteLine("No ejecutado PaginaInicial");
            return resultado;
        }

        Console.WriteLine("Ejecuando PostPaginaInicial");
        var URL = $"https://cfdiau.sat.gob.mx/nidp/wsfed/ep?id=SATUPCFDiCon&sid=0&option=credential&sid=0";

        try
        {
            using var handler = new HttpClientHandler() { CookieContainer = Cookies, AllowAutoRedirect = false, AutomaticDecompression = DecompressionMethods.All };
            using var client = new HttpClient(handler);

            client.ClearHeaders().WithTimeOut(5).WithKeepAlive().MockBrowserHeaders().AccepTextHTML();

            var result = await client.GetAsync(new Uri(URL));

            if (result.StatusCode == HttpStatusCode.OK)
            {
                string contenido = await result.Content.ReadAsStringAsync();
                var htmlDoc = new HtmlDocument();
                htmlDoc.LoadHtml(contenido);

                var referenceImgs = htmlDoc.DocumentNode
                           .Descendants("img");

                foreach (var i in referenceImgs)
                {
                    if (!string.IsNullOrEmpty(i.Attributes["src"]?.Value))
                    {
                        resultado.Propiedades.Add(SiatLogin.PARAM_CAPTCHA, i.Attributes["src"]!.Value);
                        resultado.Ok = true;
                        await HumanDelay();
                    }
                }
            }
        }
        catch (Exception ex)
        {
            resultado.Ok = false;
            resultado.Errores.Add(ex.Message);
            resultado.Exceptions.Add(ex);
        }
        return resultado;
    }

    public async static Task<ResultadoHttp> GetConsultaCancelacion(this ResultadoHttp resultado, CookieContainer Cookies)
    {

        if (!resultado.Ok)
        {
            Console.WriteLine("No ejecutado Get Consulta");
            return resultado;
        }

        Console.WriteLine("Ejecuando Get Consulta");
        var URL = $"https://portalcfdi.facturaelectronica.sat.gob.mx/ConsultaCancelacion.aspx";
        try
        {
            using var handler = new HttpClientHandler() { CookieContainer = Cookies, AllowAutoRedirect = false, AutomaticDecompression = DecompressionMethods.All };
            using var client = new HttpClient(handler);

            client.ClearHeaders().WithTimeOut(5).WithKeepAlive().MockBrowserHeaders().AccepTextHTML();

            var result = await client.GetAsync(new Uri(URL));

            if (result.StatusCode == HttpStatusCode.OK)
            {
                string contenido = await result.Content.ReadAsStringAsync();
                var htmlDoc = new HtmlDocument();
                htmlDoc.LoadHtml(contenido);

                var referenceBody = htmlDoc.DocumentNode.SelectSingleNode("//body");

                var Cancelaciones = referenceBody.GetCfdiCancelados();


            }
        }
        catch (Exception ex)
        {
            resultado.Ok = false;
            resultado.Errores.Add(ex.Message);
            resultado.Exceptions.Add(ex);
        }
        return resultado;
    }
    public async static Task<ResultadoHttp> GetConsultaEmisor(this ResultadoHttp resultado, CookieContainer Cookies)
    {        
        if (!resultado.Ok)
        {
            Console.WriteLine("No ejecutado Get ConsultaEmisor");
            return resultado;
        }
        Console.WriteLine("Ejecuando Get ConsultaEmisor");
        var URL = $"https://portalcfdi.facturaelectronica.sat.gob.mx/ConsultaEmisor.aspx";
        try
        {
            using var handler = new HttpClientHandler() { CookieContainer = Cookies, AllowAutoRedirect = false, AutomaticDecompression = DecompressionMethods.All };
            using var client = new HttpClient(handler);

            client.ClearHeaders().WithTimeOut(5).WithKeepAlive().MockBrowserHeaders().AccepTextHTML();

            var result = await client.GetAsync(new Uri(URL));

            if (result.StatusCode == HttpStatusCode.OK)
            {
 //               await resultado.VerificaSesion(Cookies);
                string contenido = await result.Content.ReadAsStringAsync();
                var htmlDoc = new HtmlDocument();
                htmlDoc.LoadHtml(contenido);

                var html = htmlDoc.DocumentNode.SelectSingleNode("//html");


                Dictionary<string, string> dict = new()
                    {

                        {"ctl00$ScriptManager1","ctl00$MainContent$UpnlBusqueda|ctl00$MainContent$RdoFechas"},
                        {"__CSRFTOKEN",html.SelectSingleNode("body/form/div[@class='aspNetHidden']/input[@name='__CSRFTOKEN']").GetAttributeValue("value","")},
                        {"__EVENTTARGET","ctl00$MainContent$RdoFechas"},
                        {"__EVENTARGUMENT",""},
                        {"__LASTFOCUS",""},
                        {"__VIEWSTATE",html.SelectSingleNode("body/form/div[@class='aspNetHidden']/input[@name='__VIEWSTATE']").GetAttributeValue("value","")},
                        {"__VIEWSTATEGENERATOR",html.SelectSingleNode("body/form/div[@class='aspNetHidden']/input[@name='__VIEWSTATEGENERATOR']").GetAttributeValue("value","")},
                        {"__VIEWSTATEENCRYPTED",html.SelectSingleNode("body/form/div[@class='aspNetHidden']/input[@name='__VIEWSTATEENCRYPTED']").GetAttributeValue("value","")},
                        {"ctl00$MainContent$TxtUUID",""},
                        {"ctl00$MainContent$FiltroCentral","RdoFechas"},
                        {"ctl00$MainContent$hfInicialBool","true"},
                        {"ctl00$MainContent$hfInicial",""},
                        {"ctl00$MainContent$CldFechaInicial2$Calendario_text",""},
                        {"ctl00$MainContent$CldFechaInicial2$DdlHora",$"0"},
                        {"ctl00$MainContent$CldFechaInicial2$DdlMinuto",$"0"},
                        {"ctl00$MainContent$CldFechaInicial2$DdlSegundo",$"0"},
                        {"ctl00$MainContent$hfFinal",""},
                        {"ctl00$MainContent$CldFechaFinal2$Calendario_text",""},
                        {"ctl00$MainContent$CldFechaFinal2$DdlHora",$"0"},
                        {"ctl00$MainContent$CldFechaFinal2$DdlMinuto",$"0"},
                        {"ctl00$MainContent$CldFechaFinal2$DdlSegundo",$"0"},
                        {"ctl00$MainContent$TxtRfcTercero",""},
                        {"ctl00$MainContent$DdlEstadoComprobante","-1"},
                        {"ctl00$MainContent$ddlComplementos","-1"},
                        {"ctl00$MainContent$TxtRfcReceptor",""},
                        {"ctl00$MainContent$ddlVigente","0"},
                        {"ctl00$MainContent$ddlCancelado","0"},
                        {"ctl00$MainContent$hfDatos",""},
                        {"ctl00$MainContent$hfFlag",""},
                        {"ctl00$MainContent$hfAux",""},
                        {"ctl00$MainContent$hfDescarga",""},
                        {"ctl00$MainContent$hfCancelacion",""},
                        {"ctl00$MainContent$hfJSONCancelacion",""},
                        {"ctl00$MainContent$hfUrlDescarga",""},
                        {"ctl00$MainContent$hfParametrosMetadata",""},
                        {"ctl00$MainContent$hdnValAccion",""},
                        {"ctl00$MainContent$hfXML",""},
                        {"__ASYNCPOST","true"},                              
                     };
                resultado.Propiedades = dict;

            }
        }
        catch (Exception ex)
        {
            resultado.Ok = false;
            resultado.Errores.Add(ex.Message);
            resultado.Exceptions.Add(ex);
        }


        var resultadoPayload = await resultado.PostConsultaEmisorDefault(Cookies);
        if (resultadoPayload.Ok)
        {
            resultadoPayload.Propiedades["__CSRFTOKEN"] = resultado.Propiedades["__CSRFTOKEN"];
            resultadoPayload.Propiedades["__VIEWSTATEGENERATOR"] = resultado.Propiedades["__VIEWSTATEGENERATOR"];
            resultadoPayload.Propiedades["__VIEWSTATEENCRYPTED"] = resultado.Propiedades["__VIEWSTATEENCRYPTED"];
         
        }
        return resultadoPayload;
    }
    public async static Task<ResultadoHttp> GetConsultaReceptor(this ResultadoHttp resultado, CookieContainer Cookies)
    {
        if (!resultado.Ok)
        {
            Console.WriteLine("No ejecutado Get ConsultaReceptor");
            return resultado;
        }

        Console.WriteLine("Ejecuando Consulta Default Recibidos");
        var URL = $"https://portalcfdi.facturaelectronica.sat.gob.mx/ConsultaReceptor.aspx";
        try
        {
            using var handler = new HttpClientHandler() { CookieContainer = Cookies, AllowAutoRedirect = false, AutomaticDecompression = DecompressionMethods.All };
            using var client = new HttpClient(handler);

            client.ClearHeaders().WithTimeOut(5).WithKeepAlive().MockBrowserHeaders().AccepTextHTML();

            var result = await client.GetAsync(new Uri(URL));

            if (result.StatusCode == HttpStatusCode.OK)
            {
                string contenido = await result.Content.ReadAsStringAsync();
                var htmlDoc = new HtmlDocument();
                htmlDoc.LoadHtml(contenido);

                var html = htmlDoc.DocumentNode.SelectSingleNode("//html");
                string dia = "";
                Dictionary<string, string> dict = new()
                    {

                        {"ctl00$ScriptManager1","ctl00$MainContent$UpnlBusqueda|ctl00$MainContent$RdoFechas"},
                        {"__CSRFTOKEN",html.SelectSingleNode("body/form/div[@class='aspNetHidden']/input[@name='__CSRFTOKEN']").GetAttributeValue("value","")},
                        {"ctl00$MainContent$TxtUUID",""},
                        {"ctl00$MainContent$FiltroCentral","RdoFechas"},
                        {"ctl00$MainContent$CldFecha$DdlAnio",$"{DateTime.UtcNow.Year}" },
                        {"ctl00$MainContent$CldFecha$DdlMes",$"1" },
                        {"ctl00$MainContent$CldFecha$DdlDia","0"},
                        {"ctl00$MainContent$CldFecha$DdlHora",$"0"},
                        {"ctl00$MainContent$CldFecha$DdlMinuto",$"0"},
                        {"ctl00$MainContent$CldFecha$DdlSegundo",$"0"},
                        {"ctl00$MainContent$CldFecha$DdlHoraFin",$"23"},
                        {"ctl00$MainContent$CldFecha$DdlMinutoFin",$"59"},
                        {"ctl00$MainContent$CldFecha$DdlSegundoFin",$"59"},
                        {"ctl00$MainContent$TxtRfcReceptor",""},
                        {"ctl00$MainContent$TxtRfcTercero",""},
                        {"ctl00$MainContent$DdlEstadoComprobante","-1"},
                        {"ctl00$MainContent$hfInicialBool","true"},
                        {"ctl00$MainContent$hfDescarga",""},
                        {"ctl00$MainContent$ddlComplementos","-1"},
                        {"ctl00$MainContent$ddlVigente","0"},
                        {"ctl00$MainContent$ddlCancelado","0"},
                        {"ctl00$MainContent$hfParametrosMetadata",""},
                        {"__EVENTTARGET","ctl00$MainContent$RdoFechas"},
                        {"__EVENTARGUMENT",""},
                        {"__LASTFOCUS",""},
                        {"__VIEWSTATE",html.SelectSingleNode("body/form/div[@class='aspNetHidden']/input[@name='__VIEWSTATE']").GetAttributeValue("value","")},
                        {"__VIEWSTATEGENERATOR",html.SelectSingleNode("body/form/div[@class='aspNetHidden']/input[@name='__VIEWSTATEGENERATOR']").GetAttributeValue("value","")},
                        {"__VIEWSTATEENCRYPTED",html.SelectSingleNode("body/form/div[@class='aspNetHidden']/input[@name='__VIEWSTATEENCRYPTED']").GetAttributeValue("value","")},                    
                        {"__ASYNCPOST","true"},
                     };
                resultado.Propiedades = dict;
            }
        }
        catch (Exception ex)
        {
            resultado.Ok = false;
            resultado.Errores.Add(ex.Message);
            resultado.Exceptions.Add(ex);
        }

        var resultadoPayload = await resultado.PostConsultaReceptorDefault(Cookies);
        if (resultadoPayload.Ok)
        {
            resultadoPayload.Propiedades["__CSRFTOKEN"] = resultado.Propiedades["__CSRFTOKEN"];
            resultadoPayload.Propiedades["__VIEWSTATEGENERATOR"] = resultado.Propiedades["__VIEWSTATEGENERATOR"];
            resultadoPayload.Propiedades["__VIEWSTATEENCRYPTED"] = resultado.Propiedades["__VIEWSTATEENCRYPTED"];
        }
        return resultadoPayload;


    }
    public async static Task<ResultadoHttp> GetNdipWsfed(this ResultadoHttp resultado, CookieContainer Cookies)
    {
        if (!resultado.Ok)
        {
            Console.WriteLine("No ejecutado Getndpi");
            return resultado;
        }

        Console.WriteLine("Ejecuando Getndpi");
        var URL = $"https://cfdiau.sat.gob.mx/nidp/wsfed/ep?sid=0";
        try
        {
            using var handler = new HttpClientHandler() { CookieContainer = Cookies, AllowAutoRedirect = false, AutomaticDecompression = DecompressionMethods.All };
            using var client = new HttpClient(handler);

            client.ClearHeaders().WithTimeOut(5).WithKeepAlive().MockBrowserHeaders().AccepTextHTML();

            var result = await client.GetAsync(new Uri(URL));

            if (result.StatusCode == HttpStatusCode.OK)
            {
                string contenido = await result.Content.ReadAsStringAsync();
                var htmlDoc = new HtmlDocument();
                htmlDoc.LoadHtml(contenido);

                var referenceForm = htmlDoc.DocumentNode.SelectSingleNode("//body");
                if (referenceForm!=null)
                { 
                Dictionary<string, string> dict = new()
                    {
                        {"wa",referenceForm.SelectSingleNode("form[@method='POST']/input[@name='wa']").GetAttributeValue("value","")},
                        { "wresult",HttpUtility.HtmlDecode(referenceForm.SelectSingleNode("form[@method='POST']/input[@name='wresult']").GetAttributeValue("value", ""))},
                        {"wctx",referenceForm.SelectSingleNode("form[@method='POST']/input[@name='wctx']").GetAttributeValue("value","")},
                    };

                resultado.Propiedades = (dict);
                }
                else                
                {
                    resultado.Ok = false;
                    resultado.Errores.Add("Error en el HTML");
                }
            }
        }
        catch (Exception ex)
        {
            resultado.Ok = false;
            resultado.Errores.Add(ex.Message);
            resultado.Exceptions.Add(ex);
        }
        return resultado;
    }


    public async static Task<ResultadoHttp> PostHome(this ResultadoHttp resultado, CookieContainer Cookies)
    {

        if (!resultado.Ok)
        {
            Console.WriteLine("No ejecutado POST home");
            return resultado;
        }

        Console.WriteLine("Ejecuando PostHome");
        var URL = $"https://portalcfdi.facturaelectronica.sat.gob.mx/";

        try
        {
            using var handler = new HttpClientHandler() { CookieContainer = Cookies, AllowAutoRedirect = false, AutomaticDecompression = DecompressionMethods.All };
            using var client = new HttpClient(handler);

            client.ClearHeaders().WithTimeOut(5).WithKeepAlive().MockBrowserHeaders().AccepTextHTML()
                .WithReferer("https://portalcfdi.facturaelectronica.sat.gob.mx/");

            var result = await client.PostAsync(new Uri(URL), new FormUrlEncodedContent(resultado.Propiedades));


            if (result.StatusCode == HttpStatusCode.OK)
            {
                string contenido = await result.Content.ReadAsStringAsync();
                var htmlDoc = new HtmlDocument();
                htmlDoc.LoadHtml(contenido);
                
                var referencehtml = htmlDoc.DocumentNode.SelectSingleNode("//html/head").InnerText;

                if (referencehtml != null && referencehtml.IndexOf("Portal Contribuyentes CFDI | Buscar CFDI") > 0)
                {
                    resultado.Ok = true;
                    await HumanDelay();

                }
                else
                {
                    resultado.Ok = false;
                    resultado.Errores.Add("Contenido no encontrado");
                }
            }
        }
        catch (Exception ex)
        {
            resultado.Ok = false;
            resultado.Errores.Add(ex.Message);
            resultado.Exceptions.Add(ex);
        }
        return resultado;
    }
    public async static Task<ConsultaConciliacion> PostConsultaEmisor(this ConsultaConciliacion consulta, CookieContainer Cookies)
    {

        consulta.Propiedades["ctl00$MainContent$CldFechaInicial2$DdlSegundo"] = $"{consulta.FechaInicio.Second}";
        consulta.Propiedades["ctl00$MainContent$CldFechaInicial2$DdlMinuto"] = $"{consulta.FechaInicio.Minute}";
        consulta.Propiedades["ctl00$MainContent$CldFechaInicial2$DdlHora"] = $"{consulta.FechaInicio.Hour}";
        consulta.Propiedades["ctl00$MainContent$CldFechaInicial2$Calendario_text"] = consulta.FechaInicio.ToString("dd/MM/yyyy");
        consulta.Propiedades["ctl00$MainContent$CldFechaFinal2$DdlSegundo"] = $"{consulta.FechaFin.Second}";
        consulta.Propiedades["ctl00$MainContent$CldFechaFinal2$DdlMinuto"] = $"{consulta.FechaFin.Minute}";
        consulta.Propiedades["ctl00$MainContent$CldFechaFinal2$DdlHora"] = $"{consulta.FechaFin.Hour}";
        consulta.Propiedades["ctl00$MainContent$CldFechaFinal2$Calendario_text"] = consulta.FechaFin.ToString("dd/MM/yyyy");
        var diferencia = consulta.FechaFin -consulta.FechaInicio;
        Console.WriteLine($"Ejecuando Consulta:{consulta.FechaInicio}-{consulta.FechaFin}");
        var URL = $"https://portalcfdi.facturaelectronica.sat.gob.mx/ConsultaEmisor.aspx";

        try
        {
            using var handler = new HttpClientHandler() { CookieContainer = Cookies, AllowAutoRedirect = false, AutomaticDecompression = DecompressionMethods.All};
            using var client = new HttpClient(handler);
            client.DefaultRequestHeaders.Add("X-MicrosoftAjax", "Delta=true");
            client.ClearHeaders().WithTimeOut(5).WithKeepAlive().MockBrowserHeaders().AccepTextHTML()
                .WithReferer("https://portalcfdi.facturaelectronica.sat.gob.mx/ConsultaEmisor.aspx");

            var result = await client.PostAsync(new Uri(URL), new FormUrlEncodedContent(consulta.Propiedades));

            if (result.StatusCode == HttpStatusCode.OK)
            {
                string contenido = await result.Content.ReadAsStringAsync();
                var htmlDoc = new HtmlDocument();
                htmlDoc.LoadHtml(contenido);
                consulta.Propiedades["__VIEWSTATE"]=contenido.Split("__VIEWSTATE|")[1].Split("|")[0];
                var referenceBody = htmlDoc.DocumentNode;
                var limite = htmlDoc.DocumentNode.SelectSingleNode("div[@id='ctl00_MainContent_PnlResultados']/div[@id='ctl00_MainContent_PnlLimiteRegistros']/h2");
                if (limite==null)
                {
                    consulta.Ok = true;
                    return await consulta.GetCfdiEmitidos(Cookies,referenceBody); 
                }
                else
                    {
                    consulta.FechaFin = consulta.FechaInicio + (diferencia/2);
                    await consulta.PostConsultaEmisor(Cookies);                     
                    }              
                }
        }
        catch (Exception ex)
        {
            consulta.Ok = false;
            consulta.Errores.Add(ex.Message);
        }
        return consulta;
    }


    public async static Task<ResultadoHttp> PostConsultaReceptorDefault(this ResultadoHttp resultado, CookieContainer Cookies)
    {
        Console.WriteLine("Ejecuando postConsultaReceptorDefault");
        var URL = $"https://portalcfdi.facturaelectronica.sat.gob.mx/ConsultaReceptor.aspx";

        try
        {
            using var handler = new HttpClientHandler() { CookieContainer = Cookies, AllowAutoRedirect = false, AutomaticDecompression = DecompressionMethods.All };
            using var client = new HttpClient(handler);
            client.DefaultRequestHeaders.Add("X-MicrosoftAjax", "Delta=true");
            client.ClearHeaders().WithTimeOut(5).WithKeepAlive().MockBrowserHeaders().AccepTextHTML()
                .WithReferer("https://portalcfdi.facturaelectronica.sat.gob.mx/ConsultaReceptor.aspx");

            var result = await client.PostAsync(new Uri(URL), new FormUrlEncodedContent(resultado.Propiedades));
            if (result.StatusCode == HttpStatusCode.OK)
            {
             
                string contenido = await result.Content.ReadAsStringAsync();
                var htmlDoc = new HtmlDocument();
                htmlDoc.LoadHtml(contenido);
                var html = htmlDoc.DocumentNode.SelectSingleNode("//html");
                Dictionary<string, string> dict = new()
                    {

                        {"ctl00$ScriptManager1","ctl00$MainContent$UpnlBusqueda|ctl00$MainContent$BtnBusqueda"},
                        {"__CSRFTOKEN",""},
                        {"ctl00$MainContent$TxtUUID",""},
                        {"ctl00$MainContent$FiltroCentral","RdoFechas"},
                        {"ctl00$MainContent$CldFecha$DdlAnio","0" },
                        {"ctl00$MainContent$CldFecha$DdlMes","0" },
                        {"ctl00$MainContent$CldFecha$DdlDia","0"},
                        {"ctl00$MainContent$CldFecha$DdlHora","0"},
                        {"ctl00$MainContent$CldFecha$DdlMinuto","0"},
                        {"ctl00$MainContent$CldFecha$DdlSegundo","0"},
                        {"ctl00$MainContent$CldFecha$DdlHoraFin","0"},
                        {"ctl00$MainContent$CldFecha$DdlMinutoFin","0"},
                        {"ctl00$MainContent$CldFecha$DdlSegundoFin","0"},
                        {"ctl00$MainContent$TxtRfcReceptor",""},
                        {"ctl00$MainContent$TxtRfcTercero",""},
                        {"ctl00$MainContent$DdlEstadoComprobante","-1"},
                        {"ctl00$MainContent$hfInicialBool","false"},
                        {"ctl00$MainContent$hfDescarga",""},
                        {"ctl00$MainContent$ddlComplementos","-1"},
                        {"ctl00$MainContent$ddlVigente","0"},
                        {"ctl00$MainContent$ddlCancelado","0"},
                        {"ctl00$MainContent$hfParametrosMetadata",""},
                        {"__EVENTTARGET",""},
                        {"__EVENTARGUMENT",""},
                        {"__LASTFOCUS",""},
                        {"__VIEWSTATE", contenido.Split("__VIEWSTATE|")[1].Split("|")[0]},
                        {"__VIEWSTATEGENERATOR",""},
                        {"__VIEWSTATEENCRYPTED",""},
                        {"__ASYNCPOST","true"},
                        {"ctl00$MainContent$BtnBusqueda","Buscar CFDI" },
                     };
                resultado.Propiedades = dict;

            }
        }
        catch (Exception ex)
        {
            resultado.Ok = false;
            resultado.Errores.Add(ex.Message);
            resultado.Exceptions.Add(ex);
        }
        return resultado;
    }

    public async static Task<ResultadoHttp> PostConsultaEmisorDefault(this ResultadoHttp resultado, CookieContainer Cookies)
    {
        Console.WriteLine("Ejecuando postConsultaReceptorDefault");
        var URL = $"https://portalcfdi.facturaelectronica.sat.gob.mx/ConsultaEmisor.aspx";

        try
        {
            using var handler = new HttpClientHandler() { CookieContainer = Cookies, AllowAutoRedirect = false, AutomaticDecompression = DecompressionMethods.All };
            using var client = new HttpClient(handler);
            client.DefaultRequestHeaders.Add("X-MicrosoftAjax", "Delta=true");
            client.ClearHeaders().WithTimeOut(5).WithKeepAlive().MockBrowserHeaders().AccepTextHTML()
                .WithReferer("https://portalcfdi.facturaelectronica.sat.gob.mx/ConsultaEmisor.aspx");

            var result = await client.PostAsync(new Uri(URL), new FormUrlEncodedContent(resultado.Propiedades));
            if (result.StatusCode == HttpStatusCode.OK)
            {

                string contenido = await result.Content.ReadAsStringAsync();
                var htmlDoc = new HtmlDocument();
                htmlDoc.LoadHtml(contenido);
                var html = htmlDoc.DocumentNode.SelectSingleNode("//html");
                Dictionary<string, string> dict = new()
                    {

                        {"ctl00$ScriptManager1","ctl00$MainContent$UpnlBusqueda|ctl00$MainContent$BtnBusqueda"},
                        {"__CSRFTOKEN","" },
                        {"ctl00$MainContent$TxtUUID",""},
                        {"ctl00$MainContent$FiltroCentral","RdoFechas"},
                        {"ctl00$MainContent$hfInicialBool","false"},
                        {"ctl00$MainContent$hfInicial",$"{DateTime.UtcNow.Year}"},
                        {"ctl00$MainContent$CldFechaInicial2$Calendario_text",""},
                        {"ctl00$MainContent$CldFechaInicial2$DdlHora",$"0"},
                        {"ctl00$MainContent$CldFechaInicial2$DdlMinuto",$"0"},
                        {"ctl00$MainContent$CldFechaInicial2$DdlSegundo",$"0"},
                        {"ctl00$MainContent$hfFinal",""},
                        {"ctl00$MainContent$CldFechaFinal2$Calendario_text",""},
                        {"ctl00$MainContent$CldFechaFinal2$DdlHora",$"0"},
                        {"ctl00$MainContent$CldFechaFinal2$DdlMinuto",$"0"},
                        {"ctl00$MainContent$CldFechaFinal2$DdlSegundo",$"0"},
                        {"ctl00$MainContent$TxtRfcReceptor",""},
                        {"ctl00$MainContent$TxtRfcTercero",""},
                        {"ctl00$MainContent$DdlEstadoComprobante","-1"},
                        {"ctl00$MainContent$ddlComplementos","-1"},
                        {"ctl00$MainContent$ddlVigente","0"},
                        {"ctl00$MainContent$ddlCancelado","0"},
                        {"ctl00$MainContent$hfDatos",""},
                        {"ctl00$MainContent$hfFlag",""},
                        {"ctl00$MainContent$hfAux",""},
                        {"ctl00$MainContent$hfDescarga",""},
                        {"ctl00$MainContent$hfCancelacion",""},
                        {"ctl00$MainContent$hfJSONCancelacion",""},
                        {"ctl00$MainContent$hfUrlDescarga",""},
                        {"ctl00$MainContent$hfParametrosMetadata",""},
                        {"ctl00$MainContent$hdnValAccion",""},
                        {"ctl00$MainContent$hfXML",""},
                        {"__EVENTTARGET",""},
                        {"__EVENTARGUMENT",""},
                        {"__LASTFOCUS",""},
                        {"__VIEWSTATE",contenido.Split("__VIEWSTATE|")[1].Split("|")[0]},
                        {"__VIEWSTATEGENERATOR",""},
                        {"__VIEWSTATEENCRYPTED",""},
                        {"__ASYNCPOST","true"},
                        {"ctl00$MainContent$BtnBusqueda","Buscar CFDI"},
                     };
                resultado.Propiedades = dict;

            }
        }
        catch (Exception ex)
        {
            resultado.Ok = false;
            resultado.Errores.Add(ex.Message);
            resultado.Exceptions.Add(ex);
        }
        return resultado;
    }

    public async static Task<ConsultaConciliacion> PostConsultaReceptor(this ConsultaConciliacion consulta, CookieContainer Cookies)
    {
        string dia = "";

        if (consulta.FechaInicio.Day < 10)
        {
            dia = $"0{consulta.FechaInicio.Day}";
        }
        else
        {
            dia = consulta.FechaInicio.Day.ToString();
        }
        if (!consulta.Ok)
        {
            Console.WriteLine("No ejecutado Consulta Recibidos");
        }
        var diferencia = consulta.FechaFin - consulta.FechaInicio;
        consulta.Propiedades["ctl00$MainContent$CldFecha$DdlAnio"] = $"{consulta.FechaInicio.Year}";
        consulta.Propiedades["ctl00$MainContent$CldFecha$DdlMes"] = $"{consulta.FechaInicio.Month}";
        consulta.Propiedades["ctl00$MainContent$CldFecha$DdlDia"] = dia;
        consulta.Propiedades["ctl00$MainContent$CldFecha$DdlSegundo"] = $"{consulta.FechaInicio.Second}";
        consulta.Propiedades["ctl00$MainContent$CldFecha$DdlMinuto"] = $"{consulta.FechaInicio.Minute}";
        consulta.Propiedades["ctl00$MainContent$CldFecha$DdlHora"] = $"{consulta.FechaInicio.Hour}";
        consulta.Propiedades["ctl00$MainContent$CldFecha$DdlSegundoFin"] = $"{diferencia.Seconds}";
        consulta.Propiedades["ctl00$MainContent$CldFecha$DdlMinutoFin"] = $"{diferencia.Minutes}";
        consulta.Propiedades["ctl00$MainContent$CldFecha$DdlHoraFin"] = $"{diferencia.Hours}";

        Console.WriteLine($"Ejecuando Consulta:{consulta.FechaInicio}-{consulta.FechaFin}");
        var URL = $"https://portalcfdi.facturaelectronica.sat.gob.mx/ConsultaReceptor.aspx";

        try
        {
            using var handler = new HttpClientHandler() { CookieContainer = Cookies, AllowAutoRedirect = false, AutomaticDecompression = DecompressionMethods.All };
            using var client = new HttpClient(handler);
            client.DefaultRequestHeaders.Add("X-MicrosoftAjax", "Delta=true");
            client.ClearHeaders().WithTimeOut(5).WithKeepAlive().MockBrowserHeaders().AccepTextHTML()
                .WithReferer("https://portalcfdi.facturaelectronica.sat.gob.mx/ConsultaReceptor.aspx");

            var result = await client.PostAsync(new Uri(URL), new FormUrlEncodedContent(consulta.Propiedades));

            if (result.StatusCode == HttpStatusCode.OK)
            {
                string contenido = await result.Content.ReadAsStringAsync();
                var htmlDoc = new HtmlDocument();
                htmlDoc.LoadHtml(contenido);

                var referenceBody = htmlDoc.DocumentNode;
                consulta.Propiedades["__VIEWSTATE"] = contenido.Split("__VIEWSTATE|")[1].Split("|")[0];      
                var limite = htmlDoc.DocumentNode.SelectSingleNode("div[@id='ctl00_MainContent_PnlResultados']/div[@id='ctl00_MainContent_PnlLimiteRegistros']/h2");
                if (limite == null)
                {
                    consulta.Ok = true;
      
                    return await consulta.GetCfdiRecibidos(Cookies, referenceBody);
                }
                else
                {
                    consulta.FechaFin = consulta.FechaInicio + (diferencia / 2);
                    await consulta.PostConsultaReceptor(Cookies);
                }
            }
        }
        catch (Exception ex)
        {
            consulta.Ok = false;
            consulta.Errores.Add(ex.Message);
        }
        return consulta;
    }
    public async static Task<ResultadoHttp> LoginConCaptcha(this ResultadoHttp resultado, CookieContainer Cookies, string Usuario, string Password, string Captcha)
    {

        if (!resultado.Ok)
        {
            Console.WriteLine("No ejecutado LoginConCaptcha");
            return resultado;
        }

        Console.WriteLine("Ejecuando PostPaginaInicial");
        var URL = $"https://cfdiau.sat.gob.mx/nidp/wsfed/ep?id=SATUPCFDiCon&sid=0&option=credential&sid=0";

        try
        {
            using var handler = new HttpClientHandler() { CookieContainer = Cookies, AllowAutoRedirect = false, AutomaticDecompression = DecompressionMethods.All };
            using var client = new HttpClient(handler);

            client.ClearHeaders().WithTimeOut(5).WithKeepAlive().MockBrowserHeaders().AccepTextHTML()
                .WithReferer("https://cfdiau.sat.gob.mx/nidp/wsfed/ep?id=SATUPCFDiCon&sid=0&option=credential&sid=0");


            Dictionary<string, string> dict = new()
                    {
                        {"option", "credential"},
                        { "Ecom_User_ID", Usuario },
                        { "Ecom_Password", Password },
                        { "userCaptcha", Captcha },
                        { "submit", "Enviar" }
                    };

            var result = await client.PostAsync(new Uri(URL), new FormUrlEncodedContent(dict));


            if (result.StatusCode == HttpStatusCode.OK)
            {
                string contenido = await result.Content.ReadAsStringAsync();
                // Este conteisdo es devulto cuando el login es exitoso
                if (contenido.IndexOf("window.location.href") > 0)
                {
                    resultado.Ok = true;
                    await HumanDelay();

                }
                else
                {
                    resultado.Ok = false;
                    resultado.Errores.Add("Contenido no encontrado");
                }
            }
        }
        catch (Exception ex)
        {
            resultado.Ok = false;
            resultado.Errores.Add(ex.Message);
            resultado.Exceptions.Add(ex);
        }
        return resultado;
    }


    /// <summary>
    /// Añade un deay simulado que es un humano
    /// </summary>
    /// <returns></returns>
    private static async Task HumanDelay()
    {
        Random rnd = new Random();
        int num = rnd.Next(250, 1000);
        await Task.Delay(num);
    }

    private static string ToUniversalIso8601(DateTime dateTime)
    {
        return dateTime.ToUniversalTime().ToString("u").Replace(" ", "T");
    }

    private static List<CfdiCancelado> GetCfdiCancelados(this HtmlNode Body)
    {

        var cancelaciones = new List<CfdiCancelado>();
        var x = Body.SelectNodes("form/main/div[@id='cuerpo_principal']/div[@id='cuerpo']/div[@id='ctl00_MainContent_PnlConsulta']/div[@id='ctl00_MainContent_UpnlResultados']/div[@id='ctl00_MainContent_PnlResultados']/div[@id='DivContenedor']/div[@id='ContenedorDinamico']/table[@id='ctl00_MainContent_tblResult']/tr");
        if (x!=null)
        {
            foreach (HtmlNode tr in x)
            {
                var datosCancelacion = tr.SelectNodes("td/span");

                if (datosCancelacion != null)
                {
                    CfdiCancelado c = new CfdiCancelado();
                    cancelaciones.Add(new CfdiCancelado()
                    {
                        FolioFiscal = datosCancelacion[1].InnerHtml,
                        RFCEmisor = datosCancelacion[2].InnerHtml,
                        Nombre = datosCancelacion[3].InnerHtml,
                        RFCReceptor = datosCancelacion[4].InnerHtml,
                        FechaEmisión = DateTime.Parse(datosCancelacion[5].InnerHtml).ToUniversalTime(),
                        FechaCertificación = DateTime.Parse(datosCancelacion[6].InnerHtml).ToUniversalTime(),
                        FechaSolicitud = DateTime.Parse(datosCancelacion[7].InnerHtml).ToUniversalTime(),
                        PacCertifico = datosCancelacion[8].InnerHtml,
                        Total = Decimal.Parse(datosCancelacion[9].InnerHtml.Substring(1)),
                        EfectoComprobante = datosCancelacion[10].InnerHtml,
                        EstatusSolicitud = datosCancelacion[11].InnerHtml,
                        Motivo = datosCancelacion[12].InnerHtml,
                        FolioSustitución = datosCancelacion[13].InnerHtml,

                    });

                }

            }
            
        }
        return cancelaciones;
    }

    private static async Task<ConsultaConciliacion> GetCfdiEmitidos(this ConsultaConciliacion consulta, CookieContainer Cookies,HtmlNode Body)
    {
        var ReferenceResultados = Body.SelectSingleNode("div[@id='ctl00_MainContent_PnlResultados']/div[@id='DivContenedor']");
        if (ReferenceResultados != null)
        { 
            var x = ReferenceResultados.SelectNodes("tr");
            if (x != null)
            {
                var cfdis = new List<string>();
                foreach (HtmlNode tr in x)
                {
                    var datosCfdiEmitido = tr.SelectNodes("td/span | td/a[@style='text-decoration:none']");
                    if (datosCfdiEmitido != null)
                    {
                        HtmlNode NodoDescargaXML = tr.SelectSingleNode("td/div/table/tr/td/span[@id='BtnDescarga']");
                        var c = new CfdiEmitido();
                        c.FolioFiscal = datosCfdiEmitido[0].InnerHtml;
                        c.RFCEmisor = datosCfdiEmitido[1].InnerHtml;
                        c.NombreEmisor = datosCfdiEmitido[2].InnerHtml;
                        c.RFCReceptor = datosCfdiEmitido[3].InnerHtml;
                        c.NombreReceptor = datosCfdiEmitido[4].InnerHtml;
                        c.FechaEmisión = DateTime.Parse(datosCfdiEmitido[5].InnerHtml).ToUniversalTime();
                        c.FechaCertificacion = DateTime.Parse(datosCfdiEmitido[6].InnerHtml).ToUniversalTime();
                        c.PacCertifico = datosCfdiEmitido[7].InnerHtml;
                        c.Total = Decimal.Parse(datosCfdiEmitido[8].InnerHtml.Substring(1));
                        c.EfectoComprobante = datosCfdiEmitido[9].InnerHtml;
                        c.EstatusCancelacion = datosCfdiEmitido[10].InnerHtml;
                        c.EstadoComprobante = datosCfdiEmitido[11].InnerHtml;
                        c.StatusProcesoCancelacion = datosCfdiEmitido[12].InnerHtml;
                        if (datosCfdiEmitido[13].InnerHtml != " ")
                        {
                            c.FechaProcesoCancelacion = DateTime.Parse(datosCfdiEmitido[13].InnerHtml).ToUniversalTime();
                        }
                        c.RfcCuentaTerceros = datosCfdiEmitido[14].InnerHtml;
                        c.Motivo = datosCfdiEmitido[15].InnerHtml;
                        c.FolioSustitución = datosCfdiEmitido[16].InnerHtml;
                        if(NodoDescargaXML!=null)
                        {
                            var url = NodoDescargaXML.GetAttributeValue("onclick", null).Split("return AccionCfdi('")[1].Split("','Recuperacion');")[0];
                            c.UrlDescarga = url;
                        }
                        cfdis.Add(JsonSerializer.Serialize(c));
                    }

                }
                consulta.PayLoad = cfdis;                
            }
        }
        consulta.Ok = true;
        return consulta;
    }

    private static async Task<ConsultaConciliacion> GetCfdiRecibidos(this ConsultaConciliacion consulta, CookieContainer Cookies, HtmlNode Body)
    {
        var ReferenceResultados = Body.SelectSingleNode("div[@id='ctl00_MainContent_PnlResultados']/div[@id='DivContenedor']/div[@id='ContenedorDinamico']/table[@id='ctl00_MainContent_tblResult']");
        var t= Body.SelectSingleNode("table[@id='ctl00_MainContent_tblResult']");
        if (ReferenceResultados != null)
        {
            var x = ReferenceResultados.SelectNodes("tr");
            if (x != null)
            {
                var cfdis = new List<string>();
                foreach (HtmlNode tr in x)
                {
                    var datosCfdiRecibido = tr.SelectNodes("td/span | td/a[@style='text-decoration:none']");
                    if (datosCfdiRecibido != null)
                    {

                        HtmlNode NodoDescargaXML = tr.SelectSingleNode("td/div/table/tr/td/span[@id='BtnDescarga']");
                        CfdiRecibido c= new CfdiRecibido();                        
                        c.FolioFiscal = datosCfdiRecibido[0].InnerHtml;
                        c.RFCEmisor = datosCfdiRecibido[1].InnerHtml;
                        c.NombreEmisor = datosCfdiRecibido[2].InnerHtml;
                        c.RFCReceptor = datosCfdiRecibido[3].InnerHtml;
                        c.NombreReceptor = datosCfdiRecibido[4].InnerHtml;
                        c.FechaEmisión = DateTime.Parse(datosCfdiRecibido[5].InnerHtml).ToUniversalTime();
                        c.FechaCertificacion = DateTime.Parse(datosCfdiRecibido[6].InnerHtml).ToUniversalTime();
                        c.PacCertifico = datosCfdiRecibido[7].InnerHtml;
                        c.Total = Decimal.Parse(datosCfdiRecibido[8].InnerHtml.Substring(1));
                        c.EfectoComprobante = datosCfdiRecibido[9].InnerHtml;
                        c.EstatusCancelacion = datosCfdiRecibido[10].InnerHtml;
                        c.EstadoComprobante = datosCfdiRecibido[11].InnerHtml;
                        c.StatusProcesoCancelacion = datosCfdiRecibido[12].InnerHtml;
                        if (datosCfdiRecibido[13].InnerHtml != " ")
                        {
                            c.FechaProcesoCancelacion = DateTime.Parse(datosCfdiRecibido[13].InnerHtml).ToUniversalTime();
                        }
                        c.RfcCuentaTerceros = datosCfdiRecibido[14].InnerHtml;
                        if(NodoDescargaXML!=null)
                        {
                            var url = NodoDescargaXML.GetAttributeValue("onclick", null).Split("return AccionCfdi('")[1].Split("','Recuperacion');")[0];
                            c.UrlDescarga = url;
                        }
                        cfdis.Add(JsonSerializer.Serialize(c));
                    }

                }
                consulta.PayLoad = cfdis;
            }
        }
        consulta.Ok = true;
        return consulta;
    }

    public async static Task<string?> GetDescargaCFDI(this string datos ,CookieContainer Cookies)
    {
        Console.WriteLine("descargando CFDI");
        var URL = $"https://portalcfdi.facturaelectronica.sat.gob.mx/{datos}";
            using var handler = new HttpClientHandler() { CookieContainer = Cookies, AllowAutoRedirect = false, AutomaticDecompression = DecompressionMethods.All };
            using var client = new HttpClient(handler);

            client.ClearHeaders().WithTimeOut(5).WithKeepAlive().MockBrowserHeaders().AccepTextHTML();

            var result = await client.GetAsync(new Uri(URL));

            if (result.StatusCode == HttpStatusCode.OK)
            {
                string contenido = await result.Content.ReadAsStringAsync();
                var htmlDoc = new HtmlDocument();
                htmlDoc.LoadHtml(contenido);
                var referenceBody = htmlDoc.DocumentNode;
                var limite = htmlDoc.DocumentNode.SelectSingleNode("body");
            if(limite!=null)
            {
                return null;
            }
            return contenido;
            }
        return null;
    }

    public async static Task<ResultadoHttp> GetDescargaPDF(this ResultadoHttp resultado, CookieContainer Cookies, string datos)
    {

        if (!resultado.Ok)
        {
            Console.WriteLine("No se descargo XML");
            return resultado;
        }

        datos = datos.Split("return AccionCfdi('")[1].Split("','Recuperacion');")[0];
        Console.WriteLine("descargando CFDI");
        var URL = $"https://portalcfdi.facturaelectronica.sat.gob.mx/{datos}";
        try
        {
            using var handler = new HttpClientHandler() { CookieContainer = Cookies, AllowAutoRedirect = false, AutomaticDecompression = DecompressionMethods.All };
            using var client = new HttpClient(handler);

            client.ClearHeaders().WithTimeOut(5).WithKeepAlive().MockBrowserHeaders().AccepTextHTML();

            var result = await client.GetAsync(new Uri(URL));

            if (result.StatusCode == HttpStatusCode.OK)
            {
                string contenido = await result.Content.ReadAsStringAsync();

            }
        }
        catch (Exception ex)
        {
            resultado.Ok = false;
            resultado.Errores.Add(ex.Message);
            resultado.Exceptions.Add(ex);
        }
        return resultado;
    }

    public async static Task<bool> GuardarXMLEmitido(this CfdiEmitido emitido ,string? xml)
    {
        var rutaXml = Environment.GetEnvironmentVariable("PathDescargaXML");
        if (!string.IsNullOrEmpty(xml))
        {
            string fullPath = Path.Combine(rutaXml, $"{emitido.FolioFiscal}.xml");
            File.WriteAllText(fullPath,xml);
            return true;
        }
        else
        {
            return false;
        }
    }
    public async static Task GuardarXMLRecibido(this CfdiRecibido recibido, string? xml)
    {
        var rutaXml = Environment.GetEnvironmentVariable("PathDescargaXML");
        if (!string.IsNullOrEmpty(xml))
        {
            string fullPath = Path.Combine(rutaXml, $"{recibido.FolioFiscal}.xml");
            File.WriteAllText(fullPath, xml);
        }
    }

    private async static Task VerificaSesion(this ResultadoHttp resultado, CookieContainer Cookies)
    {
        var timer = new PeriodicTimer(TimeSpan.FromSeconds(120));
        while (await timer.WaitForNextTickAsync())
        {
            Console.WriteLine("Ejecuando sessionVerifica");
            var URL = $"https://portalcfdi.facturaelectronica.sat.gob.mx/verificasesion.aspx?_={DateTime.Now.Ticks}";
            try
            {
                using var handler = new HttpClientHandler() { CookieContainer = Cookies, AllowAutoRedirect = false, AutomaticDecompression = DecompressionMethods.All };
                using var client = new HttpClient(handler);

                client.ClearHeaders().WithTimeOut(5).WithKeepAlive().MockBrowserHeaders().AccepTextHTML();

                var result = await client.GetAsync(new Uri(URL));

                if (result.StatusCode == HttpStatusCode.OK)
                {
                    string contenido = await result.Content.ReadAsStringAsync();
                    Console.WriteLine(contenido);
                }
                else
                {
                    timer.Dispose();
                }
            }
            catch (Exception ex)
            {
                resultado.Ok = false;
                resultado.Errores.Add(ex.Message);
                resultado.Exceptions.Add(ex);
            }
        }
    }

    public async static Task LogOut(CookieContainer Cookies)
    {
        Console.WriteLine("Ejecuando LogOut");
        var URL = $"https://portalcfdi.facturaelectronica.sat.gob.mx/logout.aspx?salir=y";
        try
        {
            using var handler = new HttpClientHandler() { CookieContainer = Cookies, AllowAutoRedirect = false, AutomaticDecompression = DecompressionMethods.All };
            using var client = new HttpClient(handler);

            client.ClearHeaders().WithTimeOut(5).WithKeepAlive().MockBrowserHeaders().AccepTextHTML();

            var result = await client.GetAsync(new Uri(URL));

            if (result.StatusCode == HttpStatusCode.OK)
            {
                string contenido = await result.Content.ReadAsStringAsync();
                if (contenido.IndexOf("Object moved to") > 0)
                {
                    await Console.Out.WriteLineAsync("Log Out existoso");
                }
            }
        }
        catch
        {
           
        }
    }
}

