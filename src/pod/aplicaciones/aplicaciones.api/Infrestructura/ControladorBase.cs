using aplicaciones.services.proxy.implementations;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;

namespace aplicaciones.api;

[ApiController]
public class ControladorJwt : ControllerBase
{
    private readonly ILogger _logger;

    public ControladorJwt(ILogger logger)
    {
        this._logger = logger;
    }

    /// <summary>
    /// DEculeve el Id del usaurio en sesión
    /// </summary>
    /// <returns></returns>
    protected Guid UsuarioId()
    {
        return Guid.Empty;
    }
    /// <summary>
    /// Valida si una cadena cumple con los requisitos de seguridad para que sea contraseña
    /// </summary>
    /// <param name="contraseñaSinVerificar"></param>
    /// <returns></returns>
    protected bool ValidaPassword(string contraseñaSinVerificar)
    {
       _logger.LogDebug("ControllerBase - Validando Password");
        Regex letras = new Regex(@"[a-zA-z]");

        Regex numeros = new Regex(@"[0-9]");

        Regex caracEsp = new Regex("[!\"#\\$%&'()*+,-./:;=?@\\[\\]^_`{|}~]");

        if (!letras.IsMatch(contraseñaSinVerificar))
        {
            return false;
        }
        if (!numeros.IsMatch(contraseñaSinVerificar))
        {
            return false;
        }

        if (!caracEsp.IsMatch(contraseñaSinVerificar))
        {
            return false;
        }
        if (contraseñaSinVerificar.Length < 8)
        {
            return false;
        }
        return true;
    }

    /// <summary>
    /// Determinar si una cadena tiene un formato válido de email.
    /// </summary>
    /// <param name="emailSinValidar"></param>
    /// <returns></returns>
    protected bool ValidaEmail(string emailSinValidar)
    {
        _logger.LogDebug("ControllerBase - Validando Email");
        Regex email = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
        if (!email.IsMatch(emailSinValidar))
        {
            return false;
        }
        return true;
    }

}