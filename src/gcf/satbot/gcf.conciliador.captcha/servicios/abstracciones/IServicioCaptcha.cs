using System;
using System.Threading.Tasks;

namespace gcf.conciliador.captcha.servicios;

public interface IServicioCaptcha
{
    /// <summary>
    /// Obtiene la interpretaci[on del capcha para un Id de conciliacion
    /// </summary>
    /// <param name="Id"></param>
    /// <returns></returns>
    Task<string?> ObtieneLecturaCaptcha(Guid Id);


    /// <summary>
    /// Envvia un captcha para verificacion para un Id unico de conciliacion
    /// </summary>
    /// <param name="rfc"></param>
    /// <param name="imagenBase64"></param>
    /// <returns></returns>
    Task<Guid> EnviaCaptcha(string rfc, string imagenBase64);
}
