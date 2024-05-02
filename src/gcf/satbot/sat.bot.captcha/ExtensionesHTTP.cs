using System.Net;

namespace sat.bot.captcha;

public static class ExtensionesHTTP
{
    public static string Imagenbase64Jpeg(string folder, string imagestr)
    {

        byte[] bytes = Convert.FromBase64String(imagestr);

        string file = Path.Combine(folder, $"{Guid.NewGuid().ToString()}.jpg");
        File.WriteAllBytes(file, bytes);
        return file;
    }

    public static int MinutosAMilisegundos(this int Minutos)
    {
        return Minutos * 60 * 1000;
    }

    public static HttpClient MockBrowserHeaders(this HttpClient client)
    {
        client.DefaultRequestHeaders.Add("Accept-Language", "es-MX,es;q=0.5");
        client.DefaultRequestHeaders.Add("Accept-Encoding", "gzip, deflate");
        client.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (Windows NT 6.3; WOW64; Trident/7.0; rv:11.0) like Gecko");
        client.Timeout = TimeSpan.FromMinutes(5);
        return client;
    }

    public static HttpClient AccepTextHTML(this HttpClient client)
    {
        client.DefaultRequestHeaders.Add("Accept", "text/html, application/xhtml+xml, */*");
        return client;
    }

    public static HttpClient WithKeepAlive(this HttpClient client)
    {
        client.DefaultRequestHeaders.ConnectionClose = true;
        return client;
    }

    public static HttpClient WithReferer(this HttpClient client, string referer)
    {
        client.DefaultRequestHeaders.Add("Referer", referer);
        return client;
    }

    public static HttpClient WithTimeOut(this HttpClient client, int Minutes)
    {
        client.Timeout = TimeSpan.FromMinutes(Minutes);
        return client;
    }

    public static HttpClient ClearHeaders(this HttpClient client)
    {
        client.DefaultRequestHeaders.Clear();
        return client;
    }


    public static HttpWebRequest BrowserRequest(this HttpClient httpClient, string URL, CookieContainer cookies, int MinutosTimeout = 5)
    {
        HttpWebRequest rq = (HttpWebRequest)WebRequest.Create(URL);
        rq.Accept = "text/html, application/xhtml+xml, */*";
        rq.Headers.Add("Accept-Encoding: gzip, deflate");
        rq.Headers.Add("Accept-Language: es-MX,es;q=0.5");
        rq.UserAgent = "Mozilla/5.0 (Windows NT 6.3; WOW64; Trident/7.0; rv:11.0) like Gecko";
        rq.Timeout = MinutosTimeout.MinutosAMilisegundos() ;
        rq.AllowAutoRedirect = false;
        rq.CookieContainer = cookies;
        rq.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
        rq.KeepAlive = true;
        return rq;
    }
}
