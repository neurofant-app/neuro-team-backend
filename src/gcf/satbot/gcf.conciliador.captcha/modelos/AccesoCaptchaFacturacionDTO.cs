using System;
using System.Collections.Generic;

namespace gcf.conciliador.captcha;

public class AccesoCaptchaFacturacionDTO
{
    /// <summary>
    /// Identificador único de la solicitud de acceso
    /// </summary>
    public Guid Id { get; set; } = Guid.NewGuid();

    /// <summary>
    /// RFC que está solicitando el acceso para buscar sus números de teléfono
    /// </summary>
    public required string RFC { get; set; }
    // R 20
    // I 

    /// <summary>
    /// Imagene del captacha en base 64
    /// </summary>
    public required string CaptchaBase64 { get; set; }
    // R MAXIMO
    // I 

    /// <summary>
    /// Fecha de la solictud en UTC
    /// </summary>
    public DateTime Fecha { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// REspuesta humana al captcha
    /// </summary>
    public string? RespuestaHumana { get; set; }
    // 10
    /// <summary>
    /// Telefono al que se envio el captcha
    /// </summary>
    public List<string> Telefonos { get; set; }
}
