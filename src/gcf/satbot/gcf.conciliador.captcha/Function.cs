using CloudNative.CloudEvents;
using gcf.conciliador.captcha.servicios;
using Google.Cloud.Functions.Framework;
using Google.Events.Protobuf.Cloud.PubSub.V1;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using sat.bot.comun.mocks;
using sat.bot.ejecutor;
using sat.bot.modelo;
using sat.bot.services;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace gcf.conciliador.captcha;

    public class Function : ICloudEventFunction<MessagePublishedData>
    {
    private readonly ILogger<Function> logger;

    public Function(ILogger<Function> logger)
    {
        this.logger = logger;
    }

    public  async Task HandleAsync(CloudEvent cloudEvent, MessagePublishedData data, CancellationToken cancellationToken)
        {
            Conciliador conciliador = new Conciliador();
            AdministradorSecretosMock administradorSecretos = new();
            //GestorArchivosGCP gestorArchivos =new(logger, new ConfiguracionGCP() { Bucket= Environment.GetEnvironmentVariable("BucketName"), ProyectoId="2"});
            GestorArchivosMock gestorArchivos=new GestorArchivosMock();
            ServicioCaptchaAPI servicioCaptcha = new();          
            ServicioSatBotMock servicioSatBot = new();
        var url = Environment.GetEnvironmentVariable("urlCaptcha");
            string content = data.Message.TextData;
            var tarea = JsonConvert.DeserializeObject<TareaConciliacion>(content);
        var x = await conciliador.Conciliar(tarea, gestorArchivos, servicioSatBot, administradorSecretos, servicioCaptcha, url);
        return ;
        }
    }
