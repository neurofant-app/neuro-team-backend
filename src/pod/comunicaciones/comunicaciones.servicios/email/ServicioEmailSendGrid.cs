using Microsoft.Extensions.Options;
using SendGrid.Helpers.Mail;
using SendGrid;
using comunes.primitivas;
using comunicaciones.modelo;
using Microsoft.Extensions.Logging;
namespace comunicaciones.servicios.email;

public class ServicioEmailSendGrid : IServicioEmail
{
    private readonly ILogger<ServicioEmailSendGrid> logger;
    private IMessageBuilder _messageBuilder;
    private SMTPConfig _SMTPConfig;

    public ServicioEmailSendGrid(ILogger<ServicioEmailSendGrid> logger, IMessageBuilder messageBuilder, IOptions<SMTPConfig> SMTPConfig)
    {
        _SMTPConfig = SMTPConfig.Value;
        this.logger = logger;
        _messageBuilder = messageBuilder;
    }

    public async Task<Respuesta> Enviar(MensajeEmail msg)
    {
        Respuesta r = new Respuesta();
        try
        {
            logger.LogDebug($"Enviando Email"); 
            msg.PlantillaCuerpo = _messageBuilder.FromTemplate(msg.PlantillaCuerpo, msg.JSONData);
            msg.PlantillaTema = _messageBuilder.FromTemplate(msg.PlantillaTema, msg.JSONData);

            var result = await EnviarCorreo(msg.PlantillaTema, msg.PlantillaCuerpo,
                msg.DireccionPara, msg.NombrePara,
                msg.DireccionDe ?? _SMTPConfig.FromEmail, msg.NombreDe ?? _SMTPConfig.From);
            r.Ok = result;
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
            logger.LogDebug(ex, "ServicioEmailSendGrid {msg}", ex.Message);
            return r;
        };
    }

    public async Task<bool> EnviarCorreo(string subject, string body, string email, string nombre, string emailDe, string nombreDe)
    {

        var apiKey = _SMTPConfig.SendgridKey;
        var client = new SendGridClient(apiKey);
        var from = new EmailAddress(emailDe, nombreDe);
        var to = new EmailAddress(email, nombre);
        var msg = MailHelper.CreateSingleEmail(from, to, subject, null, body);
        var response = await client.SendEmailAsync(msg);

        return response.IsSuccessStatusCode;
    }

}
