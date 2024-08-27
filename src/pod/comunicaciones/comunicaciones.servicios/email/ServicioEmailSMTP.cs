using comunes.primitivas;
using comunicaciones.modelo;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;
using Serilog;
using Serilog.Core;

namespace comunicaciones.servicios.email;

public class ServicioEmailSMTP : IServicioEmail
{
    private readonly ILogger<ServicioEmailSMTP> logger;
    private IMessageBuilder _messageBuilder;
    private readonly IConfiguration configuration;
    private SMTPConfig _SMTPConfig;
    public ServicioEmailSMTP(ILogger<ServicioEmailSMTP> logger, IMessageBuilder messageBuilder, IOptions<SMTPConfig> SMTPConfig, IConfiguration configuration)
    {
        _SMTPConfig = SMTPConfig.Value;
        this.logger = logger;
        _messageBuilder = messageBuilder;
        this.configuration = configuration;
    }

    public async Task<Respuesta> Enviar(MensajeEmail msg)
    {
        Respuesta r = new Respuesta();
        try
        {
            msg.PlantillaCuerpo = _messageBuilder.FromTemplate(msg.PlantillaCuerpo, msg.JSONData);
            msg.PlantillaTema = _messageBuilder.FromTemplate(msg.PlantillaTema, msg.JSONData);

            var result = await EnviarCorreo(msg.PlantillaTema, msg.PlantillaCuerpo,
                msg.DireccionPara, msg.NombrePara, _SMTPConfig.FromEmail, _SMTPConfig.From);
            r.Ok = true;
            return r;

        }
        catch (Exception ex)
        {
            r.Error = new ErrorProceso()
            {
                Codigo = CodigosError.COMUNICACIONES_EMAIL_ERROR_ENVIO,
                Mensaje = "No se pudo enviar el correo",
                HttpCode = HttpCode.BadRequest
            };
            r.Ok = false;
            r.HttpCode = HttpCode.BadRequest;
            logger.LogDebug(ex, "ServicioEmailSMTP {msg}", ex.Message);
            return r;
        };
    }

    public async Task<bool> EnviarCorreo(string subject, string body, string email, string nombre, string emailDe, string nombreDe)
    {
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress(nombreDe, emailDe));
        message.To.Add(new MailboxAddress(nombre, email));
        message.Subject = subject;

        var bodyBuilder = new BodyBuilder
        {
            HtmlBody = body
        };

        message.Body = bodyBuilder.ToMessageBody();

        using (var client = new SmtpClient())
        {
            client.Connect(_SMTPConfig.Server, _SMTPConfig.Port, MailKit.Security.SecureSocketOptions.Auto);

            if (_SMTPConfig.Authenticated)
            {
                client.Authenticate(_SMTPConfig.User, _SMTPConfig.Password);
            }
            await client.SendAsync(message);
            client.Disconnect(true);
        }

        return true;
    }
}

